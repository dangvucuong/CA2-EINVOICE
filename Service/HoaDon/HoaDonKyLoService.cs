using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common;
using Contracts.Service.HoaDon;
using Microsoft.Extensions.DependencyInjection;
using Model.Base;
using Model.Enum;
using Model.RemoteSigning;
using Model.Request.Base;
using Model.Request.HoaDon;
using Model.Request.Hub;
using Model.Request.ToKhai;
using Model.Respone;
using Model.Respone.ApiSign;
using Model.Respone.HoaDon;
using Model.Respone.Xml;
using Model.Static;
using Model.Table;
using Service.Base;
using Service.Hub;

namespace Service.HoaDon
{
    public class HoaDonKyLoService : CRUDService<hoa_don>, IHoaDonKyLoService
    {
        private IHoaDonService _hoaDonService;
        HoaDonPhatHanhHub _hoaDonPhatHanhHub;
        ProcessHub _processHub;
        private IHoaDonLogService _hoaDonLogService;
        public HoaDonKyLoService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._hoaDonService = _serviceWrapper.HoaDon.HoaDon;
            this._processHub = _serviceProvider.GetRequiredService<ProcessHub>();
            this._hoaDonPhatHanhHub = _serviceProvider.GetRequiredService<HoaDonPhatHanhHub>();
            _hoaDonLogService = _serviceWrapper.HoaDon.HoaDonLog;
        }
        private async Task<ProcessChangedModel> CreateXmlStatusIniAsync(string progress_id, ProcessStatusRespone<ProcessStepDataBase> processStatusModel, string userId, int total)
        {
            processStatusModel.progress_id = progress_id;
            processStatusModel.steps = new List<ProcessStepRespone<ProcessStepDataBase>>();
            processStatusModel.steps.Add(new ProcessStepRespone<ProcessStepDataBase>()
            {
                id = 1,
                name = "Tạo XML",
                data = new ProcessStepDataBase()
                {
                    total = total,
                    success = 0,
                    error = 0,
                    is_done = false,
                    is_processing = true
                }
            });
            processStatusModel.steps.Add(new ProcessStepRespone<ProcessStepDataBase>()
            {
                id = 2,
                name = "Ký số",
                data = new ProcessStepDataBase()
                {
                    total = total,
                    success = 0,
                    error = 0,
                    is_done = false,
                    is_processing = false
                }
            });
            processStatusModel.steps.Add(new ProcessStepRespone<ProcessStepDataBase>()
            {
                id = 3,
                name = "Gửi phát hành",
                data = new ProcessStepDataBase()
                {
                    total = total,
                    success = 0,
                    error = 0,
                    is_done = false,
                    is_processing = false
                }
            });
            // processStatusModel.steps.Add(new ProcessStepRespone<ProcessStepDataBase>()
            // {
            //     id = 4,
            //     name = "Nhận kết quả phát hành",
            //     data = new ProcessStepDataBase()
            //     {
            //         total = total,
            //         success = 0,
            //         error = 0,
            //         is_done = false,
            //         is_processing = false
            //     }
            // });
            var processChangedModel = new ProcessChangedModel()
            {
                user_id = userId,
                processStatus = processStatusModel
            };
            await _processHub.OnProcessChangedAsync(processChangedModel);
            return processChangedModel;
        }
        public async Task<IEnumerable<HoaDonCreateXmlKySoRespone>> CreateXmlVaPhatHanhsAsync(HoaDonKyLoRequest request, bool isRunBackgroundForRS = false)
        {

            var hoaDons = await _hoaDonService.SelectByIdsAsync(request.ids);
            var hoaDonsMTT = hoaDons.Where(x => x.hoa_don_hinh_thuc_code == "M").ToList();
            var hoaDonsThuong = hoaDons.Where(x => x.hoa_don_hinh_thuc_code != "M").ToList();
            //nếu tất cả là hóa đơn MTT -> bảng kê
            if (hoaDonsThuong.Count == 0 && hoaDonsMTT.Count > 0)
            {
                return await this.CreateXmlVaPhatHanhsMTTBangKeAsync(request, hoaDonsMTT, isRunBackgroundForRS);
            }
            var userId = this.GetCurrentUserId();
            user userSerialInfo = null;
            if (request.rs_ma_but_ky.ConvertToString() != "")
            {
                userSerialInfo = await _serviceWrapper.User.User.SelectByMaButKyAsync(request.rs_ma_but_ky);
                if (userSerialInfo != null) userId = userSerialInfo.id;
            }
            else
            {
                userSerialInfo = await _serviceWrapper.User.User.SelectByIdAsync(userId);

            }
            var processStatusModel = request.progress_id.ConvertToString() != "" ? new ProcessStatusRespone<ProcessStepDataBase>() : null;
            var processChangedModel = request.progress_id.ConvertToString() != "" ? new ProcessChangedModel() : null;
            if (processStatusModel != null)
            {
                processChangedModel = await CreateXmlStatusIniAsync(request.progress_id, processStatusModel, userId.ToString(), request.ids.Count);
            }
            var lockStatus = new SemaphoreSlim(1, 1);

            var result = new List<HoaDonCreateXmlKySoRespone>();
            var taskMTT = this.CreateXmlsMTTAsync(hoaDonsMTT, lockStatus, processChangedModel);
            var taskThuong = this.CreateXmlsThuongAsync(hoaDonsThuong, lockStatus, processChangedModel);
            await Task.WhenAll(taskMTT, taskThuong);
            result.AddRange(taskMTT.Result);
            result.AddRange(taskThuong.Result);
            ProcessStatusRespone<ProcessStepDataBase> processStatus = null;
            if (processChangedModel != null)
            {
                processStatus = (ProcessStatusRespone<ProcessStepDataBase>)processChangedModel.processStatus;
                processStatus.steps[0].data.is_done = true;
                processStatus.steps[0].data.is_processing = false;
                await _processHub.OnProcessChangedAsync(processChangedModel);
            }

            if (isRunBackgroundForRS && userSerialInfo.rs_ma_but_ky.ConvertToString() != "")
            {
                await KySoRemoteSigningThenPhatHanhBackgroundListAsync(hoaDons.ToList(), result, userSerialInfo.serial_number, lockStatus, processChangedModel);
                return result;
            }
            else
            {
                //check nếu là HSM, RemoteSiging -> Ký số và phát hành tiếp từ back-end
                //nếu k thì gửi lại client để ký bằng tool
                var pageSize = 10;
                var pageCount = (int)Math.Ceiling((double)result.Count() / pageSize);
                if (processStatus != null)
                {

                    processStatus.steps[1].data.is_processing = true;
                    processStatus.steps[2].data.is_processing = true;
                }
                for (int pageIndex = 0; pageIndex < pageCount; pageIndex++)
                {
                    var pageItems = result.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                    var tasks = pageItems.Select(async hoaDonXmlResult =>
                    {
                        var hoaDonId = hoaDonXmlResult.id;
                        var base64 = hoaDonXmlResult.xml_base64;
                        var hoaDon = hoaDons.FirstOrDefault(x => x.id == hoaDonId);
                        if (hoaDon != null && base64.ConvertToString() != "")
                        {
                            if (userSerialInfo.is_hsm_signing)
                                await this.SignAndPhatHanhHSMAsync(hoaDon, base64, userSerialInfo.serial_number, lockStatus, processChangedModel, null);
                            if (userSerialInfo.rs_ma_but_ky.ConvertToString() != "")
                                await this.SignAndPhatHanhRemoteSigningAsync(hoaDon, base64, userSerialInfo.serial_number, lockStatus, processChangedModel);
                        }


                    }).ToList();
                    await Task.WhenAll(tasks);
                }
                if (processStatus != null)
                {

                    processStatus.steps[1].data.is_processing = false;
                    processStatus.steps[2].data.is_processing = false;
                    processStatus.steps[1].data.is_done = true;
                    processStatus.steps[2].data.is_done = true;
                    await _processHub.OnProcessChangedAsync(processChangedModel);
                }


                return result;
            }

        }
        public async Task<bool> SignAndPhatHanhRemoteSigningAsync(hoa_don hoaDon, string base64, string serial, SemaphoreSlim lockStatus, ProcessChangedModel processChangedModel)
        {
            var kySoRemoteSigningResult = await this.KySoRemoteSigningAsync(hoaDon, base64, hoaDon.donvi_ma_dv, serial);
            if (processChangedModel != null && kySoRemoteSigningResult != null)
            {
                if (lockStatus != null) await lockStatus.WaitAsync();
                var processStatus = (ProcessStatusRespone<ProcessStepDataBase>)processChangedModel.processStatus;
                processStatus.steps[1].data.is_processing = true;
                processStatus.steps[1].data.success += kySoRemoteSigningResult.is_success ? 1 : 0;
                processStatus.steps[1].data.error += kySoRemoteSigningResult.is_success ? 0 : 1;
                await _processHub.OnProcessChangedAsync(processChangedModel);
                if (lockStatus != null) lockStatus.Release();
            }
            if (kySoRemoteSigningResult.is_success)
            {
                //phát hành
                FunctionResult<HoaDonPhatHanhRespone> phatHanhResult = null;
                if (hoaDon.hoa_don_hinh_thuc_code == "M")
                {
                    phatHanhResult = await _hoaDonService.PhatHanhMTTAsync(new HoaDonPhatHanhRequest()
                    {
                        id = hoaDon.id,
                        signed_text = kySoRemoteSigningResult.data
                    }, hoaDon);

                }
                else
                {
                    phatHanhResult = await _hoaDonService.PhatHanhAsync(new HoaDonPhatHanhRequest()
                    {
                        id = hoaDon.id,
                        signed_text = kySoRemoteSigningResult.data
                    });

                }
                if (processChangedModel != null && phatHanhResult != null)
                {
                    if (lockStatus != null) await lockStatus.WaitAsync();
                    var processStatus = (ProcessStatusRespone<ProcessStepDataBase>)processChangedModel.processStatus;
                    processStatus.steps[2].data.is_processing = true;
                    processStatus.steps[2].data.success += phatHanhResult.is_success ? 1 : 0;
                    processStatus.steps[2].data.error += phatHanhResult.is_success ? 0 : 1;
                    await _processHub.OnProcessChangedAsync(processChangedModel);
                    if (lockStatus != null) lockStatus.Release();
                }
                return true;
            }
            return true;
        }
        public async Task<bool> SignAndPhatHanhHSMAsync(hoa_don hoaDon, string base64, string serial, SemaphoreSlim lockStatus, ProcessChangedModel processChangedModel, string? bienBanBase64)
        {
            try
            {
                var kySoHSMResult = await this.KySoHSMAsync(hoaDon, base64, hoaDon.donvi_ma_dv, serial, bienBanBase64);

                if (kySoHSMResult != null)
                {
                    if (processChangedModel != null)
                    {
                        if (lockStatus != null) await lockStatus.WaitAsync();
                        var processStatus = (ProcessStatusRespone<ProcessStepDataBase>)processChangedModel.processStatus;
                        processStatus.steps[1].data.is_processing = true;
                        processStatus.steps[1].data.success += kySoHSMResult.Macode == 1 ? 1 : 0;
                        processStatus.steps[1].data.error += kySoHSMResult.Macode == 1 ? 0 : 1;
                        await _processHub.OnProcessChangedAsync(processChangedModel);
                        if (lockStatus != null) lockStatus.Release();
                    }
                    if (kySoHSMResult.Macode == 1)
                    {


                        //phát hành
                        FunctionResult<HoaDonPhatHanhRespone> phatHanhResult = null;
                        if (hoaDon.hoa_don_hinh_thuc_code == "M")
                        {
                            phatHanhResult = await _hoaDonService.PhatHanhMTTAsync(new HoaDonPhatHanhRequest()
                            {
                                id = hoaDon.id,
                                signed_text = kySoHSMResult.SignedData
                            }, hoaDon);

                        }
                        else
                        {
                            phatHanhResult = await _hoaDonService.PhatHanhAsync(new HoaDonPhatHanhRequest()
                            {
                                id = hoaDon.id,
                                signed_text = kySoHSMResult.SignedData
                            });
                        }
                        if (processChangedModel != null && phatHanhResult != null)
                        {
                            if (lockStatus != null) await lockStatus.WaitAsync();
                            var processStatus = (ProcessStatusRespone<ProcessStepDataBase>)processChangedModel.processStatus;
                            processStatus.steps[2].data.is_processing = true;
                            processStatus.steps[2].data.success += phatHanhResult.is_success ? 1 : 0;
                            processStatus.steps[2].data.error += phatHanhResult.is_success ? 0 : 1;
                            await _processHub.OnProcessChangedAsync(processChangedModel);
                            if (lockStatus != null) lockStatus.Release();
                        }
                    }

                    return true;
                }
                return false;
            }
            catch (System.Exception ex)
            {
                return false;
            }

        }
        public async Task<ApiSignResultModel> KySoHSMAsync(hoa_don hoaDon, string base64, string mst, string serial, string? bienBanBase64)
        {
            //  var kySoResulit = await _serviceWrapper.ApiSignHoaDon.SignHoaDonAsync(model.id, userSerialInfo.serial_number);
            if (base64.ConvertToString() != string.Empty)
            {
                var signResultModel = await _serviceWrapper.ApiSignHoaDon.SignAsync(base64, mst, serial);
                if (signResultModel != null && signResultModel.Macode == 1)
                {
                    signResultModel.HoadonId = hoaDon.id;
                    if (bienBanBase64 != null && bienBanBase64 != string.Empty)
                    {
                        var signResultModelBienban = await _serviceWrapper.ApiSignHoaDon.SignAsync(bienBanBase64, mst, serial);
                        if (signResultModelBienban != null && signResultModel.Macode == 1)
                        {
                            await _serviceWrapper.HoaDon.HoaDon.UpdteKySoSuccessAsync(new Model.Request.ToKhai.HoaDonPhatHanhRequest()
                            {
                                id = hoaDon.id,
                                signed_text = signResultModel.SignedData,
                                bienBanSignedText = signResultModelBienban.SignedData
                            });
                        }
                    }
                    else
                    {
                        await _serviceWrapper.HoaDon.HoaDon.UpdteKySoSuccessAsync(new Model.Request.ToKhai.HoaDonPhatHanhRequest()
                        {
                            id = hoaDon.id,
                            signed_text = signResultModel.SignedData

                        });
                    }


                }

                return signResultModel;
            }
            return null;
        }
        public async Task<FunctionResult<string>> KySoRemoteSigningAsync(hoa_don hoaDon, string base64, string mst, string serial)
        {
            try
            {
                var kySoRequest = new KySoRequest(serial, mst, base64, hoaDon.nguoi_mua_email.ConvertToString());
                kySoRequest.hoa_don_id = hoaDon.id;
                kySoRequest.ma_tra_cuu = hoaDon.ma_tra_cuu.ConvertToString();
                kySoRequest.KHHDon = hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu;
                kySoRequest.so_hoa_don = hoaDon.ma_so_hoa_don ?? 0;

                var guiYeucauResult = await _serviceWrapper.RemoteSigningSerivce.KySoAsync(kySoRequest);
                if (guiYeucauResult.is_success)
                {
                    var code = guiYeucauResult.data;
                    var cts = new CancellationTokenSource();
                    var ketQuaKy = await _serviceWrapper.RemoteSigningSerivce.TryGetKetQuaKyThenClearAsync(code, "", cts.Token);
                    if (ketQuaKy.is_success)
                    {
                        await _serviceWrapper.HoaDon.HoaDon.UpdteKySoSuccessAsync(new Model.Request.ToKhai.HoaDonPhatHanhRequest()
                        {
                            id = hoaDon.id,
                            signed_text = base64
                        });
                    }
                    return ketQuaKy;
                }

                return new ErrorResult<string>(guiYeucauResult.message);
            }
            catch (System.Exception ex)
            {
                return new ErrorResult<string>(ex.Message);
            }
        }
        private async Task<IEnumerable<HoaDonCreateXmlKySoRespone>> CreateXmlsMTTAsync(List<hoa_don> hoaDons, SemaphoreSlim lockStatus, ProcessChangedModel processChangedModel)
        {
            var result = new List<HoaDonCreateXmlKySoRespone>();
            var pageSize = 10;
            var pageCount = (int)Math.Ceiling((double)hoaDons.Count() / pageSize);

            for (int pageIndex = 0; pageIndex < pageCount; pageIndex++)
            {
                var pageItems = hoaDons.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                var tasks = pageItems.Select(async hoaDon =>
                {
                    var base64Result = await _hoaDonService.CreateBase64MTTAsync(hoaDon);
                    if (base64Result.is_success)
                    {
                        result.Add(new HoaDonCreateXmlKySoRespone()
                        {
                            id = hoaDon.id,
                            xml_base64 = base64Result.data
                        });

                    }
                    if (processChangedModel != null)
                    {
                        await lockStatus.WaitAsync();
                        var processStatus = (ProcessStatusRespone<ProcessStepDataBase>)processChangedModel.processStatus;
                        processStatus.steps[0].data.success += base64Result.is_success ? 1 : 0;
                        processStatus.steps[0].data.error += base64Result.is_success ? 0 : 1;
                        await _processHub.OnProcessChangedAsync(processChangedModel);
                        lockStatus.Release();
                    }

                }).ToList();
                await Task.WhenAll(tasks);

            }
            return result;
        }
        private async Task<IEnumerable<HoaDonCreateXmlKySoRespone>> CreateXmlsThuongAsync(List<hoa_don> hoaDons, SemaphoreSlim lockStatus, ProcessChangedModel processChangedModel)
        {
            var result = new List<HoaDonCreateXmlKySoRespone>();
            var pageSize = 10;
            var pageCount = (int)Math.Ceiling((double)hoaDons.Count() / pageSize);

            for (int pageIndex = 0; pageIndex < pageCount; pageIndex++)
            {
                var pageItems = hoaDons.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                var tasks = pageItems.Select(async hoaDon =>
                {
                    var xmlResult = await _hoaDonService.CreateXmlKySoAsync(hoaDon);
                    if (xmlResult.is_success)
                    {
                        var xml = xmlResult.data;
                        var base64 = xmlResult.data.ConvertToBase64();
                        result.Add(new HoaDonCreateXmlKySoRespone()
                        {
                            id = hoaDon.id,
                            xml_base64 = base64
                        });

                    }
                    if (processChangedModel != null)
                    {
                        await lockStatus.WaitAsync();
                        var processStatus = (ProcessStatusRespone<ProcessStepDataBase>)processChangedModel.processStatus;
                        processStatus.steps[0].data.success += xmlResult.is_success ? 1 : 0;
                        processStatus.steps[0].data.error += xmlResult.is_success ? 1 : 0;
                        await _processHub.OnProcessChangedAsync(processChangedModel);
                        lockStatus.Release();
                    }

                }).ToList();
                await Task.WhenAll(tasks);

            }
            return result;
        }

        public async Task<FunctionResult<string>> KySoRemoteSigningBackgroundAsync(hoa_don hoaDon, string base64, string mst, string serial)
        {
            try
            {
                var kySoRequest = new KySoRequest(serial, mst, base64, hoaDon.nguoi_mua_email.ConvertToString());
                kySoRequest.hoa_don_id = hoaDon.id;
                kySoRequest.ma_tra_cuu = hoaDon.ma_tra_cuu.ConvertToString();
                kySoRequest.KHHDon = hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu;
                kySoRequest.so_hoa_don = hoaDon.ma_so_hoa_don ?? 0;
                var userId = this.GetCurrentUserId();
                var guiYeucauResult = await _serviceWrapper.RemoteSigningSerivce.KySoAsync(kySoRequest);
                if (guiYeucauResult.is_success)
                {
                    var code = guiYeucauResult.data;
                    if (code != "-2")
                    {
                        var user_id = this.GetCurrentUserId();
                        await _serviceWrapper.HoaDon.RsYeuCauKy.SaveYeuCauKyAsync(code, user_id.ToString(), Model.Enum.e_rs_yeu_cau_ky_type.KY_SO_HOA_DON, hoaDon.id.ToString());

                        // _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                        //      {
                        //          await TryGetAndProcesssQuaKySoRsBackgroundAsync(code, hoaDon.id, base64, userId);
                        //      });
                        return new SuccessResult<string>(code);
                    }

                }

                return new ErrorResult<string>(guiYeucauResult.message);
            }
            catch (System.Exception ex)
            {
                return new ErrorResult<string>(ex.Message);
            }
        }


        public async Task<FunctionResult<string>> KySoThongDiep206MTTRemoteSigningBackgroundAsync(hoa_don hoaDon, string base64thongdiep, string mst, string serial)
        {
            try
            {
                var kySoRequest = new KySoRequest(serial, mst, base64thongdiep, hoaDon.nguoi_mua_email.ConvertToString());
                kySoRequest.hoa_don_id = hoaDon.id;
                kySoRequest.ma_tra_cuu = hoaDon.ma_tra_cuu.ConvertToString();
                kySoRequest.KHHDon = hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu;
                kySoRequest.so_hoa_don = hoaDon.ma_so_hoa_don ?? 0;
                var userId = this.GetCurrentUserId();
                var guiYeucauResult = await _serviceWrapper.RemoteSigningSerivce.KySoAsync(kySoRequest);
                if (guiYeucauResult.is_success)
                {
                    var code = guiYeucauResult.data;
                    if (code != "-2")
                    {
                        var user_id = this.GetCurrentUserId();
                        await _serviceWrapper.HoaDon.RsYeuCauKy.SaveYeuCauKyAsync(code, user_id.ToString(), Model.Enum.e_rs_yeu_cau_ky_type.KY_SO_HOA_DON, hoaDon.id.ToString());

                        // _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                        //      {
                        //          await TryGetAndProcesssQuaKySoRsBackgroundAsync(code, hoaDon.id, base64, userId);
                        //      });
                        return new SuccessResult<string>(code);
                    }

                }

                return new ErrorResult<string>(guiYeucauResult.message);
            }
            catch (System.Exception ex)
            {
                return new ErrorResult<string>(ex.Message);
            }
        }

        public async Task<FunctionResult<string>> KySoRemoteSigningThenPhatHanhBackgroundAsync(hoa_don hoaDon, string base64, string mst, string serial, SemaphoreSlim lockStatus, ProcessChangedModel processChangedModel)
        {

            var kySoRequest = new KySoRequest(serial, mst, base64, hoaDon.nguoi_mua_email.ConvertToString());
            kySoRequest.hoa_don_id = hoaDon.id;
            kySoRequest.ma_tra_cuu = hoaDon.ma_tra_cuu.ConvertToString();
            kySoRequest.KHHDon = hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu;
            kySoRequest.so_hoa_don = hoaDon.ma_so_hoa_don ?? 0;
            var userId = this.GetCurrentUserId();
            var guiYeucauResult = await _serviceWrapper.RemoteSigningSerivce.KySoAsync(kySoRequest);
            if (guiYeucauResult.is_success)
            {
                var code = guiYeucauResult.data;
                if (code != "-2")
                {
                    await _serviceWrapper.HoaDon.RsYeuCauKy.SaveYeuCauKyAsync(code, userId.ToString(), Model.Enum.e_rs_yeu_cau_ky_type.KY_SO_VA_PHAT_HANH_HOA_DON, hoaDon.id.ToString());
                    return new SuccessResult<string>(code);
                }


            }

            return new ErrorResult<string>(guiYeucauResult.message);
        }


        public async Task<FunctionResult<string>> KySoRemoteSigningThenPhatHanhBackgroundListAsync(List<hoa_don> hoaDons, List<HoaDonCreateXmlKySoRespone> listXmlResult, string serial_number, SemaphoreSlim lockStatus, ProcessChangedModel processChangedModel)
        {
            var userId = this.GetCurrentUserId();
            var tasks = listXmlResult.Select(async hoaDonXmlResult =>
                 {
                     var hoaDonId = hoaDonXmlResult.id;
                     var base64 = hoaDonXmlResult.xml_base64;
                     var hoaDon = hoaDons.FirstOrDefault(x => x.id == hoaDonId);
                     if (hoaDon != null)
                     {
                         await this.KySoRemoteSigningThenPhatHanhBackgroundAsync(hoaDon, base64, hoaDon.donvi_ma_dv, serial_number, lockStatus, processChangedModel);//, lockStatus, processChangedModel);
                     }
                 }).ToList();
            await Task.WhenAll(tasks);
            return new SuccessResult<string>("");

        }

        public async Task<bool> XuLyThongDiepKySoHoaDonAsync(rs_yeu_cau_ky yeuCauKy)
        {
            try
            {
                var hoa_don_id = yeuCauKy.type_key.ConvertToInt();
                var base64 = yeuCauKy.ket_qua_ky;
                var user_id_phathanh = yeuCauKy.user_id.ConvertToInt();
                var result = await _serviceWrapper.HoaDon.HoaDon.UpdteKySoSuccessAsync(new Model.Request.ToKhai.HoaDonPhatHanhRequest()
                {
                    id = hoa_don_id,
                    signed_text = base64,
                }, user_id_phathanh);
                if (result.is_success)
                {
                    await _hoaDonPhatHanhHub.OnNewNotifyCreated(new Model.Request.Hub.HoaDonPhatHanhPushNotifyModel()
                    {
                        file_thong_diep_url = "",
                        hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.CHUA_GUI_CQT,
                        id = hoa_don_id,
                        ket_qua_phat_hanh = "Đã ký số",
                        user_id = user_id_phathanh.ToString()
                    });
                    return true;
                }
                return false;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> XuLyThongDiepKySoVaPhatHanhHoaDonAsync(rs_yeu_cau_ky yeuCauKy)
        {
            try
            {
                var hoa_don_id = yeuCauKy.type_key.ConvertToInt();
                var base64 = yeuCauKy.ket_qua_ky;
                var user_id_phathanh = yeuCauKy.user_id.ConvertToInt();
                var result = await _serviceWrapper.HoaDon.HoaDon.UpdteKySoSuccessAsync(new Model.Request.ToKhai.HoaDonPhatHanhRequest()
                {
                    id = hoa_don_id,
                    signed_text = base64,
                }, user_id_phathanh);
                if (result.is_success)
                {
                    await _hoaDonPhatHanhHub.OnNewNotifyCreated(new Model.Request.Hub.HoaDonPhatHanhPushNotifyModel()
                    {
                        file_thong_diep_url = "",
                        hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.CHUA_GUI_CQT,
                        id = hoa_don_id,
                        ket_qua_phat_hanh = "Đã ký số",
                        user_id = user_id_phathanh.ToString()
                    });
                    FunctionResult<HoaDonPhatHanhRespone> phatHanhResult = null;
                    var hoaDon = await _serviceWrapper.HoaDon.HoaDon.SelectByIdAsync(hoa_don_id);
                    if (hoaDon == null) return false;
                    if (hoaDon.hoa_don_hinh_thuc_code == "M")
                    {
                        phatHanhResult = await _hoaDonService.PhatHanhMTTAsync(new HoaDonPhatHanhRequest()
                        {
                            id = hoaDon.id,
                            signed_text = base64
                        }, hoaDon, user_id_phathanh);

                    }
                    else
                    {
                        phatHanhResult = await _hoaDonService.PhatHanhAsync(new HoaDonPhatHanhRequest()
                        {
                            id = hoaDon.id,
                            signed_text = base64
                        }, user_id_phathanh);

                    }
                    if (phatHanhResult.is_success)
                    {
                        await _hoaDonPhatHanhHub.OnNewNotifyCreated(new Model.Request.Hub.HoaDonPhatHanhPushNotifyModel()
                        {
                            file_thong_diep_url = "",
                            hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.DA_GUI_LEN_CQT_CHUA_PHAN_HOI_KIEM_TRA_DU_LIEU,
                            id = hoa_don_id,
                            ket_qua_phat_hanh = "Gửi yêu cầu",
                            user_id = user_id_phathanh.ToString()
                        });
                    }
                    return true;
                }
                return false;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<HoaDonCreateXmlKySoRespone>> CreateXmlVaPhatHanhsMTTBangKeAsync(HoaDonKyLoRequest request, List<hoa_don> hoaDonsMTT, bool isRunBackgroundForRS = false)
        {
            var result = new List<HoaDonCreateXmlKySoRespone>();
            // var userId = this.GetCurrentUserId();
            // var userSerialInfo = await _serviceWrapper.User.User.SelectByIdAsync(userId);

            var userId = this.GetCurrentUserId();
            user userSerialInfo = null;
            if (request.rs_ma_but_ky.ConvertToString() != "")
            {
                userSerialInfo = await _serviceWrapper.User.User.SelectByMaButKyAsync(request.rs_ma_but_ky);
                if (userSerialInfo != null) userId = userSerialInfo.id;
            }
            else
            {
                userSerialInfo = await _serviceWrapper.User.User.SelectByIdAsync(userId);

            }

            var processStatusModel = request.progress_id.ConvertToString() != "" ? new ProcessStatusRespone<ProcessStepDataBase>() : null;
            var processChangedModel = request.progress_id.ConvertToString() != "" ? new ProcessChangedModel() : null;
            if (processStatusModel != null)
            {
                processChangedModel = await CreateXmlStatusIniAsync(request.progress_id, processStatusModel, userId.ToString(), request.ids.Count);
            }
            var lockStatus = new SemaphoreSlim(1, 1);
            var base64BangKe = await this.CreateXmlsMTTBangKeAsync(hoaDonsMTT, lockStatus, processChangedModel);

            ProcessStatusRespone<ProcessStepDataBase> processStatus = null;
            if (processChangedModel != null)
            {
                processStatus = (ProcessStatusRespone<ProcessStepDataBase>)processChangedModel.processStatus;
                processStatus.steps[0].data.is_done = true;
                processStatus.steps[0].data.is_processing = false;
                await _processHub.OnProcessChangedAsync(processChangedModel);
            }
            if (isRunBackgroundForRS && userSerialInfo.rs_ma_but_ky.ConvertToString() != "")
            {

                await KySoRemoteSigningThenPhatHanhBackgroundListBangKeAsync(hoaDonsMTT, base64BangKe, userSerialInfo.serial_number, lockStatus, processChangedModel);
                return result;
            }
            else
            {

                if (base64BangKe.ConvertToString() != string.Empty)
                {
                    var signResultModel = await _serviceWrapper.ApiSignHoaDon.SignAsync(base64BangKe, hoaDonsMTT.FirstOrDefault()?.donvi_ma_dv ?? "", userSerialInfo.serial_number);
                    if (signResultModel != null && signResultModel.Macode == 1)
                    {
                        var hoaDonIds = hoaDonsMTT.Select(x => x.id).ToList();
                        var updateKySoResult = await _serviceWrapper.HoaDon.HoaDon.UpdteKySoSuccessBangKeAsync(hoaDonsMTT, signResultModel.SignedData, userId);
                        var hoaDonKySoThanhCongIds = updateKySoResult.Where(x => x.is_success).Select(x => x.id).ToList();
                        var hoaDonKySoThatBaiIds = updateKySoResult.Where(x => !x.is_success).Select(x => x.id).ToList();
                        var hoaDonsKySoThanhCong = hoaDonsMTT.Where(x => hoaDonKySoThanhCongIds.Contains(x.id)).ToList();
                        if (processChangedModel != null)
                        {
                            if (lockStatus != null) await lockStatus.WaitAsync();
                            processStatus = (ProcessStatusRespone<ProcessStepDataBase>)processChangedModel.processStatus;
                            processStatus.steps[1].data.is_processing = true;
                            processStatus.steps[1].data.success += hoaDonKySoThanhCongIds.Count;
                            processStatus.steps[1].data.error += hoaDonKySoThatBaiIds.Count;
                            await _processHub.OnProcessChangedAsync(processChangedModel);
                            if (lockStatus != null) lockStatus.Release();
                        }
                        var phatHanhResult = await _hoaDonService.PhatHanhMTTBangKeAsync(hoaDonsKySoThanhCong, signResultModel.SignedData, userId);
                        if (processChangedModel != null && phatHanhResult != null)
                        {
                            if (lockStatus != null) await lockStatus.WaitAsync();
                            processStatus = (ProcessStatusRespone<ProcessStepDataBase>)processChangedModel.processStatus;
                            processStatus.steps[2].data.is_processing = true;
                            processStatus.steps[2].data.success += phatHanhResult.is_success ? hoaDonKySoThanhCongIds.Count : 0;
                            processStatus.steps[2].data.error += phatHanhResult.is_success ? 0 : hoaDonKySoThanhCongIds.Count;
                            await _processHub.OnProcessChangedAsync(processChangedModel);
                            if (lockStatus != null) lockStatus.Release();
                        }
                    }
                    else
                    {
                        if (processChangedModel != null)
                        {
                            if (lockStatus != null) await lockStatus.WaitAsync();
                            processStatus = (ProcessStatusRespone<ProcessStepDataBase>)processChangedModel.processStatus;
                            processStatus.steps[1].data.is_processing = true;
                            processStatus.steps[1].data.success += 0;
                            processStatus.steps[1].data.error += hoaDonsMTT.Count;
                            await _processHub.OnProcessChangedAsync(processChangedModel);
                            if (lockStatus != null) lockStatus.Release();
                        }
                    }
                }
            }
            return result;
        }
        public async Task<FunctionResult<string>> KySoRemoteSigningThenPhatHanhBackgroundListBangKeAsync(List<hoa_don> hoaDons, string base64BangKe, string serial_number, SemaphoreSlim lockStatus, ProcessChangedModel processChangedModel)
        {
            var hoaDonIds = hoaDons.Select(x => x.id).ToList().Join(",");
            var kySoRequest = new KyBangKeRequest(serial_number, hoaDons.FirstOrDefault()?.donvi_ma_dv ?? "", base64BangKe, string.Empty);
            kySoRequest.so_luong = hoaDons.Count;
            var userId = this.GetCurrentUserId();
            var guiYeucauResult = await _serviceWrapper.RemoteSigningSerivce.KySoAsync(kySoRequest);
            if (guiYeucauResult.is_success)
            {
                var code = guiYeucauResult.data;
                if (code != "-2")
                {
                    await _serviceWrapper.HoaDon.RsYeuCauKy.SaveYeuCauKyAsync(code, userId.ToString(), Model.Enum.e_rs_yeu_cau_ky_type.KY_SO_VA_PHAT_HANH_BANG_KE, hoaDonIds);
                    return new SuccessResult<string>(code);
                }


            }

            return new ErrorResult<string>(guiYeucauResult.message);

        }
        private async Task<string> CreateXmlsMTTBangKeAsync(List<hoa_don> hoaDons, SemaphoreSlim lockStatus, ProcessChangedModel processChangedModel)
        {
            var base64Result = await _hoaDonService.CreateBase64MTTBangKeAsync(hoaDons);
            if (base64Result.is_success)
            {
                if (processChangedModel != null)
                {
                    await lockStatus.WaitAsync();
                    var processStatus = (ProcessStatusRespone<ProcessStepDataBase>)processChangedModel.processStatus;
                    processStatus.steps[0].data.success += base64Result.is_success ? hoaDons.Count : 0;
                    processStatus.steps[0].data.error += base64Result.is_success ? 0 : 1;
                    await _processHub.OnProcessChangedAsync(processChangedModel);
                    lockStatus.Release();
                }
                return base64Result.data;

            }
            return string.Empty;

        }

        public async Task<bool> XuLyThongDiepKySoVaPhatHanhHoaDonBangKeAsync(rs_yeu_cau_ky yeuCauKy)
        {
            try
            {
                var hoaDonIds = yeuCauKy.type_key.ConvertToList();
                var base64 = yeuCauKy.ket_qua_ky;
                var user_id_phathanh = yeuCauKy.user_id.ConvertToInt();
                var hoaDons = await _serviceWrapper.HoaDon.HoaDon.SelectByIdsAsync(hoaDonIds);
                var updateKySoResult = await _serviceWrapper.HoaDon.HoaDon.UpdteKySoSuccessBangKeAsync(hoaDons.ToList(), base64, user_id_phathanh);
                var hoaDonKySoThanhCongIds = updateKySoResult.Where(x => x.is_success).Select(x => x.id).ToList();
                var hoaDonKySoThatBaiIds = updateKySoResult.Where(x => !x.is_success).Select(x => x.id).ToList();
                var hoaDonsKySoThanhCong = hoaDons.Where(x => hoaDonKySoThanhCongIds.Contains(x.id)).ToList();

                var taskNotify = updateKySoResult.Select(result =>
                {
                    var hoa_don_id = result.id;
                    return _hoaDonPhatHanhHub.OnNewNotifyCreated(new Model.Request.Hub.HoaDonPhatHanhPushNotifyModel()
                    {
                        file_thong_diep_url = "",
                        hoa_don_trang_thai_id = result.is_success ? (int)e_hoa_don_trang_thai.CHUA_GUI_CQT : (int)e_hoa_don_trang_thai.LOI_THONG_DIEP,
                        id = hoa_don_id,
                        ket_qua_phat_hanh = result.is_success ? "Đã ký số" : result.message,
                        user_id = user_id_phathanh.ToString()
                    });
                }).ToList();
                await this.ExcuteDbTasks(taskNotify);
                var phatHanhResult = await _hoaDonService.PhatHanhMTTBangKeAsync(hoaDonsKySoThanhCong, base64, user_id_phathanh);
                if (phatHanhResult.is_success)
                {
                    var taskNotifyGuiPhatHanh = hoaDonsKySoThanhCong.Select(hoaDon =>
                    {
                        return _hoaDonPhatHanhHub.OnNewNotifyCreated(new Model.Request.Hub.HoaDonPhatHanhPushNotifyModel()
                        {
                            file_thong_diep_url = "",
                            hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.DA_GUI_LEN_CQT_CHUA_PHAN_HOI_KIEM_TRA_DU_LIEU,
                            id = hoaDon.id,
                            ket_qua_phat_hanh = "Gửi yêu cầu",
                            user_id = user_id_phathanh.ToString()
                        });
                    }).ToList();
                    await this.ExcuteDbTasks(taskNotifyGuiPhatHanh);
                }
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// Nhận kết quả từ CQL, xử lý
        /// </summary>
        /// <param name="xmlKetQua"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> XuLyThongDiepKetQuaPhanHanhAsync(KetQuaThongDiepRespone thongDiepRespone, string xmlKetQua)
        {
            var maThamChieu = thongDiepRespone.TTChung.MTDTChieu;
            var uuid = maThamChieu.Replace($"V{AppSettings.FixedValue.MNGui}", "");
            var hoaDonIds = await _serviceWrapper.Cache.GetDataAsync<List<int>>(uuid + "_bang_ke_mtt");
            var hoaDons = new List<hoa_don>();
            if (hoaDonIds != null && hoaDonIds.Count > 0)
            {
                hoaDons = (await _serviceWrapper.HoaDon.HoaDon.SelectByIdsAsync(hoaDonIds)).ToList();
            }
            else
            {
                hoaDons = await _repositoryWrapper.HoaDon.HoaDon.SelectListHoaDonByPhatHanhUuidAsync(uuid);
            }
            var hoaDonLoiIds = new Dictionary<int, int>();
            var isHopLe = false;
            if (thongDiepRespone.TTChung.MLTDiep == "204")
            {
                var LTBao = thongDiepRespone.DLieu?.TBao?.DLTBao?.LTBao ?? "";
                var maKetQuaPhatHanh = thongDiepRespone?.DLieu?.HDon?.MCCQT?.Text.ConvertToString() ?? "";
                if (LTBao == "2")
                {
                    //tất cả hóa đơn đều thành công
                    foreach (var hoaDon in hoaDons)
                    {
                        hoaDon.phat_hanh_ma_ketqua_cqt = maKetQuaPhatHanh;
                        hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.DA_PHAT_HANH;
                        hoaDon.ket_qua_phat_hanh = $"";
                    }
                    isHopLe = true;
                }
                else
                {

                    string pattern = @"<MTLoi>(.*?)</MTLoi>";
                    var matches = Regex.Matches(xmlKetQua, pattern);
                    foreach (Match match in matches)
                    {
                        var MTLoi = match.Groups[1].Value;
                        var MTLois = MTLoi.ConvertToString().Split(";");
                        if (MTLois.Length >= 4)
                        {
                            var KHMSHDon = MTLois[0];
                            var KHHDon = MTLois[1];
                            var SHDon = MTLois[2].ConvertToInt();
                            var hoaDon = hoaDons.Where(x => x.hoa_don_dang_ky_phat_hanh_mau_so == KHMSHDon &&
                            x.hoa_don_dang_ky_phat_hanh_ky_hieu == KHHDon &&
                            x.ma_so_hoa_don == SHDon
                            ).FirstOrDefault();
                            if (hoaDon != null)
                            {
                                hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.KHONG_HOP_LE;
                                hoaDon.ket_qua_phat_hanh = $"{MTLoi}";
                                hoaDonLoiIds.Add(hoaDon.id, hoaDon.id);
                            }
                        }
                    }
                    foreach (var hoaDon in hoaDons)
                    {
                        if (!hoaDonLoiIds.ContainsKey(hoaDon.id))
                        {

                            hoaDon.phat_hanh_ma_ketqua_cqt = maKetQuaPhatHanh;
                            hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.DA_PHAT_HANH;
                            hoaDon.ket_qua_phat_hanh = $"";
                        }
                    }

                }
            }
            if (thongDiepRespone.TTChung.MLTDiep == "-1")
            {
                foreach (var hoaDon in hoaDons)
                {
                    hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.LOI_THONG_DIEP;
                    hoaDon.ket_qua_phat_hanh = $"";
                }
            }
            if (thongDiepRespone.TTChung.MLTDiep == "999")
            {
                foreach (var hoaDon in hoaDons)
                {
                    hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.DA_GUI_LEN_CQT_CHUA_PHAN_HOI_KIEM_TRA_DU_LIEU;
                    hoaDon.ket_qua_phat_hanh = $"";
                }
            }
            var fileName = Guid.NewGuid().ToString() + ".xml";
            var filePath = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{fileName}";
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            await File.WriteAllTextAsync(filePath, xmlKetQua);
            var tasks = hoaDons.Select(async hoaDon =>
            {
                var isUpdated = await _hoaDonService.UpdateAsync(hoaDon);
                await _hoaDonLogService.SaveFromPhatHanhBangKeAsync(hoaDon.id, hoaDon.ket_qua_phat_hanh ?? "", filePath, hoaDon.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.DA_PHAT_HANH);
                await _serviceWrapper.HoaDon.PushMessageToVender.CheckAndPushMessageAsync(hoaDon);
                try
                {
                    var hoaDonPhatHanhHub = _serviceProvider.GetRequiredService<HoaDonPhatHanhHub>();
                    await hoaDonPhatHanhHub.OnNewNotifyCreated(new Model.Request.Hub.HoaDonPhatHanhPushNotifyModel()
                    {
                        file_thong_diep_url = filePath,
                        hoa_don_trang_thai_id = hoaDon.hoa_don_trang_thai_id,
                        id = hoaDon.id,
                        ket_qua_phat_hanh = hoaDon.ket_qua_phat_hanh ?? "",
                        user_id = hoaDon.user_id_phathanh.ToString()
                    });
                }
                catch (System.Exception ex)
                {
                    // TODO
                }

                if (hoaDon.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.DA_PHAT_HANH)
                {
                    //nếu là hóa đơn điều chỉnh/ thay thế thì cập nhật hóa đơn gốc
                    if (hoaDon.IsHoaDonDieuChinhThayThe())
                    {
                        var hoaDonGoc = await _repositoryWrapper.HoaDon.HoaDon.SelectHoaDonGocAsync(hoaDon.donvi_ma_dv, hoaDon.hoa_don_dang_ky_phat_hanh_mau_so_goc,
                        hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu_goc, hoaDon.ma_so_hoa_don_goc.ConvertToInt()
                        );
                        if (hoaDonGoc != null)
                        {
                            var hoa_don_ids_thaythe_dieuchinh = hoaDonGoc.hoa_don_ids_thaythe_dieuchinh.ConvertToString().Split(",")
                            .Where(x => x != string.Empty).ToList();
                            if (!hoa_don_ids_thaythe_dieuchinh.Contains(hoaDon.id.ToString()))
                            {
                                hoa_don_ids_thaythe_dieuchinh.Add(hoaDon.id.ToString());
                            }
                            hoaDonGoc.hoa_don_ids_thaythe_dieuchinh = hoa_don_ids_thaythe_dieuchinh.Join(",");
                            if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DIEU_CHINH)
                            {
                                hoaDonGoc.hoa_don_hinh_thuc_id = (int)e_hoa_don_hinh_thuc.HOA_DON_BI_DIEU_CHINH;
                            }
                            if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_THAY_THE)
                            {
                                hoaDonGoc.hoa_don_hinh_thuc_id = (int)e_hoa_don_hinh_thuc.HOA_DON_BI_THAY_THE;
                            }
                            hoaDonGoc.SetUpdateInfo(hoaDon.user_id_phathanh);
                            await this.UpdateAsync(hoaDonGoc);
                        }
                    }
                    await _serviceWrapper.HoaDon.HoaDonSendEmail.SendEmailHoaDonAsync(new List<int>() { hoaDon.id });
                }

                return Task.CompletedTask;
            }).ToList();
            await this.ExcuteDbTasks(tasks);
            var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(hoaDons.FirstOrDefault()?.donvi_ma_dv ?? "");
            var ngay_hoa_don_max = hoaDons.Select(x => x.ngay_hoa_don.Date).Max();
            if (donVi != null && ngay_hoa_don_max != null)
            {
                if (donVi.ngay_hoa_don_max == null || donVi.ngay_hoa_don_max.Value.Date <= ngay_hoa_don_max.Date)
                {
                    donVi.ngay_hoa_don_max = ngay_hoa_don_max.Date;
                    await _serviceWrapper.Category.DonVi.UpdateAsync(donVi);
                }
            }
            return true;


        }
    }
}
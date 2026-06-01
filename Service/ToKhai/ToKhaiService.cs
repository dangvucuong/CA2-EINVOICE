using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Xsl;
using Common;
using Contracts.Service.ToKhai;
using Microsoft.Extensions.DependencyInjection;
using Model.Base;
using Model.Enum;
using Model.RemoteSigning;
using Model.Request.ToKhai;
using Model.Respone.Xml;
using Model.Static;
using Model.Table;
using Service.Base;
using Service.Hub;
using WebApp;

namespace Service.ToKhai
{
    public class ToKhaiService : CRUDService<to_khai>, IToKhaiService
    {
        private HoaDonPhatHanhHub _hoaDonPhatHanhHub;
        public ToKhaiService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.ToKhaiWrapper.ToKhai;
            this._hoaDonPhatHanhHub = _serviceProvider.GetRequiredService<HoaDonPhatHanhHub>();
        }

        public async Task<int> SaveToKhaiAsync(ToKhaiAddOrEditModel model)
        {
            var user = this.GetCurrentUser();
            if (model.id <= 0)
            {

                if (model.to_khai_status_id == (int)e_to_khai_status.CQT_DONG_Y)
                {
                    //giữ nguyên
                }
                else
                {
                    model.to_khai_status_id = (int)e_to_khai_status.TAO_MOI;
                }
                model.id = await this.InsertAsync(model.Map<to_khai>());
                var log = new to_khai_log()
                {
                    file_thong_diep_url = string.Empty,
                    ngay_thuc_hien = DateTime.Now,
                    nguoi_thuc_hien = user.full_name,
                    noi_dung_thuc_hien = "Tạo mới tờ khai",
                    to_khai_id = model.id,
                    to_khai_log_type_id = (int)e_to_khai_log_type.TAO_MOI
                };
                log.SetInsertInfo(user.id);
                await _serviceWrapper.ToKhaiSerivceWrapper.ToKhaiLog.InsertAsync(log);
            }
            else
            {
                var obj = await this.SelectByIdAsync(model.id);
                if (obj != null)
                {
                    obj.SetUpdateInfo(user.id);
                    obj.loai_to_khai_id = model.loai_to_khai_id;

                    obj.ma_to_khai = model.ma_to_khai;
                    obj.ngay_lap = model.ngay_lap;
                    obj.nguoi_nop_thue = model.nguoi_nop_thue;
                    obj.mst = model.mst;
                    obj.nguoi_lien_he = model.nguoi_lien_he;

                    obj.co_quan_thue = model.co_quan_thue;
                    obj.co_quan_thue_id = model.co_quan_thue_id;
                    obj.ma_cqt = model.ma_cqt;

                    obj.to_chuc_truyen_nhan_json = model.to_chuc_truyen_nhan_json;
                    obj.to_chuc_cap_giay_phep_json = model.to_chuc_cap_giay_phep_json;



                    obj.dia_chi_lien_he = model.dia_chi_lien_he;
                    obj.email_lien_he = model.email_lien_he;
                    obj.dien_thoai_lien_he = model.dien_thoai_lien_he;

                    obj.is_hoadon_co_ma_cqt = model.is_hoadon_co_ma_cqt;
                    obj.is_hoadon_co_ma_cqt_mtt = model.is_hoadon_co_ma_cqt_mtt;
                    obj.is_hoadon_khong_co_ma_cqt = model.is_hoadon_khong_co_ma_cqt;

                    obj.is_khong_phai_tra_tien_dich_vu = model.is_khong_phai_tra_tien_dich_vu;
                    obj.is_doanh_nghiep_vvn_kho_khan = model.is_doanh_nghiep_vvn_kho_khan;
                    obj.is_doanh_nghiep_vvn_khac = model.is_doanh_nghiep_vvn_khac;

                    obj.is_chuyen_du_lieu_truc_tiep = model.is_chuyen_du_lieu_truc_tiep;
                    obj.is_chuyen_lieu_thong_qua_to_chuc = model.is_chuyen_lieu_thong_qua_to_chuc;

                    obj.is_chuyen_day_du_tung_hoadon = model.is_chuyen_day_du_tung_hoadon;
                    obj.is_chuyen_theo_bang_tonghop = model.is_chuyen_theo_bang_tonghop;

                    obj.is_sd_hoadon_gtgt = model.is_sd_hoadon_gtgt;
                    obj.is_sd_hoadon_banhang = model.is_sd_hoadon_banhang;
                    obj.is_sd_hoadon_khac = model.is_sd_hoadon_khac;
                    obj.is_sd_chungtu_giong_hoadon = model.is_sd_chungtu_giong_hoadon;
                    obj.is_ban_hang_du_tru_quoc_gia = model.is_ban_hang_du_tru_quoc_gia;
                    obj.is_ban_tai_san_cong = model.is_ban_tai_san_cong;

                    obj.noi_lap = model.noi_lap;
                    obj.ngay_co_hieu_luc = model.ngay_co_hieu_luc;

                    obj.dai_dien_phap_luat_ho_ten = model.dai_dien_phap_luat_ho_ten;
                    obj.dai_dien_phap_luat_dien_thoai = model.dai_dien_phap_luat_dien_thoai;
                    obj.dai_dien_phap_luat_dien_cccd = model.dai_dien_phap_luat_dien_cccd;
                    obj.dai_dien_phap_luat_dien_ngay_sinh = model.dai_dien_phap_luat_dien_ngay_sinh;
                    obj.dai_dien_phap_luat_dien_gioi_tinh = model.dai_dien_phap_luat_dien_gioi_tinh;
                    obj.is_co_quan_xu_ly_tai_san_cong = model.is_co_quan_xu_ly_tai_san_cong;
                    obj.is_sd_hoadon_gtgt_bien_lai = model.is_sd_hoadon_gtgt_bien_lai;
                    obj.is_sd_hoadon_banhang_bien_lai = model.is_sd_hoadon_banhang_bien_lai;
                    obj.is_sd_hoadon_thuong_mai = model.is_sd_hoadon_thuong_mai;
                    obj.so_ho_chieu = model.so_ho_chieu;

                    //tam ngung su dung
                    obj.tam_ngung_su_dung = model.tam_ngung_su_dung;

                    await this.UpdateAsync(obj);
                    var log = new to_khai_log()
                    {
                        file_thong_diep_url = string.Empty,
                        ngay_thuc_hien = DateTime.Now,
                        nguoi_thuc_hien = user.full_name,
                        noi_dung_thuc_hien = "Cập nhật tờ khai",
                        to_khai_id = model.id,
                        to_khai_log_type_id = (int)e_to_khai_log_type.CAP_NHAT
                    };
                    log.SetInsertInfo(user.id);
                    await _serviceWrapper.ToKhaiSerivceWrapper.ToKhaiLog.InsertAsync(log);
                }


            }
            var toKhaiCts = await _repositoryWrapper.ToKhaiWrapper.ToKhaiCTS.SelectByToKhaiAsync(model.id);
            var toKhaiCtsUrl = toKhaiCts.Select(x => x.url).ToList();
            var list_cts_url = model.list_cts.Select(x => x.url).ToList();

            var ctsInsert = model.list_cts.Where(x => !toKhaiCtsUrl.Contains(x.url));
            var ctsDelete = toKhaiCts.Where(x => !list_cts_url.Contains(x.url));

            foreach (var item in ctsInsert)
            {
                item.to_khai_id = model.id;
                item.SetInsertInfo(model.last_modified_user_id);
                item.id = await _repositoryWrapper.ToKhaiWrapper.ToKhaiCTS.InsertAsync(item);
            }
            foreach (var item in ctsDelete)
            {
                await _repositoryWrapper.ToKhaiWrapper.ToKhaiCTS.DeleteAsync(item.id, model.last_modified_user_id);
            }

            return model.id;
        }

        public async Task<ToKhaiAddOrEditModel> SelectViewModel(int id)
        {
            var toKhai = await this.SelectByIdAsync(id);
            if (toKhai != null)
            {
                var toKhaiCTS = await _repositoryWrapper.ToKhaiWrapper.ToKhaiCTS.SelectByToKhaiAsync(id);
                var model = toKhai.Map<ToKhaiAddOrEditModel>();
                model.list_cts = toKhaiCTS.ToList();
                return model;

            }
            return null;
        }

        public async Task<IEnumerable<to_khai>> SelectByDonViAsync(string donvi_ma_dv)
        {

            return await _repositoryWrapper.ToKhaiWrapper.ToKhai.SelectByDonViAsync(donvi_ma_dv);
        }

        public async Task<string> PhatHanhAsync(int id, string signedText, int user_id_phathanh = 0)
        {
            var toKhai = await this.SelectByIdAsync(id);
            if (toKhai.phat_hanh_uuid.ConvertToString() != "")
            {
                return "Tờ khai đã phát hành trước đó";
            }
            var MNGui = AppSettings.FixedValue.MNGui;
            var MNNhan = AppSettings.FixedValue.MNNhan;
            var MLTDiep = "100";
            var guidstr = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            var MTDiep = AppSettings.FixedValue.MNGui + guidstr;
            var MTDTChieu = "";
            var SLuong = "1";
            var MST = toKhai.donvi_ma_dv;
            var strChuoiHoaDon = signedText;

            toKhai.user_id_phathanh = user_id_phathanh > 0 ? user_id_phathanh : this.GetCurrentUserId();
            toKhai.phat_hanh_uuid = guidstr;
            await _serviceWrapper.Cache.SetDataAsync<string>(guidstr, "to_khai", DateTime.Now.AddDays(30));
            await _repositoryWrapper.HoaDon.PhatHanhUUID.SaveLogUuidAsync(guidstr, "to_khai", toKhai.user_id_phathanh);
            await this.UpdateAsync(toKhai);
            await _serviceWrapper.Cache.SetDataAsync<to_khai>(guidstr + "_to_khai", toKhai, DateTime.Now.AddDays(30));

            string kq = "";
            // Tao thong tin XML chung
            string linkelement = "";

            var doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            doc.AppendChild(docNode);

            // The TDiep
            XmlElement TDiepNode = doc.CreateElement("", "TDiep", linkelement);
            doc.AppendChild(TDiepNode);
            // TT Chung
            XmlElement TTChungTDNode = doc.CreateElement("", "TTChung", linkelement);
            TDiepNode.AppendChild(TTChungTDNode);
            // PBan
            XmlNode PBanTTNode = doc.CreateElement("", "PBan", linkelement);
            PBanTTNode.AppendChild(doc.CreateTextNode("2.1.0"));
            TTChungTDNode.AppendChild(PBanTTNode);
            // MNGui
            XmlNode MNGuiNode = doc.CreateElement("", "MNGui", linkelement);
            MNGuiNode.AppendChild(doc.CreateTextNode(MNGui));
            TTChungTDNode.AppendChild(MNGuiNode);
            // MNNhan
            XmlNode MNNhanNode = doc.CreateElement("", "MNNhan", linkelement);
            MNNhanNode.AppendChild(doc.CreateTextNode(MNNhan));
            TTChungTDNode.AppendChild(MNNhanNode);
            // MLTDiep
            XmlNode MLTDiepNode = doc.CreateElement("", "MLTDiep", linkelement);
            MLTDiepNode.AppendChild(doc.CreateTextNode(MLTDiep));
            TTChungTDNode.AppendChild(MLTDiepNode);
            // MTDiep
            XmlNode MTDiepNode = doc.CreateElement("", "MTDiep", linkelement);
            MTDiepNode.AppendChild(doc.CreateTextNode(MTDiep));
            TTChungTDNode.AppendChild(MTDiepNode);
            // MTDTChieu
            XmlNode MTDTChieuNode = doc.CreateElement("", "MTDTChieu", linkelement);
            MTDTChieuNode.AppendChild(doc.CreateTextNode(MTDTChieu));
            TTChungTDNode.AppendChild(MTDTChieuNode);
            // MST
            XmlNode MSTTTNode = doc.CreateElement("", "MST", linkelement);
            MSTTTNode.AppendChild(doc.CreateTextNode(MST));
            TTChungTDNode.AppendChild(MSTTTNode);
            // SLuong
            XmlNode SLuongNode = doc.CreateElement("", "SLuong", linkelement);
            SLuongNode.AppendChild(doc.CreateTextNode(SLuong));
            TTChungTDNode.AppendChild(SLuongNode);
            // DLieu
            XmlElement DLieuNode = doc.CreateElement("", "DLieu", linkelement);
            TDiepNode.AppendChild(DLieuNode);
            XmlNodeList lstNode = doc.GetElementsByTagName("DLieu");
            XmlNode convert = XmlStringToXmlNode(strChuoiHoaDon);

            for (int i = 0; i < lstNode.Count; i++)
            {
                XmlNode xnode = lstNode[lstNode.Count - 1];
                xnode.AppendChild(xnode.OwnerDocument.ImportNode(convert, true));
            }

            kq = doc.InnerXml;
            var base64thongdiep = kq.ConvertToBase64();
            var user = this.GetCurrentUser();
            var fileName = Guid.NewGuid().ToString() + ".xml";
            // var filePath = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{fileName}";
            var filePath = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{fileName}";
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            await File.WriteAllTextAsync(filePath, kq);

            using (var client = Helper.WSInterTRCA2Helper.GetClient())
            {
                await client.OpenAsync();
                var authHeader = Helper.WSInterTRCA2Helper.GetAuthHeader();
                try
                {
                    var guiThongDiepResult = await client.Guithongdiep2024Async(authHeader, base64thongdiep, 1);
                    if (guiThongDiepResult.Guithongdiep2024Result.ConvertToString().Length > 2)
                    {
                        var log = new to_khai_log()
                        {
                            file_thong_diep_url = filePath,
                            ngay_thuc_hien = DateTime.Now,
                            nguoi_thuc_hien = user?.full_name ?? "",
                            to_khai_id = toKhai.id,
                            noi_dung_thuc_hien = "Gửi phát hành",
                            to_khai_log_type_id = -2
                        };
                        log.SetInsertInfo(0);
                        await _serviceWrapper.ToKhaiSerivceWrapper.ToKhaiLog.InsertAsync(log);
                        toKhai.to_khai_status_id = (int)e_thong_bao_sai_sot_trang_thai.CHO_CQT;
                        await this.UpdateAsync(toKhai);
                    }
                    else
                    {
                        var log = new to_khai_log()
                        {
                            file_thong_diep_url = filePath,
                            ngay_thuc_hien = DateTime.Now,
                            nguoi_thuc_hien = user?.full_name ?? "",
                            to_khai_id = toKhai.id,
                            noi_dung_thuc_hien = $"Gửi phát hành thất bại {guiThongDiepResult.Guithongdiep2024Result.ConvertToString()}",
                            to_khai_log_type_id = -2
                        };
                        log.SetInsertInfo(0);
                        await _serviceWrapper.ToKhaiSerivceWrapper.ToKhaiLog.InsertAsync(log);
                    }
                }
                catch (System.Exception ex)
                {
                    var log = new to_khai_log()
                    {
                        file_thong_diep_url = filePath,
                        ngay_thuc_hien = DateTime.Now,
                        nguoi_thuc_hien = user?.full_name ?? "",
                        to_khai_id = toKhai.id,
                        noi_dung_thuc_hien = $"Gửi phát hành thất bại {ex.Message.ConvertToString()}",
                        to_khai_log_type_id = -2
                    };
                    log.SetInsertInfo(0);
                    await _serviceWrapper.ToKhaiSerivceWrapper.ToKhaiLog.InsertAsync(log);
                }
                finally
                {
                    await client.CloseAsync();
                }



                // return new SuccessResult<bool>();
            }

            return kq;
        }
        private XmlNode XmlStringToXmlNode(string xmlInputString)
        {
            if (string.IsNullOrEmpty(xmlInputString.Trim()))
            {
                throw new ArgumentNullException(nameof(xmlInputString));
            }

            string decodedString = Encoding.UTF8.GetString(Convert.FromBase64String(xmlInputString));
            XmlDocument xmlDoc = new XmlDocument();

            using (StringReader sr = new StringReader(decodedString))
            {
                xmlDoc.Load(sr);
            }

            return xmlDoc.DocumentElement;
        }

        public async Task<string> CreateXmlKySoAsync(int id)
        {
            var toKhai = await this.SelectByIdAsync(id);
            var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(toKhai?.donvi_ma_dv ?? "");
            var toKhaiCers = await _repositoryWrapper.ToKhaiWrapper.ToKhaiCTS.SelectByToKhaiAsync(id);
            var MSo = "01/ĐKTĐ-HĐĐT";
            var HThuc = toKhai.loai_to_khai_id.ToString();
            var Ten = toKhai.loai_to_khai_id == 1
            ? "Tờ khai đăng ký sử dụng hóa đơn điện tử"
            : "Tờ khai thay đổi thông tin sử dụng hóa đơn điện tử";
            var TNNT = toKhai.nguoi_nop_thue;
            var MST = toKhai.mst;
            var CQTQLy = toKhai.co_quan_thue;
            var MCQTQLy = toKhai.ma_cqt;
            var NLHe = toKhai.nguoi_lien_he;
            var DCLHe = toKhai.dia_chi_lien_he;
            var DCTDTu = toKhai.email_lien_he;
            var DTLHe = toKhai.dien_thoai_lien_he;
            var DDanh = toKhai.noi_lap;
            var NLap = toKhai.ngay_lap.ToString("yyyy-MM-dd");
            var CMa = toKhai.is_hoadon_co_ma_cqt ? "1" : "0";
            var KCMa = toKhai.is_hoadon_khong_co_ma_cqt ? "1" : "0";
            var CMMTT = toKhai.is_hoadon_co_ma_cqt_mtt ? "1" : "0";

            var CDLTTDCQT = toKhai.is_chuyen_du_lieu_truc_tiep ? "1" : "0";
            var CDLQTCTN = toKhai.is_chuyen_lieu_thong_qua_to_chuc ? "1" : "0";

            var NNTDBKKhan = toKhai.is_doanh_nghiep_vvn_kho_khan ? "1" : "0";
            var NNTKTDNUBND = toKhai.is_doanh_nghiep_vvn_khac ? "1" : "0";

            var CDDu = toKhai.is_chuyen_day_du_tung_hoadon ? "1" : "0";
            var CBTHop = toKhai.is_chuyen_theo_bang_tonghop ? "1" : "0";

            var HDGTGT = toKhai.is_sd_hoadon_gtgt ? "1" : "0";
            var HDBHang = toKhai.is_sd_hoadon_banhang ? "1" : "0";
            //hoa don ban tai san cong
            var HDBTSCong = toKhai.is_ban_tai_san_cong ? "1" : "0";
            //hoa don ban hang du tru quoc gia
            var HDBHDTQGia = toKhai.is_ban_hang_du_tru_quoc_gia ? "1" : "0";
            var HDKhac = toKhai.is_sd_hoadon_khac ? "1" : "0";

            var CTu = toKhai.is_sd_chungtu_giong_hoadon ? "1" : "0";


            string kq = "";
            // Create XML document
            string linkelement = "";
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", "");
            doc.AppendChild(docNode);

            string sId = Guid.NewGuid().ToString();

            // Create root element 'TKhai'
            XmlElement TDiepNode = doc.CreateElement("", "TKhai", linkelement);
            doc.AppendChild(TDiepNode);

            // Create 'DLTKhai' element with 'Id' attribute
            XmlElement DLTKhaiNode = doc.CreateElement("", "DLTKhai", linkelement);
            XmlAttribute productAttribute = doc.CreateAttribute("Id");
            productAttribute.Value = "_" + sId;
            DLTKhaiNode.Attributes.Append(productAttribute);
            TDiepNode.AppendChild(DLTKhaiNode);

            // Create 'TTChung' element
            XmlNode TTChungNode = doc.CreateElement("", "TTChung", linkelement);
            DLTKhaiNode.AppendChild(TTChungNode);
            AddElementWithText(doc, TTChungNode, "PBan", "2.1.0");
            AddElementWithText(doc, TTChungNode, "MSo", MSo);
            AddElementWithText(doc, TTChungNode, "Ten", Ten);
            AddElementWithText(doc, TTChungNode, "HThuc", HThuc);
            AddElementWithText(doc, TTChungNode, "TNNT", TNNT);
            AddElementWithText(doc, TTChungNode, "MST", MST);
            AddElementWithText(doc, TTChungNode, "CQTQLy", CQTQLy);
            AddElementWithText(doc, TTChungNode, "MCQTQLy", MCQTQLy);
            AddElementWithText(doc, TTChungNode, "NLHe", NLHe);
            AddElementWithText(doc, TTChungNode, "DCLHe", DCLHe);
            AddElementWithText(doc, TTChungNode, "DCTDTu", DCTDTu);
            AddElementWithText(doc, TTChungNode, "DTLHe", DTLHe);
            AddElementWithText(doc, TTChungNode, "DDanh", DDanh);
            AddElementWithText(doc, TTChungNode, "NLap", NLap);

            AddElementWithText(doc, TTChungNode, "TNDDPLuat", toKhai.dai_dien_phap_luat_ho_ten.ConvertToString());
            AddElementWithText(doc, TTChungNode, "DTDDPLuat", toKhai.dai_dien_phap_luat_dien_thoai.ConvertToString());
            AddElementWithText(doc, TTChungNode, "CCCDan", toKhai.dai_dien_phap_luat_dien_cccd.ConvertToString());
            AddElementWithText(doc, TTChungNode, "SHChieu", toKhai.so_ho_chieu.ConvertToString());
            if (toKhai.dai_dien_phap_luat_dien_ngay_sinh.HasValue)
                AddElementWithText(doc, TTChungNode, "NSDDPLuat", toKhai.dai_dien_phap_luat_dien_ngay_sinh.Value.ToString("yyyy-MM-dd"));
            AddElementWithText(doc, TTChungNode, "GTinh", toKhai.dai_dien_phap_luat_dien_gioi_tinh.ConvertToString());

            // Create 'NDTKhai' element
            XmlNode NDTKhaiNode = doc.CreateElement("", "NDTKhai", linkelement);
            DLTKhaiNode.AppendChild(NDTKhaiNode);

            // Create 'HTHDon' element
            XmlNode HTHDonNode = doc.CreateElement("", "HTHDon", linkelement);
            NDTKhaiNode.AppendChild(HTHDonNode);
            AddElementWithText(doc, HTHDonNode, "CMa", CMa);
            AddElementWithText(doc, HTHDonNode, "KCMa", KCMa);
            AddElementWithText(doc, HTHDonNode, "CMTMTTien", CMMTT);

            // Create 'HTGDLHĐĐT' element
            XmlNode HTGDLHĐĐTNode = doc.CreateElement("", "HTGDLHDDT", linkelement);
            NDTKhaiNode.AppendChild(HTGDLHĐĐTNode);
            AddElementWithText(doc, HTGDLHĐĐTNode, "NNTDBKKhan", NNTDBKKhan);
            AddElementWithText(doc, HTGDLHĐĐTNode, "NNTKTDNUBND", NNTKTDNUBND);
            AddElementWithText(doc, HTGDLHĐĐTNode, "CDLTTDCQT", CDLTTDCQT);
            AddElementWithText(doc, HTGDLHĐĐTNode, "CDLQTCTN", CDLQTCTN);
            AddElementWithText(doc, HTGDLHĐĐTNode, "CQXLTSCong", toKhai.is_co_quan_xu_ly_tai_san_cong ? "1" : "0");

            // Create 'PThuc' element
            XmlNode PThucNode = doc.CreateElement("", "PThuc", linkelement);
            NDTKhaiNode.AppendChild(PThucNode);
            AddElementWithText(doc, PThucNode, "CDDu", CDDu);
            AddElementWithText(doc, PThucNode, "CBTHop", CBTHop);

            // Create 'LHDSDung' element
            XmlNode LHDSDungNode = doc.CreateElement("", "LHDSDung", linkelement);
            NDTKhaiNode.AppendChild(LHDSDungNode);
            AddElementWithText(doc, LHDSDungNode, "HDGTGT", HDGTGT);
            AddElementWithText(doc, LHDSDungNode, "HDBHang", HDBHang);
            AddElementWithText(doc, LHDSDungNode, "HDBTSCong", HDBTSCong);
            AddElementWithText(doc, LHDSDungNode, "HDBHDTQGia", HDBHDTQGia);
            AddElementWithText(doc, LHDSDungNode, "HDKhac", HDKhac);
            AddElementWithText(doc, LHDSDungNode, "CTu", CTu);
            AddElementWithText(doc, LHDSDungNode, "HDGTGTTHBLai", toKhai.is_sd_hoadon_gtgt_bien_lai ? "1" : "0");
            AddElementWithText(doc, LHDSDungNode, "HDBHTHBLai", toKhai.is_sd_hoadon_banhang_bien_lai ? "1" : "0");
            AddElementWithText(doc, LHDSDungNode, "HDTMai", toKhai.is_sd_hoadon_thuong_mai ? "1" : "0");

            // Create 'TTTCGP' element
            XmlNode TTTCGPNode = doc.CreateElement("", "TTTCGP", linkelement);
            NDTKhaiNode.AppendChild(TTTCGPNode);


            var TTTCGPs = toKhai.to_chuc_cap_giay_phep_json.ConvertToString() != ""
            ? toKhai.to_chuc_cap_giay_phep_json.ConvertToString() :
            "";
            if (TTTCGPs != "")
            {
                try
                {
                    var listTTTCGPs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TTTCGP>>(TTTCGPs);
                    foreach (var item in listTTTCGPs)
                    {
                        XmlNode TCGPNode = doc.CreateElement("", "TCGP", linkelement);
                        TTTCGPNode.AppendChild(TCGPNode);
                        AddElementWithText(doc, TCGPNode, "STT", (item.id.ConvertToInt() + 1).ToString());
                        AddElementWithText(doc, TCGPNode, "TTCGP", item.TTCGP);
                        AddElementWithText(doc, TCGPNode, "MSTTCGP", item.MSTTCGP);
                        AddElementWithText(doc, TCGPNode, "TNgay", item.TNgay);
                        AddElementWithText(doc, TCGPNode, "DNgay", item.DNgay);
                    }
                }
                catch (System.Exception ex)
                {
                    XmlNode TCGPNode = doc.CreateElement("", "TCGP", linkelement);
                    TTTCGPNode.AppendChild(TCGPNode);
                    AddElementWithText(doc, TCGPNode, "STT", "1");
                    AddElementWithText(doc, TCGPNode, "TTCGP", "CÔNG TY CỔ PHẦN CÔNG NGHỆ THẺ NACENCOMM");
                    AddElementWithText(doc, TCGPNode, "MSTTCGP", "0103930279");
                    AddElementWithText(doc, TCGPNode, "TNgay", "2021-12-01");
                    AddElementWithText(doc, TCGPNode, "DNgay", "2030-12-31");
                }
            }



            // Create 'TTTCGP' element
            XmlNode TTTCTNNode = doc.CreateElement("", "TTTCTN", linkelement);
            NDTKhaiNode.AppendChild(TTTCTNNode);


            var TCTNs = toKhai.to_chuc_truyen_nhan_json.ConvertToString() != ""
         ? toKhai.to_chuc_truyen_nhan_json.ConvertToString() :
         "";
            if (TCTNs != "")
            {
                try
                {
                    var listTCTN = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TCTN>>(TCTNs);
                    foreach (var item in listTCTN)
                    {
                        XmlNode TCTNNode = doc.CreateElement("", "TCTN", linkelement);
                        TTTCTNNode.AppendChild(TCTNNode);
                        AddElementWithText(doc, TCTNNode, "STT", (item.id.ConvertToInt() + 1).ToString());
                        AddElementWithText(doc, TCTNNode, "TTCTN", item.TTCTN);
                        AddElementWithText(doc, TCTNNode, "MSTTCTN", item.MSTTCTN);
                        AddElementWithText(doc, TCTNNode, "TNgay", item.TNgay);
                        AddElementWithText(doc, TCTNNode, "DNgay", item.DNgay);
                    }
                }
                catch (System.Exception ex)
                {
                    XmlNode TCTNNode = doc.CreateElement("", "TCTN", linkelement);
                    TTTCTNNode.AppendChild(TCTNNode);
                    AddElementWithText(doc, TCTNNode, "STT", "1");
                    AddElementWithText(doc, TCTNNode, "TTCTN", "CÔNG TY CỔ PHẦN CÔNG NGHỆ THẺ NACENCOMM");
                    AddElementWithText(doc, TCTNNode, "MSTTCTN", "0103930279");
                    AddElementWithText(doc, TCTNNode, "TNgay", "2021-12-01");
                    AddElementWithText(doc, TCTNNode, "DNgay", "2030-12-31");
                }
            }

            //thông tin tạm ngưng sử dụng hóa đơn điện tử
            var TTTNgungsdung = toKhai.tam_ngung_su_dung.ConvertToString() != ""
           ? toKhai.tam_ngung_su_dung.ConvertToString() :
           "";
            if (TTTNgungsdung != "")
            {
                XmlNode TTTNSDungNode = doc.CreateElement("", "TTTNSDung", linkelement);
                NDTKhaiNode.AppendChild(TTTNSDungNode);
                try
                {
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<TTTNSDungRoot>(TTTNgungsdung);
                    var listTamngung = data?.TTTNSDung?.TNSDung;
                    if (listTamngung != null)
                    {
                        foreach (var item in listTamngung)
                        {
                            XmlNode TNSDungNode = doc.CreateElement("", "TNSDung", linkelement);
                            TTTNSDungNode.AppendChild(TNSDungNode);
                            AddElementWithText(doc, TNSDungNode, "STT", item.STT);
                            AddElementWithText(doc, TNSDungNode, "TTCGP", item.TTCGP);
                            AddElementWithText(doc, TNSDungNode, "MSTTCGP", item.MSTTCGP);
                            AddElementWithText(doc, TNSDungNode, "TNgay", item.TNgay);
                            AddElementWithText(doc, TNSDungNode, "DNgay", item.DNgay);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    XmlNode TNSDungNode = doc.CreateElement("", "TNSDung", linkelement);
                    TTTNSDungNode.AppendChild(TNSDungNode);
                    AddElementWithText(doc, TNSDungNode, "STT", "1");
                    AddElementWithText(doc, TNSDungNode, "TTCGP", "CÔNG TY CỔ PHẦN CÔNG NGHỆ THẺ NACENCOMM");
                    AddElementWithText(doc, TNSDungNode, "MSTTCGP", "0103930279");
                    AddElementWithText(doc, TNSDungNode, "TNgay", "2021-12-01");
                    AddElementWithText(doc, TNSDungNode, "DNgay", "2030-12-31");
                }
            }



            // Create 'DSCTSSDung' element
            XmlNode DSCTSSDungNode = doc.CreateElement("", "DSCTSSDung", linkelement);
            NDTKhaiNode.AppendChild(DSCTSSDungNode);

            // Add certificate information
            var STT = 0;
            foreach (var cert in toKhaiCers)
            {
                STT += 1;
                XmlNode CTSNode = doc.CreateElement("", "CTS", linkelement);
                DSCTSSDungNode.AppendChild(CTSNode);
                AddElementWithText(doc, CTSNode, "STT", STT.ToString());
                // string delimiter = ", E";
                var issuer = cert.issuer;
                // int index = cert.subject.ConvertToString().IndexOf(delimiter);
                // if (index != -1)
                // {
                //     subject = subject.Substring(0, index).Replace("CN=", "");
                // }

                var match = Regex.Match(issuer, @"CN=([^,]+)");
                issuer = match.Success ? match.Groups[1].Value : issuer;
                AddElementWithText(doc, CTSNode, "TTChuc", issuer);
                AddElementWithText(doc, CTSNode, "Seri", cert.serial_number.ToUpper());
                AddElementWithText(doc, CTSNode, "TNgay", cert.not_before.ToString("yyyy-MM-ddTHH:mm:ss"));
                AddElementWithText(doc, CTSNode, "DNgay", cert.not_after.ToString("yyyy-MM-ddTHH:mm:ss"));
                if (TTTNgungsdung != "")
                {
                    AddElementWithText(doc, CTSNode, "HThuc", "3");
                }
                else
                {
                    AddElementWithText(doc, CTSNode, "HThuc", "1");
                }
                    
            }

            // Create 'DSCKS' element
            XmlElement DSCKSNode = doc.CreateElement("", "DSCKS", linkelement);
            TDiepNode.AppendChild(DSCKSNode);
            XmlElement NNTNode = doc.CreateElement("", "NNT", linkelement);
            // var SigningTime = doc.CreateElement("", "SigningTime", toKhai.ngay_tao.ToString());
            // NNTNode.AppendChild(SigningTime)
            //add thêm nội dung ký từ file xml ký số
            var logKyGui = (await _serviceWrapper.ToKhaiSerivceWrapper.ToKhaiLog.SelectByToKhaiAsync(id)).Where(x => x.to_khai_log_type_id == 0).LastOrDefault();
            AddElementWithText(doc, NNTNode, "SigningTime", logKyGui?.created_time.ToString("yyyy-MM-ddTHH:mm:ss") ?? toKhai.ngay_tao.ToString("yyyy-MM-ddTHH:mm:ss"));
            AddElementWithText(doc, NNTNode, "X509SubjectName", "CN=" + (donVi?.ten_dv ?? "") + ",");
            DSCKSNode.AppendChild(NNTNode);


            //
            XmlElement CCKSKhacNode = doc.CreateElement("", "CCKSKhac", linkelement);
            DSCKSNode.AppendChild(CCKSKhacNode);

            kq = doc.InnerXml;
            var user = this.GetCurrentUser();
            var fileName = Guid.NewGuid().ToString() + ".xml";
            var filePath = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{fileName}";
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            await File.WriteAllTextAsync(filePath, kq);
            var log = new to_khai_log()
            {
                file_thong_diep_url = filePath,
                ngay_thuc_hien = DateTime.Now,
                nguoi_thuc_hien = user?.full_name ?? "",
                to_khai_id = toKhai.id,
                noi_dung_thuc_hien = "Tạo XML Ký số",
                to_khai_log_type_id = -1
            };
            log.SetInsertInfo(0);
            await _serviceWrapper.ToKhaiSerivceWrapper.ToKhaiLog.InsertAsync(log);
            return kq;
        }

        private void AddElementWithText(XmlDocument doc, XmlNode parentNode, string elementName, string textContent)
        {
            XmlNode newNode = doc.CreateElement("", elementName, "");
            newNode.AppendChild(doc.CreateTextNode(textContent));
            parentNode.AppendChild(newNode);
        }

        public async Task<FunctionResult<bool>> XuLyThongDiepAsync(to_khai toKhai, KetQuaThongDiepRespone ketQuaThongDiepRespone, string xmlThongDiep)
        {
            var lyDoTuChoi = ketQuaThongDiepRespone?.DLieu?.TBao?.DLTBao?.DSLDKCNhan?.LDo?.MTa ?? "";
            var THOP = ketQuaThongDiepRespone?.DLieu?.TBao?.DLTBao?.THop ?? "";
            var TTXNCQT = ketQuaThongDiepRespone?.DLieu?.TBao?.DLTBao?.TTXNCQT ?? "";
            var MLTDiep = ketQuaThongDiepRespone.TTChung.MLTDiep;
            var MCCQT = ketQuaThongDiepRespone?.DLieu?.TBao?.DLTBao?.MCCQT ?? "";

            if (xmlThongDiep.Contains("<MLTDiep>999</MLTDiep>") && xmlThongDiep.Contains("<TTTNhan>0</TTTNhan>"))
            {
                toKhai.to_khai_status_id = (int)e_thong_bao_sai_sot_trang_thai.DA_GUI_CQT;

            }
            if (xmlThongDiep.Contains("<MLTDiep>999</MLTDiep>") && xmlThongDiep.Contains("<TTTNhan>1</TTTNhan>"))
            {
                toKhai.to_khai_status_id = (int)e_thong_bao_sai_sot_trang_thai.LOI_THONG_DIEP;

            }
            if (xmlThongDiep.Contains("<MLTDiep>-1</MLTDiep>"))
            {
                toKhai.to_khai_status_id = (int)e_thong_bao_sai_sot_trang_thai.LOI_THONG_DIEP;

            }
            if (MLTDiep == "102")
            {
                if (THOP == "1" || THOP == "3")
                {
                    toKhai.to_khai_status_id = (int)e_thong_bao_sai_sot_trang_thai.CHO_CQT;
                }
                if (THOP == "2" || THOP == "4")
                {
                    toKhai.to_khai_status_id = (int)e_thong_bao_sai_sot_trang_thai.CQT_TU_CHOI;
                }
            }
            if (MLTDiep == "103")
            {
                if (TTXNCQT == "1")
                {
                    toKhai.to_khai_status_id = (int)e_thong_bao_sai_sot_trang_thai.CQT_DONG_Y;
                    toKhai.ma_dang_ky = MCCQT;
                    var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(toKhai.mst);
                    if (donVi != null)
                    {
                        donVi.to_khai_success_id = toKhai.id;
                        await _serviceWrapper.Category.DonVi.UpdateAsync(donVi);
                    }
                }
                if (TTXNCQT == "2")
                {
                    toKhai.to_khai_status_id = (int)e_thong_bao_sai_sot_trang_thai.CQT_TU_CHOI;
                }
            }

            var isUpdated = await this.UpdateAsync(toKhai);
            var fileName = Guid.NewGuid().ToString() + ".xml";
            var filePath = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{fileName}";
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            await File.WriteAllTextAsync(filePath, xmlThongDiep);
            var noiDungThucHien = "";
            var to_khai_log_type_id = 0;
            if (toKhai.to_khai_status_id == (int)e_thong_bao_sai_sot_trang_thai.CHO_CQT)
            {
                noiDungThucHien = "CQT đã tiếp nhận";
                to_khai_log_type_id = (int)e_to_khai_log_type.GUI_CQT;
            }
            if (toKhai.to_khai_status_id == (int)e_thong_bao_sai_sot_trang_thai.CQT_DONG_Y)
            {
                noiDungThucHien = "CQT đã đồng ý";
                to_khai_log_type_id = (int)e_to_khai_log_type.CQT_DONG_Y;

            }
            if (toKhai.to_khai_status_id == (int)e_thong_bao_sai_sot_trang_thai.CQT_TU_CHOI)
            {
                noiDungThucHien = $"CQT Từ chối với lý do: {lyDoTuChoi}";
                to_khai_log_type_id = (int)e_to_khai_log_type.CQT_TU_CHOI;

            }
            var log = new to_khai_log()
            {
                file_thong_diep_url = filePath,
                ngay_thuc_hien = DateTime.Now,
                nguoi_thuc_hien = "Cơ quan thuế",
                to_khai_id = toKhai.id,
                noi_dung_thuc_hien = noiDungThucHien,
                to_khai_log_type_id = to_khai_log_type_id
            };
            log.SetInsertInfo(0);
            await _serviceWrapper.ToKhaiSerivceWrapper.ToKhaiLog.InsertAsync(log);
            if (ketQuaThongDiepRespone.TTChung.MLTDiep != "999")
            {
                await _hoaDonPhatHanhHub.OnToKhaiNotifyCreated(new Model.Request.Hub.TBSSPhatHanhPushNotifyModel()
                {
                    file_thong_diep_url = filePath,
                    thong_bao_sai_sot_trang_thai_id = toKhai.to_khai_status_id,
                    id = toKhai.id,
                    ket_qua_phat_hanh = log.noi_dung_thuc_hien,
                    user_id = toKhai.user_id_phathanh.ToString()
                });
            }
            return new SuccessResult<bool>("");

        }

        public async Task<FunctionResult<string>> GetHtmlPrintAsync(int id)
        {
            try
            {
                var toKhai = await this.SelectByIdAsync(id);
                if (toKhai == null) return new ErrorResult<string>("Không tìm thấy dữ liệu hợp lệ");
                // var toKhaiHistory = (await _serviceWrapper.ToKhaiSerivceWrapper.ToKhaiLog.SelectByToKhaiAsync(id)).Where(x => x.file_thong_diep_url != string.Empty).ToList().OrderByDescending(x => x.id).FirstOrDefault();
                // if (toKhaiHistory == null) return new ErrorResult<string>("Không tìm thấy dữ liệu hợp lệ");
                // var xmlFilePath = toKhaiHistory.file_thong_diep_url;
                var xsltPath = "Template/to_khai/tokhai2.xslt";
                var xsltArgument = new XsltArgumentList();
                // var xmlData = File.ReadAllText("Template/to_khai/test.xml");
                var xmlData2 = await this.CreateXmlKySoAsync(id);
                xmlData2 = xmlData2.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");
                XmlDocument doc = new XmlDocument();
                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", "");
                doc.AppendChild(docNode);

                string sId = Guid.NewGuid().ToString();

                // Create root element 'TKhai'
                XmlElement TDiepNode = doc.CreateElement("", "TDiep", "");
                doc.AppendChild(TDiepNode);
                XmlNode DLieuNote = doc.CreateElement("", "DLieu", "");
                DLieuNote.InnerXml = xmlData2;
                TDiepNode.AppendChild(DLieuNote);
                var html = await _serviceWrapper.Xslt.FillDataAsXmlAsync(xsltPath, doc.InnerXml, xsltArgument);
                return html.is_success ? new SuccessResult<string>(html.data) : new ErrorResult<string>(html.message);
            }
            catch (System.Exception ex)
            {
                return new ErrorResult<string>(ex.Message);
            }
        }

        public async Task<FunctionResult<string>> GetHtmlPhatHanhAsync(int id)
        {
            try
            {
                var toKhai = await this.SelectByIdAsync(id);
                if (toKhai == null) return new ErrorResult<string>("Không tìm thấy dữ liệu hợp lệ");

                if (toKhai.to_khai_status_id == 2)
                {
                    return new SuccessResult<string>("");
                }

                var toKhaiHistory = (await _serviceWrapper.ToKhaiSerivceWrapper.ToKhaiLog.SelectByToKhaiAsync(id)).Where(x => x.file_thong_diep_url != string.Empty).ToList().OrderByDescending(x => x.id).FirstOrDefault();
                if (toKhaiHistory == null) return new ErrorResult<string>("Không tìm thấy dữ liệu hợp lệ");
                var xmlFilePath = toKhaiHistory.file_thong_diep_url;
                var xsltPath = "Template/to_khai/ThongbaoChapnhan_CQT.xslt";
                var xsltArgument = new XsltArgumentList();
                var xmlData = File.ReadAllText(xmlFilePath);
                // var xmlData2 = await this.CreateXmlKySoAsync(id);
                // xmlData2
                // xmlData2 = xmlData2.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");
                // XmlDocument doc = new XmlDocument();
                // XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", "");
                // doc.AppendChild(docNode);

                // string sId = Guid.NewGuid().ToString();

                // Create root element 'TKhai'
                // XmlElement TDiepNode = doc.CreateElement("", "TDiep", "");
                // doc.AppendChild(TDiepNode);
                // XmlNode DLieuNote = doc.CreateElement("", "DLieu", "");
                // DLieuNote.InnerXml = xmlData2;
                // TDiepNode.AppendChild(DLieuNote);
                var html = await _serviceWrapper.Xslt.FillDataAsXmlAsync(xsltPath, xmlData, xsltArgument);
                return html.is_success ? new SuccessResult<string>(html.data) : new ErrorResult<string>(html.message);
            }
            catch (System.Exception ex)
            {
                return new ErrorResult<string>(ex.Message);
            }
        }


        public async Task<FunctionResult<string>> KySoVaPhatHanhAsync(int id)
        {
            var user = this.GetCurrentUser();
            var userId = user.id;
            var xml = await this.CreateXmlKySoAsync(id);
            var base64 = xml.ConvertToBase64();
            var userSerialInfo = await _serviceWrapper.User.User.SelectByIdAsync(userId);
            var mst = user.donvi_ma_dv;
            FunctionResult<string> ketQuaKy = null;
            // LogWriter.Writer(Newtonsoft.Json.JsonConvert.SerializeObject(userSerialInfo), "KySoVaPhatHanhAsync", "ToKhai " + id.ToString());
            if (userSerialInfo.is_hsm_signing)
            {
                ketQuaKy = await this.KySoHSMAsync(base64, userSerialInfo.serial_number, mst);
                if (ketQuaKy != null && ketQuaKy.is_success)
                {
                    var result = await this.PhatHanhAsync(id, ketQuaKy.data);
                    return new SuccessResult<string>(result);
                }
            }
            if (userSerialInfo.rs_ma_but_ky.ConvertToString() != "")
            {
                return await this.KySoRemoteAndPhatHanhBackgroundAsync(base64, userSerialInfo.serial_number, mst, id);

            }

            return new ErrorResult<string>();
        }
        private async Task<FunctionResult<string>> KySoHSMAsync(string base64, string serial, string mst)
        {
            var signResultModel = await _serviceWrapper.ApiSignHoaDon.SignAsync(base64, mst, serial);
            if (signResultModel != null && signResultModel.Macode == 1)
            {
                return new SuccessResult<string>(signResultModel.SignedData);
            }

            return new ErrorResult<string>();
        }
        private async Task<FunctionResult<string>> KySoRemoteAndPhatHanhBackgroundAsync(string base64, string serial, string mst, int id)
        {
            var userId = this.GetCurrentUserId();
            var kySoRequest = new KySoToKhaiRequest(serial, mst, base64, "");
            kySoRequest.id = id;
            LogWriter.Writer(base64, $"Ky RS to khai id {id}", "");
            var guiYeucauResult = await _serviceWrapper.RemoteSigningSerivce.KySoAsync(kySoRequest);
            if (guiYeucauResult.is_success)
            {
                // LogWriter.Writer(guiYeucauResult.data, $"Ky RS to khai id {id} code", "");
                var code = guiYeucauResult.data;
                _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                     {
                         await TryGetAndProcesssQuaKySoRsBackgroundAsync(code, id, userId);
                     });
                return new SuccessResult<string>(code);
                // var cts = new CancellationTokenSource();
                // var ketQuaKy = await _serviceWrapper.RemoteSigningSerivce.TryGetKetQuaKyThenClearAsync(code, "", cts.Token);
                // LogWriter.Writer(Newtonsoft.Json.JsonConvert.SerializeObject(ketQuaKy), $"Ky RS to khai id {id} ket qua", "");
                // if (ketQuaKy.is_success)
                // {

                //     return new SuccessResult<string>(ketQuaKy.data);
                // }
                // return new ErrorResult<string>(ketQuaKy.data);
            }

            return new ErrorResult<string>(guiYeucauResult.message);
        }
        public async Task<bool> TryGetAndProcesssQuaKySoRsBackgroundAsync(string code, int to_khai_id, int user_id_phathanh)
        {
            var cts = new CancellationTokenSource();
            var ketQuaKy = await _serviceWrapper.RemoteSigningSerivce.TryGetKetQuaKyThenClearAsync(code, "", cts.Token);
            LogWriter.Writer(Newtonsoft.Json.JsonConvert.SerializeObject(ketQuaKy), $"Ky RS to khai id {to_khai_id} ket qua", "");
            if (ketQuaKy.is_success)
            {
                var x = ketQuaKy.data;
                var result = await this.PhatHanhAsync(to_khai_id, ketQuaKy.data, user_id_phathanh);
                return true;

                // return new SuccessResult<string>(ketQuaKy.data);
            }
            return false;
            // return new ErrorResult<string>(ketQuaKy.data);
        }
    }
    public class TTTCGP
    {
        public string id { get; set; }
        public string TTCGP { get; set; }
        public string MSTTCGP { get; set; }
        public string TNgay { get; set; }
        public string DNgay { get; set; }
    }
    public class TCTN
    {
        public string id { get; set; }
        public string TTCTN { get; set; }
        public string MSTTCTN { get; set; }
        public string TNgay { get; set; }
        public string DNgay { get; set; }
    }
    public class TTTNSDungRoot
    {
        public TTTNSDungData TTTNSDung { get; set; }
    }

    public class TTTNSDungData
    {
        public List<TNSDung> TNSDung { get; set; }
    }

    public class TNSDung
    {
        public string STT { get; set; }
        public string TTCGP { get; set; }
        public string MSTTCGP { get; set; }
        public string TNgay { get; set; }
        public string DNgay { get; set; }
    }
}
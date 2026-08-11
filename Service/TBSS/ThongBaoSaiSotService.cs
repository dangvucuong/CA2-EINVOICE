using System.Xml;
using Contracts.Service.TBSS;
using Model.Base;
using Model.Static;
using Model.Table;
using Service.Base;
using Common;
using System.Text;
using Model.Respone.Xml;
using Model.Enum;
using Model.Request.TBSS;
using Service.Hub;
using Microsoft.Extensions.DependencyInjection;
using Model.RemoteSigning;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using Model.Respone.Upload;
using Model.Request.Upload;
using System.Data;

namespace Service.TBSS
{
    public class ThongBaoSaiSotService : CRUDService<thong_bao_sai_sot>, IThongBaoSaiSotService
    {
        HoaDonPhatHanhHub _hoaDonPhatHanhHub;

        public ThongBaoSaiSotService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.ThongBaoSaiSot.ThongBaoSaiSot;
            this._hoaDonPhatHanhHub = _serviceProvider.GetRequiredService<HoaDonPhatHanhHub>();
        }

        public async Task<string> CreateXmlKySoAsync(int id)
        {
            try
            {
                var taskThongBao = this.SelectByIdAsync(id);
                var taskThongBaoChiTiets =
                    _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSotChiTiet.SelectByThongBaoIdAsync(id);
                await Task.WhenAll(taskThongBao, taskThongBaoChiTiets);
                var thongBao = taskThongBao.Result;
                var thongBaoChiTiets = taskThongBaoChiTiets.Result.ToList();
                var taskDonVi = _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(thongBao?.donvi_ma_dv ?? "");
                var taskCoQuanThue = _serviceWrapper.Category.CoQuanThue.SelectByMaAsync(thongBao?.ma_cqt ?? "");
                await Task.WhenAll(taskDonVi, taskCoQuanThue);
                var donVi = taskDonVi.Result;
                var coQuanThue = taskCoQuanThue.Result;
                // var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(thongBao?.donvi_ma_dv ?? "");
                // var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(thongBao?.donvi_ma_dv ?? "");
                var So = "1";
                var Loai = "1";
                var NTBCCQT = "";
                var MCQT = thongBao.ma_cqt;

                var MDVQHNSach = "";
                var NTBao = thongBao.ngay_thong_bao.ToString("yyyy-MM-dd");
                string kq = "";
                //Tao thong tin XML chung

                string linkelement = "";
                XmlDocument doc = new XmlDocument();
                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", "");
                doc.AppendChild(docNode);

                //The TBao
                XmlElement TBaoNode = doc.CreateElement("", "TBao", linkelement);
                doc.AppendChild(TBaoNode);
                string sId = Guid.NewGuid().ToString();

                //The DLTBao
                XmlNode DLTBaoNode = doc.CreateElement("", "DLTBao", linkelement);
                XmlAttribute productAttribute = doc.CreateAttribute("Id");
                productAttribute.Value = "_" + sId;
                DLTBaoNode.Attributes.Append(productAttribute);
                TBaoNode.AppendChild(DLTBaoNode);

                //PBan
                XmlNode PBanNode = doc.CreateElement("", "PBan", linkelement);
                PBanNode.AppendChild(doc.CreateTextNode("2.1.0"));
                DLTBaoNode.AppendChild(PBanNode);
                //MSo
                XmlNode MSoNode = doc.CreateElement("", "MSo", linkelement);
                MSoNode.AppendChild(doc.CreateTextNode(thongBao.ma_so));
                DLTBaoNode.AppendChild(MSoNode);
                //Ten
                XmlNode TenNode = doc.CreateElement("", "Ten", linkelement);
                TenNode.AppendChild(doc.CreateTextNode(thongBao.ten_thong_bao));
                DLTBaoNode.AppendChild(TenNode);
                //Loai
                XmlNode LoaiNode = doc.CreateElement("", "Loai", linkelement);
                LoaiNode.AppendChild(doc.CreateTextNode(Loai));
                DLTBaoNode.AppendChild(LoaiNode);

                //So
                if (!string.IsNullOrEmpty(So))
                {
                    XmlNode SoNode = doc.CreateElement("", "So", linkelement);
                    SoNode.AppendChild(doc.CreateTextNode(So));
                    DLTBaoNode.AppendChild(SoNode);
                }

                //NTBCCQT
                if (!string.IsNullOrEmpty(NTBCCQT))
                {
                    XmlNode NTBCCQTNode = doc.CreateElement("", "NTBCCQT", linkelement);
                    NTBCCQTNode.AppendChild(doc.CreateTextNode(NTBCCQT));
                    DLTBaoNode.AppendChild(NTBCCQTNode);
                }

                //MCQT
                XmlNode MCQTNode = doc.CreateElement("", "MCQT", linkelement);
                MCQTNode.AppendChild(doc.CreateTextNode(MCQT));
                DLTBaoNode.AppendChild(MCQTNode);

                //TCQT
                XmlNode TCQTNode = doc.CreateElement("", "TCQT", linkelement);
                TCQTNode.AppendChild(doc.CreateTextNode(coQuanThue?.ten ?? thongBao.ma_cqt));
                DLTBaoNode.AppendChild(TCQTNode);
                //TNNT
                XmlNode TNNTNode = doc.CreateElement("", "TNNT", linkelement);
                TNNTNode.AppendChild(doc.CreateTextNode(donVi?.ten_dv ?? thongBao.donvi_ma_dv));
                DLTBaoNode.AppendChild(TNNTNode);
                //MST
                XmlNode MSTNode = doc.CreateElement("", "MST", linkelement);
                MSTNode.AppendChild(doc.CreateTextNode(thongBao.donvi_ma_dv));
                DLTBaoNode.AppendChild(MSTNode);

                //MDVQHNSach
                if (!string.IsNullOrEmpty(MDVQHNSach))
                {
                    XmlNode MDVQHNSachNode = doc.CreateElement("", "MDVQHNSach", linkelement);
                    MDVQHNSachNode.AppendChild(doc.CreateTextNode(MDVQHNSach));
                    DLTBaoNode.AppendChild(MDVQHNSachNode);
                }

                //DDanh
                XmlNode DDanhNode = doc.CreateElement("", "DDanh", linkelement);
                DDanhNode.AppendChild(doc.CreateTextNode(thongBao.dia_danh));
                DLTBaoNode.AppendChild(DDanhNode);
                //NTBao
                XmlNode NTBaoNode = doc.CreateElement("", "NTBao", linkelement);
                NTBaoNode.AppendChild(doc.CreateTextNode(NTBao));
                DLTBaoNode.AppendChild(NTBaoNode);
                //DSHDon
                XmlNode DSHDonNode = doc.CreateElement("", "DSHDon", linkelement);
                DLTBaoNode.AppendChild(DSHDonNode);

                if (thongBaoChiTiets.Count > 0)
                {
                    for (int i = 0; i < thongBaoChiTiets.Count; i++)
                    {
                        var thongBaoChiTiet = thongBaoChiTiets[i];
                        //HDon
                        XmlNode HDonNode = doc.CreateElement("", "HDon", linkelement);
                        DSHDonNode.AppendChild(HDonNode);
                        //STT
                        XmlNode STTNode = doc.CreateElement("", "STT", linkelement);
                        STTNode.AppendChild(doc.CreateTextNode((i + 1).ToString()));
                        HDonNode.AppendChild(STTNode);

                        //KHMSHDon
                        XmlNode KHMSHDonNode = doc.CreateElement("", "KHMSHDon", linkelement);
                        KHMSHDonNode.AppendChild(doc.CreateTextNode(thongBaoChiTiet.hoa_don_dang_ky_phat_hanh_mau_so));
                        HDonNode.AppendChild(KHMSHDonNode);
                        //KHHDon
                        XmlNode KHHDonNode = doc.CreateElement("", "KHHDon", linkelement);
                        KHHDonNode.AppendChild(doc.CreateTextNode(thongBaoChiTiet.hoa_don_dang_ky_phat_hanh_ky_hieu));
                        HDonNode.AppendChild(KHHDonNode);
                        //SHDon
                        XmlNode SHDonNode = doc.CreateElement("", "SHDon", linkelement);
                        SHDonNode.AppendChild(doc.CreateTextNode(thongBaoChiTiet.ma_so_hoa_don));
                        HDonNode.AppendChild(SHDonNode);
                        //Ngay
                        XmlNode NgayNode = doc.CreateElement("", "Ngay", linkelement);
                        NgayNode.AppendChild(doc.CreateTextNode(thongBaoChiTiet.ngay_hoa_don.ToString("yyyy-MM-dd")));
                        HDonNode.AppendChild(NgayNode);
                        //MCQTCap
                        XmlNode MCQTCapNode = doc.CreateElement("", "MCCQT", linkelement);
                        MCQTCapNode.AppendChild(doc.CreateTextNode(thongBaoChiTiet.ma_cqt_cap));
                        HDonNode.AppendChild(MCQTCapNode);
                        // //MCQTCap
                        // XmlNode MCQTCapNode = doc.CreateElement("", "MCQTCap", linkelement);
                        // MCQTCapNode.AppendChild(doc.CreateTextNode(thongBaoChiTiet.ma_cqt_cap));
                        // HDonNode.AppendChild(MCQTCapNode);


                        //LADHDDT
                        XmlNode LADHDDTNode = doc.CreateElement("", "LADHDDT", linkelement);
                        LADHDDTNode.AppendChild(doc.CreateTextNode(thongBao.loai_hoa_don_dien_tu_id.ToString()));
                        HDonNode.AppendChild(LADHDDTNode);
                        //TCTBao
                        XmlNode TCTBaoNode = doc.CreateElement("", "TCTBao", linkelement);
                        TCTBaoNode.AppendChild(doc.CreateTextNode(thongBao.thong_bao_sai_sot_tinh_chat_id.ToString()));
                        HDonNode.AppendChild(TCTBaoNode);

                        //LDo
                        if (!string.IsNullOrEmpty(thongBao.ly_do))
                        {
                            XmlNode LDoNode = doc.CreateElement("", "LDo", linkelement);
                            LDoNode.AppendChild(doc.CreateTextNode(thongBao.ly_do));
                            HDonNode.AppendChild(LDoNode);
                        }
                    }
                }

                // DS CKS
                XmlNode DSCKSNode = doc.CreateElement("", "DSCKS", linkelement);
                TBaoNode.AppendChild(DSCKSNode);
                // NNT CKS

                XmlNode NNTCKSNode = doc.CreateElement("", "NNT", linkelement);
                DSCKSNode.AppendChild(NNTCKSNode);
                //NMua CKS
                XmlNode CCKSKhacCKSNode = doc.CreateElement("", "CCKSKhac", linkelement);
                DSCKSNode.AppendChild(CCKSKhacCKSNode);
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
                var log = new thong_bao_sai_sot_log()
                {
                    file_thong_diep_url = filePath,
                    ngay_thuc_hien = DateTime.Now,
                    nguoi_thuc_hien = "Cơ quan thuế",
                    thong_bao_sai_sot_id = id,
                    noi_dung_thuc_hien = "Tạo XML ký số",
                    thong_bao_sai_sot_log_type_id = -1
                };
                log.SetInsertInfo(0);
                await _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSotLog.InsertAsync(log);
                return kq;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private string GetHoaDonType(string kyHieu)
        {
            if (kyHieu.ConvertToString().Length >= 4)
            {
                if (kyHieu.ConvertToString().Substring(3, 1).ConvertToString().ToUpper() == "M") return "M";
            }

            if (kyHieu.ConvertToString().FirstOrDefault().ConvertToString().ToUpper() == "K") return "K";
            if (kyHieu.ConvertToString().FirstOrDefault().ConvertToString().ToUpper() == "C") return "C";

            return "";
        }

        public async Task<string> CreateXmlThongDiepAsync(int id, string signedText)
        {
            try
            {
                var taskThongBao = this.SelectByIdAsync(id);
                var taskThongBaoChiTiets =
                    _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSotChiTiet.SelectByThongBaoIdAsync(id);
                await Task.WhenAll(taskThongBao, taskThongBaoChiTiets);
                var thongBao = taskThongBao.Result;
                var thongBaoChiTiets = taskThongBaoChiTiets.Result.ToList();
                // var thongBao = await this.SelectByIdAsync(id);
                if (thongBao == null) return String.Empty;
                var guidstr = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
                thongBao.phat_hanh_uuid = guidstr;
                var userId = this.GetCurrentUserId();
                await _serviceWrapper.Cache.SetDataAsync<string>(guidstr, "tbss", DateTime.Now.AddDays(30));
                await _repositoryWrapper.HoaDon.PhatHanhUUID.SaveLogUuidAsync(guidstr, "tbss", userId);
                await this.UpdateAsync(thongBao);
                await _serviceWrapper.Cache.SetDataAsync<thong_bao_sai_sot>(guidstr + "_tbss", thongBao,
                    DateTime.Now.AddDays(30));
                var isThongBaoHoaDonMTT = thongBaoChiTiets.Any(x =>
                    this.GetHoaDonType(x.hoa_don_dang_ky_phat_hanh_ky_hieu) == "M");
                var MNGui = AppSettings.FixedValue.MNGui;
                var MNNhan = AppSettings.FixedValue.MNNhan;
                var MLTDiep = isThongBaoHoaDonMTT ? "303" : "300";
                var MTDiep = AppSettings.FixedValue.MNGui + guidstr.Replace("-", "");
                var MTDTChieu = "";
                var SLuong = "1";
                string kq = "";
                //Tao thong tin XML chung

                string linkelement = "";

                XmlDocument doc = new XmlDocument();
                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                doc.AppendChild(docNode);

                //The TDiep
                XmlElement TDiepNode = doc.CreateElement("", "TDiep", linkelement);
                doc.AppendChild(TDiepNode);
                //TT Chung
                XmlElement TTChungTDNode = doc.CreateElement("", "TTChung", linkelement);
                TDiepNode.AppendChild(TTChungTDNode);
                //PBan
                XmlNode PBanTTNode = doc.CreateElement("", "PBan", linkelement);
                PBanTTNode.AppendChild(doc.CreateTextNode("2.1.0"));
                TTChungTDNode.AppendChild(PBanTTNode);
                //MNGui
                XmlNode MNGuiNode = doc.CreateElement("", "MNGui", linkelement);
                MNGuiNode.AppendChild(doc.CreateTextNode(MNGui));
                TTChungTDNode.AppendChild(MNGuiNode);
                // MNNhan
                XmlNode MNNhanNode = doc.CreateElement("", "MNNhan", linkelement);
                MNNhanNode.AppendChild(doc.CreateTextNode(MNNhan));
                TTChungTDNode.AppendChild(MNNhanNode);
                //MLTDiep
                XmlNode MLTDiepNode = doc.CreateElement("", "MLTDiep", linkelement);
                MLTDiepNode.AppendChild(doc.CreateTextNode(MLTDiep));
                TTChungTDNode.AppendChild(MLTDiepNode);
                //MTDiep
                XmlNode MTDiepNode = doc.CreateElement("", "MTDiep", linkelement);
                MTDiepNode.AppendChild(doc.CreateTextNode(MTDiep));
                TTChungTDNode.AppendChild(MTDiepNode);
                //MTDTChieu
                XmlNode MTDTChieuNode = doc.CreateElement("", "MTDTChieu", linkelement);
                MTDTChieuNode.AppendChild(doc.CreateTextNode(MTDTChieu));
                TTChungTDNode.AppendChild(MTDTChieuNode);
                //MST
                XmlNode MSTTTNode = doc.CreateElement("", "MST", linkelement);
                MSTTTNode.AppendChild(doc.CreateTextNode(thongBao.donvi_ma_dv));
                TTChungTDNode.AppendChild(MSTTTNode);
                //SLuong
                XmlNode SLuongNode = doc.CreateElement("", "SLuong", linkelement);
                SLuongNode.AppendChild(doc.CreateTextNode(SLuong));
                TTChungTDNode.AppendChild(SLuongNode);
                //DLieu
                XmlElement DLieuNode = doc.CreateElement("", "DLieu", linkelement);
                TDiepNode.AppendChild(DLieuNode);
                XmlNodeList lstNode = doc.GetElementsByTagName("DLieu");
                XmlNode convert = XmlStringToXmlNode(signedText);

                for (int i = 0; i < lstNode.Count; i++)
                {
                    XmlNode xnode = lstNode[lstNode.Count - 1];
                    xnode.AppendChild(xnode.OwnerDocument.ImportNode(convert, true));
                }

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
                var log = new thong_bao_sai_sot_log()
                {
                    file_thong_diep_url = filePath,
                    ngay_thuc_hien = DateTime.Now,
                    nguoi_thuc_hien = "Cơ quan thuế",
                    thong_bao_sai_sot_id = id,
                    noi_dung_thuc_hien = "Tạo XML thông điệp",
                    thong_bao_sai_sot_log_type_id = -1
                };
                log.SetInsertInfo(0);
                await _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSotLog.InsertAsync(log);
                return kq;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
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

        public async Task<IEnumerable<thong_bao_sai_sot>> SelectByDonViAsync(string donvi_ma_dv)
        {
            return await _repositoryWrapper.ThongBaoSaiSot.ThongBaoSaiSot.SelectByDonViAsync(donvi_ma_dv);
        }

        public async Task<FunctionResult<bool>> PhatHanhAsync(int id, string signedText)
        {
            var thongBaoSaiSot = await this.SelectByIdAsync(id);
            thongBaoSaiSot.user_id_phathanh = this.GetCurrentUserId();
            await this.UpdateAsync(thongBaoSaiSot);
            var xml = await this.CreateXmlThongDiepAsync(id, signedText);
            var base64thongdiep = xml.ConvertToBase64();
            using (var client = Helper.WSInterTRCA2Helper.GetClient())
            {
                await client.OpenAsync();
                var authHeader = Helper.WSInterTRCA2Helper.GetAuthHeader();
                var guiThongDiepResult = await client.Guithongdiep2024Async(authHeader, base64thongdiep, 1);
                await client.CloseAsync();

                return new SuccessResult<bool>();
            }
        }

        public async Task<FunctionResult<bool>> XuLyThongDiepAsync(thong_bao_sai_sot thongBaoSaiSot,
            KetQuaThongDiepRespone ketQuaThongDiepRespone, string xmlThongDiep)
        {
            var lyDoTuChoi = ketQuaThongDiepRespone?.DLieu?.TBao?.DLTBao?.DSHDon?.HDon?.DSLDKTNhan?.LDo?.MTa ?? "";
            var lyDoTuChoiKhac = ketQuaThongDiepRespone?.DLieu?.TBao?.DLTBao?.KHLKhac?.DSLDo?.LDo?.MTLoi ?? "";
            var ket_qua_phan_hoi = "";
            if (ketQuaThongDiepRespone.TTChung.MLTDiep.ConvertToString() == "999")
            {
                thongBaoSaiSot.thong_bao_sai_sot_trang_thai_id = (int)e_thong_bao_sai_sot_trang_thai.CHO_CQT;
                ket_qua_phan_hoi = "CQT Nhận thông điệp";
            }
            var MTLoi = "";
            string pattern = @"<MTLoi>(.*?)</MTLoi>";
            var match = Regex.Match(xmlThongDiep, pattern, RegexOptions.Singleline);
            if (match.Success)
            {
                MTLoi = match.Groups[1].Value;

            }
            if (ketQuaThongDiepRespone.TTChung.MLTDiep.ConvertToString() == "301")
            {

                if (MTLoi == "")
                {
                    thongBaoSaiSot.thong_bao_sai_sot_trang_thai_id = (int)e_thong_bao_sai_sot_trang_thai.CQT_DONG_Y;
                    ket_qua_phan_hoi = "CQT Chấp nhận";
                }
                else
                {
                    thongBaoSaiSot.thong_bao_sai_sot_trang_thai_id = (int)e_thong_bao_sai_sot_trang_thai.CQT_TU_CHOI;
                }

                //TTTNCCQT
                // var TTTNCCQT = ketQuaThongDiepRespone?.DLieu?.TBao?.DLTBao?.DSHDon?.HDon?.TTTNCCQT ?? "";
                // if (TTTNCCQT == "1")
                // {
                //     thongBaoSaiSot.thong_bao_sai_sot_trang_thai_id = (int)e_thong_bao_sai_sot_trang_thai.CQT_DONG_Y;
                //     ket_qua_phan_hoi = "CQT Chấp nhận";
                // }
                // else
                // {
                //     thongBaoSaiSot.thong_bao_sai_sot_trang_thai_id = (int)e_thong_bao_sai_sot_trang_thai.CQT_TIEP_NHAN;
                // }
            }

            if (ketQuaThongDiepRespone.TTChung.MLTDiep.ConvertToString() == "204")
            {
                thongBaoSaiSot.thong_bao_sai_sot_trang_thai_id = (int)e_thong_bao_sai_sot_trang_thai.CQT_TU_CHOI;
            }

            if (ket_qua_phan_hoi == "")
            {
                ket_qua_phan_hoi = thongBaoSaiSot.thong_bao_sai_sot_trang_thai_id ==
                                   (int)e_thong_bao_sai_sot_trang_thai.CQT_DONG_Y
                    ? "CQT Chấp nhận"
                    : $"CQT Từ chối với lý do: {lyDoTuChoi} {lyDoTuChoiKhac} {MTLoi}";
            }

            thongBaoSaiSot.ket_qua_phan_hoi = ket_qua_phan_hoi;
            var isUpdated = await this.UpdateAsync(thongBaoSaiSot);

            var fileName = Guid.NewGuid().ToString() + ".xml";
            var filePath = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{fileName}";
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            await File.WriteAllTextAsync(filePath, xmlThongDiep);
            var log = new thong_bao_sai_sot_log()
            {
                file_thong_diep_url = filePath,
                ngay_thuc_hien = DateTime.Now,
                nguoi_thuc_hien = "Cơ quan thuế",
                thong_bao_sai_sot_id = thongBaoSaiSot.id,
                noi_dung_thuc_hien = ket_qua_phan_hoi,
                thong_bao_sai_sot_log_type_id = thongBaoSaiSot.thong_bao_sai_sot_trang_thai_id ==
                                                (int)e_thong_bao_sai_sot_trang_thai.CQT_DONG_Y
                    ? (int)e_to_khai_log_type.CQT_DONG_Y
                    : (int)e_to_khai_log_type.CQT_TU_CHOI
            };
            log.SetInsertInfo(0);
            await _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSotLog.InsertAsync(log);
            if (!isUpdated) return new ErrorResult<bool>("");
            if (thongBaoSaiSot.thong_bao_sai_sot_trang_thai_id == (int)e_thong_bao_sai_sot_trang_thai.CQT_DONG_Y)
            {
                var thongBaoSaiSotChiTiets =
                    await _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSotChiTiet.SelectByThongBaoIdAsync(
                        thongBaoSaiSot.id);
                foreach (var thongBaoSaiSotChiTiet in thongBaoSaiSotChiTiets)
                {
                    var hoaDon = await _serviceWrapper.HoaDon.HoaDon.SelectByIdAsync(thongBaoSaiSotChiTiet.hoa_don_id);
                    if (hoaDon != null)
                    {
                        if (thongBaoSaiSot.thong_bao_sai_sot_tinh_chat_id == (int)e_thong_bao_sai_sot_tinh_chat.HUY)
                        {
                            hoaDon.hoa_don_hinh_thuc_id = (int)e_hoa_don_hinh_thuc.DA_GUI_TBSS_HUY;
                            hoaDon.hoa_don_trang_thai_id = (int)e_hoa_don_trang_thai.DA_HUY;
                        }

                        if (thongBaoSaiSot.thong_bao_sai_sot_tinh_chat_id ==
                            (int)e_thong_bao_sai_sot_tinh_chat.DIEU_CHINH)
                        {
                            hoaDon.hoa_don_hinh_thuc_id = (int)e_hoa_don_hinh_thuc.HOA_DON_BI_DIEU_CHINH;
                        }

                        if (thongBaoSaiSot.thong_bao_sai_sot_tinh_chat_id ==
                            (int)e_thong_bao_sai_sot_tinh_chat.THAY_THE)
                        {
                            hoaDon.hoa_don_hinh_thuc_id = (int)e_hoa_don_hinh_thuc.DA_GUI_TBSS_THAY_THE;
                        }

                        if (thongBaoSaiSot.thong_bao_sai_sot_tinh_chat_id ==
                            (int)e_thong_bao_sai_sot_tinh_chat.GIAI_TRINH)
                        {
                            hoaDon.hoa_don_hinh_thuc_id = (int)e_hoa_don_hinh_thuc.HOA_DON_DA_THONG_BAO_GIAI_TRINH;
                        }

                        await _serviceWrapper.HoaDon.HoaDon.UpdateAsync(hoaDon);
                        await _serviceWrapper.HoaDon.PushMessageToVender.CheckAndPushMessageAsync(hoaDon);
                        var logHoaDon = new hoa_don_log()
                        {
                            file_thong_diep_url = filePath,
                            ngay_thuc_hien = DateTime.Now,
                            nguoi_thuc_hien = "Cơ quan thuế",
                            noi_dung_thuc_hien = "Cơ quan thuế chấp nhận thông báo sai sót",
                            hoa_don_id = hoaDon.id,
                            hoa_don_log_type_id = (int)e_hoa_don_log_type.CHAP_NHAN_TBSS
                        };
                        logHoaDon.SetInsertInfo(0);
                        await _serviceWrapper.HoaDon.HoaDonLog.InsertAsync(logHoaDon);
                    }
                }
            }

            if (ketQuaThongDiepRespone.TTChung.MLTDiep != "999")
            {
                await _hoaDonPhatHanhHub.OnTBSSNotifyCreated(new Model.Request.Hub.TBSSPhatHanhPushNotifyModel()
                {
                    file_thong_diep_url = filePath,
                    thong_bao_sai_sot_trang_thai_id = thongBaoSaiSot.thong_bao_sai_sot_trang_thai_id,
                    id = thongBaoSaiSot.id,
                    ket_qua_phat_hanh = log.noi_dung_thuc_hien,
                    user_id = thongBaoSaiSot.user_id_phathanh.ToString()
                });
            }

            return new SuccessResult<bool>("");
        }

        public async Task<FunctionResult<thong_bao_sai_sot>> SaveChangesAsync(ThongBaoSaiSotAddOrEditRequest request)
        {
            var user = this.GetCurrentUser();
            var user_id = user.id;
            var obj = request.Map<thong_bao_sai_sot>();
            if (request.id <= 0)
            {
                obj.SetInsertInfo(user_id);
                obj.donvi_ma_dv = user.donvi_ma_dv;
                obj.id = await this.InsertAsync(obj);
            }
            else
            {
                var dbObj = await this.SelectByIdAsync(request.id);
                if (dbObj == null ||
                    dbObj.thong_bao_sai_sot_trang_thai_id != (int)e_thong_bao_sai_sot_trang_thai.TAO_MOI)
                {
                    return new ErrorResult<thong_bao_sai_sot>("Không tìm thấy thông báo sai sót hợp lệ");
                }

                dbObj.ma_cqt = obj.ma_cqt;
                dbObj.ten_cqt = obj.ten_cqt;
                dbObj.dia_danh = obj.dia_danh;
                dbObj.thong_bao_sai_sot_trang_thai_id = obj.thong_bao_sai_sot_trang_thai_id;
                dbObj.thong_bao_sai_sot_tinh_chat_id = obj.thong_bao_sai_sot_tinh_chat_id;
                dbObj.ly_do = obj.ly_do;
                dbObj.SetUpdateInfo(user_id);
                await this.UpdateAsync(dbObj);
            }

            if (request.id <= 0)
            {
                if (obj.id > 0)
                {
                    foreach (var item in request.thong_bao_sai_sot_chi_tiets)
                    {
                        item.thong_bao_sai_sot_id = obj.id;
                        item.SetInsertInfo(user_id);
                        item.id = await _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSotChiTiet.InsertAsync(item);
                    }

                    var log = new thong_bao_sai_sot_log()
                    {
                        file_thong_diep_url = string.Empty,
                        ngay_thuc_hien = DateTime.Now,
                        nguoi_thuc_hien = user.full_name,
                        thong_bao_sai_sot_id = obj.id,
                        noi_dung_thuc_hien = "Thêm mới",
                        thong_bao_sai_sot_log_type_id = (int)e_to_khai_log_type.TAO_MOI
                    };
                    log.SetInsertInfo(user_id);
                    await _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSotLog.InsertAsync(log);
                    await this.CreateXmlBienBanFromTbssAsync(obj.id);
                    return new SuccessResult<thong_bao_sai_sot>(obj);
                }
            }
            else
            {
                var thongBaoChiTietsDb =
                    await _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSotChiTiet.SelectByThongBaoIdAsync(obj.id);
                var thongBaoChiTietIdsDb = thongBaoChiTietsDb.Select(x => x.id).ToList();
                var thongBaoChiTietIdsNew = request.thong_bao_sai_sot_chi_tiets.Select(x => x.id).ToList();

                var thongBaoChiTietIdsDelete =
                    thongBaoChiTietIdsDb.Where(x => !thongBaoChiTietIdsNew.Contains(x)).ToList();


                foreach (var thongBaoChiTietIdDelete in thongBaoChiTietIdsDelete)
                {
                    await _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSotChiTiet.DeleteAsync(thongBaoChiTietIdDelete);
                }

                var thongBaochiTietsInsert = request.thong_bao_sai_sot_chi_tiets.Where(x => x.id == 0).ToList();
                foreach (var item in thongBaochiTietsInsert)
                {
                    item.thong_bao_sai_sot_id = obj.id;
                    item.SetInsertInfo(user_id);
                    item.id = await _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSotChiTiet.InsertAsync(item);
                }

                var log = new thong_bao_sai_sot_log()
                {
                    file_thong_diep_url = string.Empty,
                    ngay_thuc_hien = DateTime.Now,
                    nguoi_thuc_hien = user.full_name,
                    thong_bao_sai_sot_id = obj.id,
                    noi_dung_thuc_hien = "Cập nhật",
                    thong_bao_sai_sot_log_type_id = (int)e_to_khai_log_type.CAP_NHAT
                };
                log.SetInsertInfo(user_id);
                await _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSotLog.InsertAsync(log);
                await this.CreateXmlBienBanFromTbssAsync(obj.id);
                return new SuccessResult<thong_bao_sai_sot>(obj);
            }


            return new ErrorResult<thong_bao_sai_sot>("");
        }

        public async Task<FunctionResult<bool>> KySoVaPhatHanhAsync(int id)
        {
            var user = this.GetCurrentUser();
            var userId = user.id;
            var xml = await this.CreateXmlKySoAsync(id);
            var base64 = xml.ConvertToBase64();
            var userSerialInfo = await _serviceWrapper.User.User.SelectByIdAsync(userId);
            var mst = user.donvi_ma_dv;
            FunctionResult<string> ketQuaKy = null;
            if (userSerialInfo.is_hsm_signing)
                ketQuaKy = await this.KySoHSMAsync(base64, userSerialInfo.serial_number, mst);
            if (userSerialInfo.rs_ma_but_ky.ConvertToString() != "")
                ketQuaKy = await this.KySoRemoteAsync(base64, userSerialInfo.serial_number, mst, id);
            if (ketQuaKy != null && ketQuaKy.is_success)
            {
                await this.PhatHanhAsync(id, ketQuaKy.data);
            }

            return new ErrorResult<bool>();
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

        private async Task<FunctionResult<string>> KySoRemoteAsync(string base64, string serial, string mst, int id)
        {
            var kySoRequest = new KyTBSSRequest(serial, mst, base64, "");
            kySoRequest.id = id;

            var guiYeucauResult = await _serviceWrapper.RemoteSigningSerivce.KySoAsync(kySoRequest);
            if (guiYeucauResult.is_success)
            {
                var code = guiYeucauResult.data;
                var cts = new CancellationTokenSource();
                var ketQuaKy =
                    await _serviceWrapper.RemoteSigningSerivce.TryGetKetQuaKyThenClearAsync(code, "", cts.Token);
                if (ketQuaKy.is_success)
                {
                    return new SuccessResult<string>(ketQuaKy.data);
                }

                return new ErrorResult<string>(ketQuaKy.data);
            }

            return new ErrorResult<string>(guiYeucauResult.message);
        }

        public async Task<string> GetHtmlPreviewAsync(int id)
        {
            var taskThongBaoLog = (_serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSotLog.SelectByThongBaoIdAsync(id));
            var taskThongBao = this.SelectByIdAsync(id);
            // var taskThongBaoChiTiets = _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSotChiTiet.SelectByThongBaoIdAsync(id);
            await Task.WhenAll(
                taskThongBao
                // ,taskThongBaoChiTiets
                , taskThongBaoLog
            );
            var thongBao = taskThongBao.Result;
            // var thongBaoChiTiets = taskThongBaoChiTiets.Result.ToList();
            var thongBaoLogs = taskThongBaoLog.Result.ToList();
            var thongBaoLogSuccess = thongBaoLogs.Where(x => x.thong_bao_sai_sot_log_type_id == 4).LastOrDefault();
            var thongDiepGuiLog = thongBaoLogs
                .Where(x => x.noi_dung_thuc_hien == "Tạo XML thông điệp"
                    && !string.IsNullOrEmpty(x.file_thong_diep_url))
                .OrderByDescending(x => x.id)
                .FirstOrDefault();
            var taskDonVi = _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(thongBao?.donvi_ma_dv ?? "");
            var taskCoQuanThue = _serviceWrapper.Category.CoQuanThue.SelectByMaAsync(thongBao?.ma_cqt ?? "");
            await Task.WhenAll(taskDonVi, taskCoQuanThue);
            var donVi = taskDonVi.Result;
            var coQuanThue = taskCoQuanThue.Result;
            var xmlString = "";
            if (thongDiepGuiLog != null && File.Exists(thongDiepGuiLog.file_thong_diep_url))
            {
                xmlString = await File.ReadAllTextAsync(thongDiepGuiLog.file_thong_diep_url);
            }
            else
            {
                var xml = await this.CreateXmlKySoAsync(id);
                if (xml.ConvertToString() != "")
                {
                    var doc = XDocument.Parse(xml);
                    if (doc != null)
                    {
                        XElement currentRoot = doc.Root;
                        XElement tdiepElement = new XElement("TDiep");
                        XElement dLieuElement = new XElement("DLieu");
                        dLieuElement.Add(currentRoot);
                        tdiepElement.Add(dLieuElement);
                        currentRoot.ReplaceWith(tdiepElement);
                        var x509SubjectNameElement = new XElement("X509SubjectName");
                        x509SubjectNameElement.Value = "CN=" + (donVi?.ten_dv ?? "") + ",";
                        var SigningTimeElement = new XElement("SigningTime");
                        SigningTimeElement.Value = (thongBaoLogSuccess?.created_time ?? DateTime.Now).ToString("O")
                            .Substring(0, 19);
                        tdiepElement.Add(x509SubjectNameElement);
                        tdiepElement.Add(SigningTimeElement);
                        xmlString = GetCompactXmlString(doc);
                    }
                }
            }

            var xslt_path = "Template/tbss/Tdiep_04tbss_hd.xslt";
            var xsltArgument = new XsltArgumentList();
            var result = await _serviceWrapper.Xslt.FillDataAsXmlAsync(xslt_path, xmlString, xsltArgument);
            // var resultTest = await _serviceWrapper.Xslt.FillDataAsXmlAsync(xslt_path, xmlStringTest, xsltArgument);
            var html = "";

            if (result.is_success)
            {
                var input = result.data;
                string searchString = "<html"; //
                int index = result.data.IndexOf(searchString);
                if (index != -1)
                {
                    html = input.Substring(index);
                }

                html = result.data;
            }

            if (thongBao.thong_bao_sai_sot_trang_thai_id == (int)e_thong_bao_sai_sot_trang_thai.CQT_DONG_Y)
            {
                string pattern = @"<div\s+[^>]*style=['""]paramMau['""]\s*>\s*(.*?)\s*</div>";
                string replacement = @"<div id='background' style='paramMau'>
                    <div style='padding:10px;border:3px solid darkgreen;font-weight:bold;transform: rotate(-25deg);margin-top: -50px;'>ĐÃ DUYỆT</div>
                    </div>";
                html = Regex.Replace(html, pattern, replacement, RegexOptions.IgnoreCase);
                var stylemau =
                    "position:absolute;z-index:0;width:100%;height:100%;background:transparent;display:flex;justify-content:center;align-items:center;color:darkgreen;font-size:25px;text-align:center;";
                html = html.Replace("paramMau", stylemau);
            }

            if (thongBao.thong_bao_sai_sot_trang_thai_id == (int)e_thong_bao_sai_sot_trang_thai.CQT_TU_CHOI)
            {
                string pattern = @"<div\s+[^>]*style=['""]paramMau['""]\s*>\s*(.*?)\s*</div>";
                string replacement = @"<div id='background' style='paramMau'>
                    <div style='padding:10px;border:3px solid red;font-weight:bold;transform: rotate(-25deg);margin-top: -50px;'>TỪ CHỐI</div>
                    </div>";
                html = Regex.Replace(html, pattern, replacement, RegexOptions.IgnoreCase);
                var stylemau =
                    "position:absolute;z-index:0;width:100%;height:100%;background:transparent;display:flex;justify-content:center;align-items:center;color:red;font-size:25px;text-align:center;";
                html = html.Replace("paramMau", stylemau);
            }
            else
            {
                string pattern = @"<div\s+[^>]*style=['""]paramMau['""]\s*>\s*(.*?)\s*</div>";
                string replacement = @"<div id='background' style='paramMau'>
                    <div style='padding:10px;border:3px solid red;font-weight:bold;transform: rotate(-25deg);margin-top: -50px;'>CHƯA GỬI CQT</div>
                    </div>";
                html = Regex.Replace(html, pattern, replacement, RegexOptions.IgnoreCase);
                var stylemau =
                    "position:absolute;z-index:0;width:100%;height:100%;background:transparent;display:flex;justify-content:center;align-items:center;color:red;font-size:25px;text-align:center;";
                html = html.Replace("paramMau", stylemau);
            }

            return html;
            // }

            return string.Empty;
        }

        public async Task<FunctionResult<string>> GetHtmlKetQuaAsync(int id)
        {
            try
            {
                var thongBao = await this.SelectByIdAsync(id);
                if (thongBao == null) return new ErrorResult<string>("Không tìm thấy dữ liệu hợp lệ");

                var thongBaoHistory = (await _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSotLog.SelectByThongBaoIdAsync(id))
                    .Where(x => !string.IsNullOrEmpty(x.file_thong_diep_url)
                        && x.thong_bao_sai_sot_log_type_id == (int)e_to_khai_log_type.CQT_DONG_Y)
                    .OrderByDescending(x => x.id)
                    .FirstOrDefault();
                if (thongBaoHistory == null) return new ErrorResult<string>("Không tìm thấy kết quả phản hồi từ cơ quan thuế");

                var xmlFilePath = thongBaoHistory.file_thong_diep_url;
                if (!File.Exists(xmlFilePath)) return new ErrorResult<string>("Không tìm thấy file thông điệp phản hồi");

                var xsltPath = "Template/thongbaosaisot.xslt";
                var xsltArgument = new XsltArgumentList();
                var xmlData = await File.ReadAllTextAsync(xmlFilePath);
                var html = await _serviceWrapper.Xslt.FillDataAsXmlAsync(xsltPath, xmlData, xsltArgument);
                return html.is_success ? new SuccessResult<string>(html.data) : new ErrorResult<string>(html.message);
            }
            catch (Exception ex)
            {
                return new ErrorResult<string>(ex.Message);
            }
        }

        private string GetCompactXmlString(XDocument doc)
        {
            // Sử dụng XmlWriterSettings để tạo XML string dạng compact
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = false, // Không xuống dòng
                OmitXmlDeclaration = false, // bao gồm khai báo XML
                NewLineChars = "", // Không thêm dòng mới
                Encoding = Encoding.UTF8 // Sử dụng UTF-8
            };

            var sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }

            return sb.ToString();
        }

        public async Task<FunctionResult<string>> CreateXmlBienBanFromTbssAsync(int id)
        {
            var user = this.GetCurrentUser();
            var thongBaoSaiSot = await this.SelectByIdAsync(id);
            var thongBaoSaiSotChiTiets = await _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSotChiTiet.SelectByThongBaoIdAsync(id);
            if (thongBaoSaiSot == null) return new ErrorResult<string>("Dữ liệu không hợp lệ");

            var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(thongBaoSaiSot.donvi_ma_dv);
            if (donVi == null) return new ErrorResult<string>("Dữ liệu không hợp lệ");

            var hoaDonId = thongBaoSaiSotChiTiets.Select(x => x.hoa_don_id).FirstOrDefault();
            var hoaDon = await _serviceWrapper.HoaDon.HoaDon.SelectByIdAsync(hoaDonId);
            if (hoaDon == null) return new ErrorResult<string>("Dữ liệu không hợp lệ");
            string kq = "";
            //Tao thong tin XML chung
            string linkelement = "";
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", "");
            doc.AppendChild(docNode);


            var BBan = doc.CreateElement("", "BBan", linkelement);
            doc.AppendChild(BBan);

            var NDBBan = doc.CreateElement("", "NDBBan", linkelement);
            XmlAttribute productAttribute = doc.CreateAttribute("Id");
            productAttribute.Value = "_" + id.ToString();
            NDBBan.Attributes.Append(productAttribute);
            BBan.AppendChild(NDBBan);

            var TTChung = doc.CreateElement("", "TTChung", linkelement);
            NDBBan.AppendChild(TTChung);

            var PBan = doc.CreateElement("", "PBan", linkelement);
            PBan.AppendChild(doc.CreateTextNode("2.1.0"));
            TTChung.AppendChild(PBan);

            var TBBan = doc.CreateElement("", "TBBan", linkelement);
            TBBan.AppendChild(doc.CreateTextNode("BIÊN BẢN ĐIỀU CHỈNH HÓA ĐƠN"));
            TTChung.AppendChild(TBBan);

            var SBBan = doc.CreateElement("", "SBBan", linkelement);
            SBBan.AppendChild(doc.CreateTextNode(thongBaoSaiSot.so_thong_bao));
            TTChung.AppendChild(SBBan);

            var NBBan = doc.CreateElement("", "NBBan", linkelement);
            NBBan.AppendChild(doc.CreateTextNode(thongBaoSaiSot.ngay_thong_bao.ToString("yyyy-MM-dd")));
            TTChung.AppendChild(NBBan);

            var TCHDon = doc.CreateElement("", "TCHDon", linkelement);
            //Số (1: Thay thế, 2: Điều chỉnh)
            if (thongBaoSaiSot.thong_bao_sai_sot_tinh_chat_id == (int)e_thong_bao_sai_sot_tinh_chat.THAY_THE)
                TCHDon.AppendChild(doc.CreateTextNode("1"));
            if (thongBaoSaiSot.thong_bao_sai_sot_tinh_chat_id == (int)e_thong_bao_sai_sot_tinh_chat.DIEU_CHINH)
                TCHDon.AppendChild(doc.CreateTextNode("2"));
            TTChung.AppendChild(TCHDon);

            var NBan = doc.CreateElement("", "NBan", linkelement);
            NBan.AppendChild(doc.CreateTextNode(donVi.ten_dv));
            TTChung.AppendChild(NBan);

            var MSTNBan = doc.CreateElement("", "MSTNBan", linkelement);
            MSTNBan.AppendChild(doc.CreateTextNode(donVi.ma_dv));
            TTChung.AppendChild(MSTNBan);

            var DCNban = doc.CreateElement("", "DCNban", linkelement);
            DCNban.AppendChild(doc.CreateTextNode(donVi.dia_chi));
            TTChung.AppendChild(DCNban);

            var NMua = doc.CreateElement("", "NMua", linkelement);
            NMua.AppendChild(doc.CreateTextNode(hoaDon.nguoi_mua_ten));
            TTChung.AppendChild(NMua);

            var MSTNMua = doc.CreateElement("", "MSTNMua", linkelement);
            MSTNMua.AppendChild(doc.CreateTextNode(hoaDon.nguoi_mua_mst));
            TTChung.AppendChild(MSTNMua);

            var DCNMua = doc.CreateElement("", "DCNMua", linkelement);
            DCNMua.AppendChild(doc.CreateTextNode(hoaDon.nguoi_mua_mst));
            TTChung.AppendChild(DCNMua);

            var KHMSHDon = doc.CreateElement("", "KHMSHDon", linkelement);
            KHMSHDon.AppendChild(doc.CreateTextNode(hoaDon.hoa_don_dang_ky_phat_hanh_mau_so));
            TTChung.AppendChild(KHMSHDon);

            var KHHDon = doc.CreateElement("", "KHHDon", linkelement);
            KHHDon.AppendChild(doc.CreateTextNode(hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu));
            TTChung.AppendChild(KHHDon);

            var SHDon = doc.CreateElement("", "SHDon", linkelement);
            SHDon.AppendChild(doc.CreateTextNode(hoaDon.so_hoa_don.ToString()));
            TTChung.AppendChild(SHDon);

            var NLHDGoc = doc.CreateElement("", "NLHDGoc", linkelement);
            NLHDGoc.AppendChild(doc.CreateTextNode(hoaDon.ngay_hoa_don.ToString("yyyy-MM-dd")));
            TTChung.AppendChild(NLHDGoc);

            var DSLDTDoi = doc.CreateElement("", "DSLDTDoi", linkelement);
            TTChung.AppendChild(DSLDTDoi);

            var LDo = doc.CreateElement("", "LDo", linkelement);
            LDo.AppendChild(doc.CreateTextNode(thongBaoSaiSot.ly_do));
            DSLDTDoi.AppendChild(LDo);
            kq = doc.InnerXml;
            var fileName = Guid.NewGuid().ToString() + ".xml";
            var filePath = $"Xml/{DateTime.Now.Year}/{DateTime.Now.Month}/{fileName}";
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            await File.WriteAllTextAsync(filePath, kq);
            var log = new thong_bao_sai_sot_log()
            {
                file_thong_diep_url = string.Empty,
                ngay_thuc_hien = DateTime.Now,
                nguoi_thuc_hien = user.full_name,
                thong_bao_sai_sot_id = id,
                noi_dung_thuc_hien = "Tạo XML bảng kê",
                thong_bao_sai_sot_log_type_id = (int)e_to_khai_log_type.TAO_XML_BANG_KE
            };
            log.SetInsertInfo(user.id);
            await _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSotLog.InsertAsync(log);
            return new SuccessResult<string>(string.Empty, kq);
        }

        public async Task<FunctionResult<System.Data.DataTable>> ReadAndValidImportDataAsync(UploadRespone upload)
        {
            var excelDatas = await _serviceWrapper.Upload.ReadUploadedExcelFile(new ReadUploadedExcelFileRequest()
            {
                file_path = upload.url,
                sheetIndex = 0
            });
            if (excelDatas == null)
            {
                return new ErrorResult<DataTable>("Không được được nội dung file excel");
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("stt", typeof(int));
            dt.Columns.Add("hoa_don_dang_ky_phat_hanh_mau_so", typeof(string));
            dt.Columns.Add("hoa_don_dang_ky_phat_hanh_ky_hieu", typeof(string));
            dt.Columns.Add("ma_so_hoa_don", typeof(int));
            dt.Columns.Add("ngay_hoa_don", typeof(DateTime));
            dt.Columns.Add("ma_cqt_cap", typeof(string));

            dt.Columns.Add("ma_loi", typeof(string));
            for (int i = 0; i < excelDatas.Rows.Count; i++)
            {
                DataRow data = excelDatas.Rows[i];
                DataRow row = dt.NewRow();
                var maLois = new List<string>();
                row["hoa_don_dang_ky_phat_hanh_mau_so"] = excelDatas.Columns.Contains("mau_so") ? data["mau_so"].ConvertToString() : "";
                row["hoa_don_dang_ky_phat_hanh_ky_hieu"] = excelDatas.Columns.Contains("ky_hieu") ? data["ky_hieu"].ConvertToString() : "";
                row["ma_so_hoa_don"] = excelDatas.Columns.Contains("so_hoa_don") ? data["so_hoa_don"].ConvertToInt() : 0;
                row["ma_cqt_cap"] = excelDatas.Columns.Contains("ma_cqt_cap") ? data["ma_cqt_cap"].ConvertToString() : "";
                row["ngay_hoa_don"] = excelDatas.Columns.Contains("ngay_hoa_don") ? data["ngay_hoa_don"].ConvertToString().MapDateFromddMMYYY() : null;



                if (row["hoa_don_dang_ky_phat_hanh_mau_so"].ConvertToString() == "") maLois.Add("Tên hàng hóa không được để trống");
                if (row["hoa_don_dang_ky_phat_hanh_ky_hieu"].ConvertToString() == "") maLois.Add("Ký hiệu không được để trống");
                if (row["ma_so_hoa_don"].ConvertToInt() <= 0) maLois.Add("Số hóa đơn không hợp lệ");
                if (row["ngay_hoa_don"] == null) maLois.Add("Ngày hóa đơn không hợp lệ");
                row["ma_loi"] = maLois.Join(";\n");
                dt.Rows.Add(row);
            }
            return new SuccessResult<DataTable>(dt);
        }
    }
}
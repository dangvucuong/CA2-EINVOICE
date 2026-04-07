using System.Xml;
using Common;
using Contracts.Service.BangTongHop;
using Model.Base;
using Model.Enum;
using Model.Request.BangTongHop;
using Model.Respone.Xml;
using Model.Table;
using Service.Base;

namespace Service.BangTongHop
{
    public class BangTongHopService : CRUDService<bang_tong_hop_du_lieu>, IBangTongHopService
    {
        public BangTongHopService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.BangTongHopDuLieu.BangTongHop;
        }

        public async Task<string> CreateXmlKySoAsync(int id)
        {
            var user = this.GetCurrentUser();
            var donVi = await this.GetCurrentDonViAsync();
            var bangTongHop = await this.SelectByIdAsync(id);
            var hoaDonIds = (await _serviceWrapper.BangTongHopDuLieu.BangTongHopHoaDon.SelectByBangTongHopAsync(id)).Select(x => x.hoa_don_id).ToList();
            var hoaDons = await _serviceWrapper.HoaDon.HoaDon.SelectByIdsAsync(hoaDonIds);
            var MSo = "01/TH-HĐĐT";
            var Ten = "Bảng tổng hợp dữ liệu hóa đơn điện tử";
            var SBTHDLieu = bangTongHop.so_thu_tu_lan_bo_sung.ToString();
            var LKDLieu = bangTongHop.bang_tong_hop_du_lieu_type.ToString();
            var KDLieu = bangTongHop.ky_du_lieu;
            var LDau = bangTongHop.is_lan_dau ? "1" : "0";
            var BSLThu = bangTongHop.so_thu_tu_lan_bo_sung.ToString();
            var NLap = DateTime.Now.ToString("yyyy-MM-dd");
            var MST = user.donvi_ma_dv;
            var TNNT = donVi?.ten_dv ?? "";
            var HDDIn = "0";
            var LHHoa = "";
            // bangTongHop.loai_hang_hoa_dich_vu_type.ConvertToString();
            //              Dim kydulieu As String = String.Empty
            //  If loai = "N" Then
            //      kydulieu = txtNgaytinhthue.Date.ToString("dd/MM/yyyy")
            //  ElseIf loai = "T" Then
            //      kydulieu = cmbthangtinhthue.Value & "/" & cmbNamtinhthue.Text
            //  ElseIf loai = "Q" Then
            //      kydulieu = cmbQuy.Value & "/" & cmbNamtinhthue.Text
            //  End If

            string kq = "";
            string linkelement = "";
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", "");
            doc.AppendChild(docNode);
            XmlElement HDonNode = doc.CreateElement("", "BTHDLieu", linkelement);
            doc.AppendChild(HDonNode);

            string sGUID = Guid.NewGuid().ToString();

            XmlNode DLHDonNode = doc.CreateElement("", "DLBTHop", linkelement);
            XmlAttribute productAttribute = doc.CreateAttribute("Id");
            productAttribute.Value = "_" + sGUID;
            DLHDonNode.Attributes.Append(productAttribute);
            HDonNode.AppendChild(DLHDonNode);

            XmlNode TTChungNode = doc.CreateElement("", "TTChung", linkelement);
            DLHDonNode.AppendChild(TTChungNode);

            AddElement(TTChungNode, "PBan", "2.1.0", linkelement);
            AddElement(TTChungNode, "MSo", MSo, linkelement);
            AddElement(TTChungNode, "Ten", Ten, linkelement);
            AddElement(TTChungNode, "SBTHDLieu", SBTHDLieu, linkelement);
            AddElement(TTChungNode, "LKDLieu", LKDLieu, linkelement);
            AddElement(TTChungNode, "KDLieu", KDLieu, linkelement);
            AddElement(TTChungNode, "LDau", LDau, linkelement);

            if (LDau == "0")
            {
                AddElement(TTChungNode, "BSLThu", BSLThu, linkelement);
            }

            AddElement(TTChungNode, "NLap", NLap, linkelement);
            AddElement(TTChungNode, "TNNT", TNNT, linkelement);
            AddElement(TTChungNode, "MST", MST, linkelement);
            AddElement(TTChungNode, "HDDIn", HDDIn, linkelement);
            AddElement(TTChungNode, "LHHoa", LHHoa, linkelement);

            XmlNode NDBTHDLieuNode = doc.CreateElement("", "NDBTHDLieu", linkelement);
            DLHDonNode.AppendChild(NDBTHDLieuNode);
            XmlNode DSDLieuNode = doc.CreateElement("", "DSDLieu", linkelement);
            NDBTHDLieuNode.AppendChild(DSDLieuNode);
            var stt = 0;
            foreach (var item in hoaDons)
            {
                stt += 1;
                XmlNode DLieuNode = doc.CreateElement("", "DLieu", linkelement);
                DSDLieuNode.AppendChild(DLieuNode);

                AddElementIfNotEmpty(DLieuNode, "STT", stt.ToString(), linkelement);
                AddElement(DLieuNode, "KHMSHDon", item.hoa_don_dang_ky_phat_hanh_mau_so, linkelement);
                AddElement(DLieuNode, "KHHDon", item.hoa_don_dang_ky_phat_hanh_ky_hieu, linkelement);
                AddElement(DLieuNode, "SHDon", item.ma_so_hoa_don.ToString(), linkelement);
                AddElement(DLieuNode, "NLap", item.ngay_hoa_don.ToString("yyyy-MM-dd"), linkelement);
                AddElement(DLieuNode, "TNMua", item.nguoi_mua_ten_donvi, linkelement);
                AddElement(DLieuNode, "MSTNMua", item.nguoi_mua_mst, linkelement);

                // AddElementIfNotEmpty(DLieuNode, "MKHang", item.MKHang, linkelement);
                // AddElementIfNotEmpty(DLieuNode, "MHHDVu", item.MHHDVu, linkelement);
                // AddElementIfNotEmpty(DLieuNode, "THHDVu", item.THHDVu, linkelement);
                // AddElementIfNotEmpty(DLieuNode, "DVTinh", item.DVTinh, linkelement);
                // AddElementIfNotEmpty(DLieuNode, "SLuong", item.SLuong, linkelement);

                // AddElement(DLieuNode, "TTCThue", item.TTCThue, linkelement);
                // AddElement(DLieuNode, "TSuat", item.TSuat, linkelement);
                // AddElement(DLieuNode, "TgTThue", item.TgTThue, linkelement);
                // AddElement(DLieuNode, "TgTTToan", item.TgTTToan, linkelement);
                // AddElement(DLieuNode, "TThai", item.TThai, linkelement);
                // AddElementIfNotEmpty(DLieuNode, "LHDCLQuan", item.LHDCLQuan, linkelement);
                // AddElementIfNotEmpty(DLieuNode, "KHMSHDCLQuan", item.KHMSHDCLQuan, linkelement);
                // AddElementIfNotEmpty(DLieuNode, "KHHDCLQuan", item.KHHDCLQuan, linkelement);
                // AddElementIfNotEmpty(DLieuNode, "SHDCLQuan", item.SHDCLQuan, linkelement);
                // AddElementIfNotEmpty(DLieuNode, "LKDLDChinh", item.LKDLDChinh, linkelement);
                // AddElementIfNotEmpty(DLieuNode, "KDLDChinh", item.KDLDChinh, linkelement);
                // AddElementIfNotEmpty(DLieuNode, "STBao", item.STBao, linkelement);
                // AddElementIfNotEmpty(DLieuNode, "NTBao", item.NTBao, linkelement);
                // AddElementIfNotEmpty(DLieuNode, "GChu", item.GChu, linkelement);
            }

            XmlNode DSCKSNode = doc.CreateElement("", "DSCKS", linkelement);
            HDonNode.AppendChild(DSCKSNode);
            XmlNode NNTCKSNode = doc.CreateElement("", "NNT", linkelement);
            DSCKSNode.AppendChild(NNTCKSNode);
            XmlNode CCKSKhacCKSNode = doc.CreateElement("", "CCKSKhac", linkelement);
            DSCKSNode.AppendChild(CCKSKhacCKSNode);

            kq = doc.InnerXml;
            return kq;
        }
        private void AddElement(XmlNode parent, string name, string value, string linkelement)
        {
            XmlNode node = parent.OwnerDocument.CreateElement("", name, linkelement);
            node.AppendChild(parent.OwnerDocument.CreateTextNode(value));
            parent.AppendChild(node);
        }

        private void AddElementIfNotEmpty(XmlNode parent, string name, string value, string linkelement)
        {
            if (!string.IsNullOrEmpty(value))
            {
                AddElement(parent, name, value, linkelement);
            }
        }

        public Task<string> CreateXmlThongDiepAsync(int id, string signedText)
        {
            throw new NotImplementedException();
        }

        public Task<FunctionResult<bool>> PhatHanhAsync(int id, string signedText)
        {
            throw new NotImplementedException();
        }

        public async Task<FunctionResult<bang_tong_hop_du_lieu>> SaveChangesAsync(BangTongHopAddOrEditRequest request)
        {
            var user = this.GetCurrentUser();
            var user_id = user.id;
            var obj = request.Map<bang_tong_hop_du_lieu>();
            if (obj.bang_tong_hop_du_lieu_type == "ngay")
            {
                obj.ky_du_lieu = obj.ngay.ToString();
            }
            if (obj.bang_tong_hop_du_lieu_type == "thang")
            {
                obj.ky_du_lieu = $"{obj.thang.ToString()}/{obj.nam.ToString()}";
            }
            if (obj.bang_tong_hop_du_lieu_type == "quy")
            {
                obj.ky_du_lieu = $"{obj.quy.ToString()}/{obj.nam.ToString()}";
            }
            if (request.id <= 0)
            {

                obj.SetInsertInfo(user_id);
                obj.donvi_ma_dv = user.donvi_ma_dv;
                obj.bang_tong_hop_du_lieu_trang_thai_id = (int)e_thong_bao_sai_sot_trang_thai.TAO_MOI;
                obj.id = await this.InsertAsync(obj);

            }
            else
            {
                var dbObj = await this.SelectByIdAsync(request.id);
                if (dbObj == null || dbObj.bang_tong_hop_du_lieu_trang_thai_id != (int)e_thong_bao_sai_sot_trang_thai.TAO_MOI)
                {
                    return new ErrorResult<bang_tong_hop_du_lieu>("Không tìm thấy dữ liệu hợp lệ");
                }
                dbObj.bang_tong_hop_du_lieu_loai_hang_hoa_id = obj.bang_tong_hop_du_lieu_loai_hang_hoa_id;
                dbObj.bang_tong_hop_du_lieu_type = obj.bang_tong_hop_du_lieu_type;
                dbObj.is_lan_dau = obj.is_lan_dau;
                dbObj.ky_du_lieu = obj.ky_du_lieu;
                dbObj.ngay = obj.ngay;
                dbObj.thang = obj.thang;
                dbObj.quy = obj.quy;
                dbObj.nam = obj.nam;
                dbObj.so_thu_tu_lan_bo_sung = obj.so_thu_tu_lan_bo_sung;
                dbObj.so_luong_hoa_don = request.so_luong_hoa_don;
                dbObj.SetUpdateInfo(user_id);
                await this.UpdateAsync(dbObj);
            }
            if (request.id <= 0)
            {
                if (obj.id > 0)
                {
                    var duLieuHoaDons = new List<bang_tong_hop_du_lieu_hoa_don>();
                    foreach (var hoa_don_id in request.hoa_don_ids)
                    {
                        var item = new bang_tong_hop_du_lieu_hoa_don()
                        {
                            bang_tong_hop_du_lieu_id = obj.id,
                            hoa_don_id = hoa_don_id
                        };
                        item.SetInsertInfo(user_id);
                        duLieuHoaDons.Add(item);
                    }
                    await _serviceWrapper.BangTongHopDuLieu.BangTongHopHoaDon.InsertsAsync(duLieuHoaDons);
                    var log = new bang_tong_hop_du_lieu_log()
                    {
                        file_thong_diep_url = string.Empty,
                        ngay_thuc_hien = DateTime.Now,
                        nguoi_thuc_hien = user.full_name,
                        bang_tong_hop_du_lieu_id = obj.id,
                        noi_dung_thuc_hien = "Thêm mới",
                        bang_tong_hop_du_lieu_log_type_id = (int)e_to_khai_log_type.TAO_MOI
                    };
                    log.SetInsertInfo(user_id);
                    await _serviceWrapper.BangTongHopDuLieu.BangTongHopLog.InsertAsync(log);
                    return new SuccessResult<bang_tong_hop_du_lieu>(obj);
                }
            }
            else
            {
                var bangTongHopHoaDonsDb = await _serviceWrapper.BangTongHopDuLieu.BangTongHopHoaDon.SelectByBangTongHopAsync(obj.id);
                var hoaDonIdsDb = bangTongHopHoaDonsDb.Select(x => x.hoa_don_id).ToList();
                var hoaDonIdsNew = request.hoa_don_ids;

                var hoaDonIdsDelete = hoaDonIdsDb.Where(x => !hoaDonIdsNew.Contains(x)).ToList();
                var bangTongHopHoaDonsDeleteIds = bangTongHopHoaDonsDb.Where(x => hoaDonIdsDelete.Contains(x.hoa_don_id)).Select(x => x.id).ToList();

                await _serviceWrapper.BangTongHopDuLieu.BangTongHopHoaDon.DeletesAsync(bangTongHopHoaDonsDeleteIds, user_id);

                var hoaDonIdsInsert = hoaDonIdsNew.Where(x => !hoaDonIdsDb.Contains(x)).ToList();
                var duLieuHoaDons = new List<bang_tong_hop_du_lieu_hoa_don>();
                foreach (var hoa_don_id in hoaDonIdsInsert)
                {
                    var item = new bang_tong_hop_du_lieu_hoa_don()
                    {
                        bang_tong_hop_du_lieu_id = obj.id,
                        hoa_don_id = hoa_don_id
                    };
                    item.SetInsertInfo(user_id);
                    duLieuHoaDons.Add(item);
                }
                await _serviceWrapper.BangTongHopDuLieu.BangTongHopHoaDon.InsertsAsync(duLieuHoaDons);
                var log = new bang_tong_hop_du_lieu_log()
                {
                    file_thong_diep_url = string.Empty,
                    ngay_thuc_hien = DateTime.Now,
                    nguoi_thuc_hien = user.full_name,
                    bang_tong_hop_du_lieu_id = obj.id,
                    noi_dung_thuc_hien = "Cập nhật",
                    bang_tong_hop_du_lieu_log_type_id = (int)e_to_khai_log_type.TAO_MOI
                };
                log.SetInsertInfo(user_id);
                await _serviceWrapper.BangTongHopDuLieu.BangTongHopLog.InsertAsync(log);
                return new SuccessResult<bang_tong_hop_du_lieu>(obj);

            }


            return new ErrorResult<bang_tong_hop_du_lieu>("");
        }

        public Task<IEnumerable<bang_tong_hop_du_lieu>> SelectByDonViAsync(string donvi_ma_dv)
        {
            return _repositoryWrapper.BangTongHopDuLieu.BangTongHop.SelectByDonViAsync(donvi_ma_dv);
        }

        public Task<FunctionResult<bool>> XuLyThongDiepAsync(bang_tong_hop_du_lieu thongBaoSaiSot, KetQuaThongDiepRespone ketQuaThongDiepRespone, string xmlThongDiep)
        {
            throw new NotImplementedException();
        }
    }
}
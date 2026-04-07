using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model.Base;

namespace Model.Table
{
    public class rs_yeu_cau_ky : modify_infor
    {
        public int id { get; set; }
        public string code { get; set; }
        public string user_id { get; set; }
        public string type { get; set; }
        public string type_key { get; set; }
        public string ket_qua_ky { get; set; }
    }
}
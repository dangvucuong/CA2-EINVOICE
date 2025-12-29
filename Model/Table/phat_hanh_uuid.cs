using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model.Base;

namespace Model.Table
{
    public class phat_hanh_uuid : modify_infor
    {
        public int id { get; set; }
        public string uuid { get; set; }
        public string type_name { get; set; }
    }
}
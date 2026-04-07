namespace Model.RemoteSigning
{
    public abstract class BaseRequest
    {
        public string Serial { get; set; }
        public string Masothue { get; set; }
        public abstract string Cleartext { get; set; }
        public abstract string Mahethong { get; set; }
        public abstract string Keyhethong { get; set; }
        public string Linkfile_goc
        {
            get
            {
                return "https://ca2einv.nacencomm.vn/";
            }
        }
        public string Email { get; set; }
        public abstract string Type { get; set; }

    }
}
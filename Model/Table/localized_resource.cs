namespace Model.Table
{
    public class localized_resource
    {
        public int id { get; set; }
        public string scope { get; set; }
        public string code { get; set; }
        public string language { get; set; }
        public string value { get; set; }
        public string unique_key
        {
            get
            {
                return $"{scope}-{code}_{language}";
            }
        }
    }
}
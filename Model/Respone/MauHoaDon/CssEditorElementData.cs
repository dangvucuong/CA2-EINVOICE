namespace Model.Respone.MauHoaDon
{
    public class CssEditorElementData
    {
        public string elementId { get; set; }
        public string elementText { get; set; }
        public bool isDisplay { get; set; }
        public string type { get; set; }
        public CssEditorValue cssValue { get; set; }



    }
    public class CssEditorValue
    {
        public int fontSize { get; set; }
        public string color { get; set; }
        public string align { get; set; }
        public bool isBold { get; set; }
        public bool isItalic { get; set; }


    }
}
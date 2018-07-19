namespace ATP.Common.Entities
{
    public class SelectItem
    {
        #region CONSTRUCTOR

        public SelectItem()
        {
        }

        public SelectItem(string value, string text)
        {
            Value = value;
            Text = text;
        }

        public SelectItem(string value, string text, bool selected)
        {
            Value = value;
            Text = text;
            Selected = selected;
        }

        #endregion CONSTRUCTOR

        public string Value { get; set; }

        public string Text { get; set; }

        public bool Selected { get; set; }
    }
}

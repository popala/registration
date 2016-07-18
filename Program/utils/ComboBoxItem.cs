
namespace Rejestracja
{
    class ComboBoxItem
    {
        public string text { get; set; }
        public long id { get; set; }

        public ComboBoxItem()
        {
        }

        public ComboBoxItem(long id, string text)
        {
            this.text = text;
            this.id = id;
        }

        public override string ToString()
        {
            return this.text;
        }
    }
}


namespace Rejestracja.Utils
{
    class ComboBoxItem
    {
        public string text { get; set; }
        public int id { get; set; }

        public ComboBoxItem()
        {
        }

        public ComboBoxItem(int id, string text)
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

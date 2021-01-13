namespace AvaloniaApplication1.ViewModels
{
    public class Item
    {
        private readonly object[] _values =
        {
            10,
            "Text"
        };

        public object this[int index]
        {
            get => _values[index];
            set => _values[index] = value;
        }
    }
}

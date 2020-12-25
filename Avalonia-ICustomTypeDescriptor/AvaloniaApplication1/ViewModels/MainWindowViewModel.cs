using System.Collections.ObjectModel;

namespace AvaloniaApplication1.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>()
        {
            new Item()
            {
                Name = "Alice",
                Age = 23
            },
            new Item()
            {
                Name = "Bob",
                Age = 44
            }
        };
    }
}

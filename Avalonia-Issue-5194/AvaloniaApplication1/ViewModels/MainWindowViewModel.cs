using System.Collections.ObjectModel;

namespace AvaloniaApplication1.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>()
        {
            new Item()
        };
    }
}

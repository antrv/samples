using System.Collections.ObjectModel;

namespace AvaloniaApplication1.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Person> Items { get; } = new ObservableCollection<Person>()
        {
            new Person()
            {
                Name = "Alice",
                Age = 23
            },
            new Person()
            {
                Name = "Bob",
                Age = 44
            }
        };
    }
}

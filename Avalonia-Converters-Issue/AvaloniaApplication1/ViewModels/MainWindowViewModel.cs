using System;
using ReactiveUI;

namespace AvaloniaApplication1.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private int _integer = 20;
        private double _double = 1.345;
        private Guid _guid = Guid.NewGuid();

        public int Integer
        {
            get => _integer;
            set => this.RaiseAndSetIfChanged(ref _integer, value);
        }

        public double Double
        {
            get => _double;
            set => this.RaiseAndSetIfChanged(ref _double, value);
        }

        public Guid Guid
        {
            get => _guid;
            set => this.RaiseAndSetIfChanged(ref _guid, value);
        }
    }
}

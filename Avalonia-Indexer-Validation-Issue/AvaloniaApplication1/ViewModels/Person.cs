using System.Linq;
using Avalonia.Data;
using ReactiveUI;

namespace AvaloniaApplication1.ViewModels
{
    public class AdditionalData : ReactiveObject
    {
        private readonly string[] _data =
            Enumerable.Range(0, 10)
                .Select(n => $"Additional {n}").ToArray();

        public string this[int index]
        {
            get => _data[index];
            set
            {
                if (_data[index] != value)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        throw new DataValidationException("Additional value cannot be null or empty");
                    }

                    this.RaiseAndSetIfChanged(ref _data[index], value, "Item[]");
                }
            }
        }
    }

    public class Person : ReactiveObject
    {
        private readonly AdditionalData _additionalData = new AdditionalData();
        private string _name;
        private int _age;

        public AdditionalData AdditionalData => _additionalData;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        throw new DataValidationException("Name cannot be null or empty");
                    }

                    this.RaiseAndSetIfChanged(ref _name, value);
                }
            }
        }

        public int Age
        {
            get => _age;
            set => this.RaiseAndSetIfChanged(ref _age, value);
        }
    }
}

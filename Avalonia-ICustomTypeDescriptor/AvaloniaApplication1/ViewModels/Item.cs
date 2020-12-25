using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Data;
using JetBrains.Annotations;
using ReactiveUI;

namespace AvaloniaApplication1.ViewModels
{
    public class Item : ReactiveObject, ICustomTypeDescriptor
    {
        private readonly Dictionary<string, string> _dynamicProperties = new Dictionary<string, string>()
        {
            ["Property1"] = "Value1",
            ["Property2"] = "Value2"
        };

        private string _name;
        private int _age;

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

        AttributeCollection ICustomTypeDescriptor.GetAttributes() => AttributeCollection.Empty;
        string ICustomTypeDescriptor.GetClassName() => nameof(Item);
        string ICustomTypeDescriptor.GetComponentName() => nameof(Item);
        TypeConverter ICustomTypeDescriptor.GetConverter() => null;
        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent() => null;
        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty() => null;
        object ICustomTypeDescriptor.GetEditor(Type editorBaseType) => null;
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents() => EventDescriptorCollection.Empty;
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes) =>
            EventDescriptorCollection.Empty;
        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties() => _properties;
        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes) => _properties;
        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd) => null;

        private static readonly PropertyDescriptorCollection _properties;

        static Item()
        {
            _properties = new PropertyDescriptorCollection(new PropertyDescriptor[]
            {
                new ItemPropertyDescriptor("Property1"),
                new ItemPropertyDescriptor("Property2"),
            });
        }

        private sealed class ItemPropertyDescriptor : PropertyDescriptor
        {
            public ItemPropertyDescriptor([NotNull] string name)
                : base(name, Array.Empty<Attribute>())
            {
            }

            public override bool CanResetValue(object component) => true;

            public override object GetValue(object component)
            {
                ((Item)component)._dynamicProperties.TryGetValue(Name, out string str);
                return str;
            }

            public override void ResetValue(object component)
            {
                ((Item)component)._dynamicProperties.Remove(Name);
            }

            public override void SetValue(object component, object value)
            {
                Item item = (Item)component;
                item._dynamicProperties.TryGetValue(Name, out string currentValue);
                string newValue = value?.ToString();
                if (currentValue != newValue)
                {
                    item._dynamicProperties[Name] = newValue;
                    item.RaisePropertyChanged(Name);
                }
            }

            public override bool ShouldSerializeValue(object component) => false;
            public override Type ComponentType => typeof(Item);
            public override bool IsReadOnly => false;
            public override Type PropertyType => typeof(string);
        }
    }
}

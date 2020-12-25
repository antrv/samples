using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Utilities;
using JetBrains.Annotations;

namespace AvaloniaApplication1
{
    /// <summary>
    /// Reads a property from a standard C# object that optionally supports the
    /// <see cref="INotifyPropertyChanged"/> interface.
    /// </summary>
    public sealed class CustomPropertyAccessorPlugin: IPropertyAccessorPlugin
    {
        public static void Init()
        {
            ExpressionObserver.PropertyAccessors.Insert(2, new CustomPropertyAccessorPlugin());
        }

        /// <inheritdoc/>
        public bool Match(object obj, string propertyName) => obj is ICustomTypeDescriptor customTypeDescriptor &&
            customTypeDescriptor.GetProperties().Find(propertyName, false) != null;

        /// <summary>
        /// Starts monitoring the value of a property on an object.
        /// </summary>
        /// <param name="reference">The object.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>
        /// An <see cref="IPropertyAccessor"/> interface through which future interactions with the
        /// property will be made.
        /// </returns>
        public IPropertyAccessor Start([NotNull] WeakReference<object> reference, [NotNull] string propertyName)
        {
            if (reference == null) throw new ArgumentNullException(nameof(reference));
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            reference.TryGetTarget(out object instance);

            PropertyDescriptor propertyDescriptor = null;
            if (instance is ICustomTypeDescriptor customTypeDescriptor)
                propertyDescriptor = customTypeDescriptor.GetProperties().Find(propertyName, false);

            if (propertyDescriptor != null)
                return new Accessor(reference, propertyDescriptor);

            if (instance != AvaloniaProperty.UnsetValue)
            {
                string message = $"Could not find custom property '{propertyName}' on '{instance}'";
                MissingMemberException exception = new MissingMemberException(message);
                return new PropertyError(new BindingNotification(exception, BindingErrorType.Error));
            }

            return null;
        }

        private class Accessor: PropertyAccessorBase, IWeakSubscriber<PropertyChangedEventArgs>
        {
            private readonly WeakReference<object> _reference;
            private readonly PropertyDescriptor _property;
            private bool _eventRaised;

            public Accessor([NotNull] WeakReference<object> reference, [NotNull] PropertyDescriptor property)
            {
                _reference = reference;
                _property = property;
            }

            public override Type PropertyType => _property.PropertyType;

            public override object Value
            {
                get
                {
                    object o = GetReferenceTarget();
                    return o is null ? null : _property.GetValue(o);
                }
            }

            public override bool SetValue(object value, BindingPriority priority)
            {
                if (!_property.IsReadOnly)
                {
                    _eventRaised = false;

                    try
                    {
                        _property.SetValue(GetReferenceTarget(), value);
                    }
                    catch (Exception exception)
                    {
                        throw new DataValidationException(exception.Message);
                    }

                    if (!_eventRaised)
                        SendCurrentValue();

                    return true;
                }

                return false;
            }

            void IWeakSubscriber<PropertyChangedEventArgs>.OnEvent(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == _property.Name || string.IsNullOrEmpty(e.PropertyName))
                {
                    _eventRaised = true;
                    SendCurrentValue();
                }
            }

            protected override void SubscribeCore()
            {
                SubscribeToChanges();
                SendCurrentValue();
            }

            protected override void UnsubscribeCore()
            {
                // TODO: PropertyDescriptor change event support
                // https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.propertydescriptor.supportschangeevents
                // https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.propertydescriptor.addvaluechanged
                // https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.propertydescriptor.removevaluechanged
                object instance = GetReferenceTarget();
                if (instance is INotifyPropertyChanged npc)
                    WeakSubscriptionManager.Unsubscribe(npc, nameof(npc.PropertyChanged), this);
            }

            private object GetReferenceTarget()
            {
                _reference.TryGetTarget(out object target);
                return target;
            }

            private void SendCurrentValue()
            {
                try
                {
                    object value = Value;
                    PublishValue(value);
                }
                catch
                {
                }
            }

            private void SubscribeToChanges()
            {
                // TODO: PropertyDescriptor change event support
                object instance = GetReferenceTarget();
                if (instance is INotifyPropertyChanged npc)
                    WeakSubscriptionManager.Subscribe(npc, nameof(npc.PropertyChanged), this);
            }
        }
    }
}

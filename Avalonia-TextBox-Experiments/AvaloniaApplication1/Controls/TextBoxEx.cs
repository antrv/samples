using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;

namespace AvaloniaApplication1.Controls
{
    [PseudoClasses(":empty")]
    public class TextBoxEx : TextBox
    {
        private readonly Action _snapshot;
        private readonly Action<string> _handleTextInput;

        public TextBoxEx()
        {
            Text = "0";
            AddHandler(TextInputEvent, OnTextInput, RoutingStrategies.Tunnel);
            AddHandler(KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel);
            TextProperty.Changed.Subscribe(OnTextChanged);

            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            Type textBoxType = typeof(TextBox);
            object undoRedoHelperRef = textBoxType.GetField("_undoRedoHelper", bindingFlags).GetValue(this);
            MethodInfo snapshotMethod = undoRedoHelperRef.GetType().GetMethod("Snapshot");
            _snapshot = (Action)Delegate.CreateDelegate(typeof(Action), undoRedoHelperRef, snapshotMethod);
            MethodInfo handleTextInputMethod = textBoxType.GetMethod("HandleTextInput", bindingFlags);
            _handleTextInput = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), this,
                handleTextInputMethod);
        }

        private void OnTextChanged(AvaloniaPropertyChangedEventArgs<string> args)
        {
            string value = args.NewValue.Value;
            if (string.IsNullOrEmpty(value))
                Text = "0";
        }

        private async void OnKeyDown(object sender, KeyEventArgs args)
        {
            bool Match(List<KeyGesture> gestures) => gestures.Any(g => g.Matches(args));
            PlatformHotkeyConfiguration keymap = AvaloniaLocator.Current.GetService<PlatformHotkeyConfiguration>();

            if (Match(keymap.Paste))
            {
                string text =
                    FilterText(
                        await ((IClipboard)AvaloniaLocator.Current.GetService(typeof(IClipboard))).GetTextAsync());

                if (string.IsNullOrEmpty(text))
                {
                    args.Handled = true;
                    return;
                }

                _snapshot();
                _handleTextInput(text);
                _snapshot();
                args.Handled = true;
            }
        }

        private void OnTextInput(object sender, TextInputEventArgs args)
        {
            args.Text = FilterText(args.Text);
        }

        private static string FilterText(string text)
        {
            if (text is null)
                return string.Empty;

            string result = string.Empty;
            foreach (char c in text)
            {
                if (c >= '0' && c <= '9')
                    result += c;
            }

            return result;
        }
    }
}

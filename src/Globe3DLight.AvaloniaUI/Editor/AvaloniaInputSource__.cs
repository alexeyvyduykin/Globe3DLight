using System;
using System.Linq;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Globe3DLight.Input;

namespace Globe3DLight.AvaloniaUI.Editor
{
    public class AvaloniaInputSource__ : InputSource
    {
        private static ModifierFlags ToModifierFlags(KeyModifiers inputModifiers)
        {
            var modifier = ModifierFlags.None;

            if (inputModifiers.HasFlag(KeyModifiers.Alt))
            {
                modifier |= ModifierFlags.Alt;
            }

            if (inputModifiers.HasFlag(KeyModifiers.Control))
            {
                modifier |= ModifierFlags.Control;
            }

            if (inputModifiers.HasFlag(KeyModifiers.Shift))
            {
                modifier |= ModifierFlags.Shift;
            }

            return modifier;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvaloniaInputSource"/> class.
        /// </summary>
        /// <param name="source">The source element.</param>
        /// <param name="relative">The relative element.</param>
        /// <param name="translate">The translate function.</param>
        public AvaloniaInputSource__(Control source)
        {
            LeftDown = GetPointerPressedObservable(source, MouseButton.Left);
            LeftUp = GetPointerReleasedObservable(source, MouseButton.Left);
            RightDown = GetPointerPressedObservable(source, MouseButton.Right);
            RightUp = GetPointerReleasedObservable(source, MouseButton.Right);
            Move = GetPointerMovedObservable(source);
        }

        private static bool IsMouseButton(Control target, PointerPressedEventArgs e, MouseButton button)
        {
            var properties = e.GetCurrentPoint(target).Properties;
            if ((properties.IsLeftButtonPressed && button == MouseButton.Left)
                || (properties.IsRightButtonPressed && button == MouseButton.Right)
                || (properties.IsMiddleButtonPressed && button == MouseButton.Middle))
            {
                return true;
            }
            return false;
        }

        private static IObservable<InputArgs> GetPointerPressedObservable(Control target, MouseButton button)
        {
            return Observable.FromEventPattern<EventHandler<PointerPressedEventArgs>, PointerPressedEventArgs>(
                handler => target.PointerPressed += handler,
                handler => target.PointerPressed -= handler)
                .Where(e => IsMouseButton(target, e.EventArgs, button)).Select(
                e =>
                {
                    var point = e.EventArgs.GetPosition(target);
                    return new InputArgs(point.X, point.Y, ToModifierFlags(e.EventArgs.KeyModifiers));
                });
        }

        private static IObservable<InputArgs> GetPointerReleasedObservable(Control target, MouseButton button)
        {
            return Observable.FromEventPattern<EventHandler<PointerReleasedEventArgs>, PointerReleasedEventArgs>(
                handler => target.PointerReleased += handler,
                handler => target.PointerReleased -= handler)
                .Where(e => e.EventArgs.InitialPressMouseButton == button).Select(
                e =>
                {
                    var point = e.EventArgs.GetPosition(target);
                    return new InputArgs(point.X, point.Y, ToModifierFlags(e.EventArgs.KeyModifiers));
                });
        }

        private static IObservable<InputArgs> GetPointerMovedObservable(Control target)
        {
            return Observable.FromEventPattern<EventHandler<PointerEventArgs>, PointerEventArgs>(
                handler => target.PointerMoved += handler,
                handler => target.PointerMoved -= handler)
                .Select(
                e =>
                {
                    var point = e.EventArgs.GetPosition(target);
                    return new InputArgs(point.X, point.Y, ToModifierFlags(e.EventArgs.KeyModifiers));
                });
        }
    }
}

﻿using System;
using System.Linq;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Globe3DLight.Input;

namespace Globe3DLight.Editor
{
    public class AvaloniaInputSource : InputSource
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
        public AvaloniaInputSource(Control source, Control relative, Func<Point, Point> translate)
        {
            LeftDown = GetPointerPressedObservable(source, relative, translate, MouseButton.Left);
            LeftUp = GetPointerReleasedObservable(source, relative, translate, MouseButton.Left);
            RightDown = GetPointerPressedObservable(source, relative, translate, MouseButton.Right);
            RightUp = GetPointerReleasedObservable(source, relative, translate, MouseButton.Right);
            Move = GetPointerMovedObservable(source, relative, translate);
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

        private static IObservable<InputArgs> GetPointerPressedObservable(Control target, Control relative, Func<Point, Point> translate, MouseButton button)
        {
            return Observable.FromEventPattern<EventHandler<PointerPressedEventArgs>, PointerPressedEventArgs>(
                handler => target.PointerPressed += handler,
                handler => target.PointerPressed -= handler)
                .Where(e => IsMouseButton(target, e.EventArgs, button)).Select(
                e =>
                {
                    var point = translate(e.EventArgs.GetPosition(relative));
                    return new InputArgs(point.X, point.Y, ToModifierFlags(e.EventArgs.KeyModifiers));
                });
        }

        private static IObservable<InputArgs> GetPointerReleasedObservable(Control target, Control relative, Func<Point, Point> translate, MouseButton button)
        {
            return Observable.FromEventPattern<EventHandler<PointerReleasedEventArgs>, PointerReleasedEventArgs>(
                handler => target.PointerReleased += handler,
                handler => target.PointerReleased -= handler)
                .Where(e => e.EventArgs.InitialPressMouseButton == button).Select(
                e =>
                {
                    var point = translate(e.EventArgs.GetPosition(relative));
                    return new InputArgs(point.X, point.Y, ToModifierFlags(e.EventArgs.KeyModifiers));
                });
        }

        private static IObservable<InputArgs> GetPointerMovedObservable(Control target, Control relative, Func<Point, Point> translate)
        {
            return Observable.FromEventPattern<EventHandler<PointerEventArgs>, PointerEventArgs>(
                handler => target.PointerMoved += handler,
                handler => target.PointerMoved -= handler)
                .Select(
                e =>
                {
                    var point = translate(e.EventArgs.GetPosition(relative));
                    return new InputArgs(point.X, point.Y, ToModifierFlags(e.EventArgs.KeyModifiers));
                });
        }
    }
}

using System;
using TimeDataViewer.Spatial;

namespace TimeDataViewer.Core
{
    public class InputEventArgs : EventArgs
    {
        public bool Handled { get; set; }

        public ScreenPoint Position { get; set; }
    }

    public partial class PlotModel
    {
        public event EventHandler<InputEventArgs> MouseDown;

        public event EventHandler<InputEventArgs> MouseMove;

        public event EventHandler<InputEventArgs> MouseUp;

        public event EventHandler<InputEventArgs> MouseEnter;

        public event EventHandler<InputEventArgs> MouseLeave;

        public virtual void HandleMouseDown(object sender, InputEventArgs e)
        {
            if (!e.Handled)
            {
                OnMouseDown(sender, e);
            }
        }

        public virtual void HandleMouseMove(object sender, InputEventArgs e)
        {
            if (!e.Handled)
            {
                OnMouseMove(sender, e);
            }
        }

        public virtual void HandleMouseUp(object sender, InputEventArgs e)
        {
            if (!e.Handled)
            {
                OnMouseUp(sender, e);
            }
        }

        public virtual void HandleMouseEnter(object sender, InputEventArgs e)
        {
            if (!e.Handled)
            {
                OnMouseEnter(sender, e);
            }
        }

        public virtual void HandleMouseLeave(object sender, InputEventArgs e)
        {
            if (!e.Handled)
            {
                OnMouseLeave(sender, e);
            }
        }

        protected virtual void OnMouseDown(object sender, InputEventArgs e)
        {
            MouseDown?.Invoke(sender, e);
        }

        protected virtual void OnMouseMove(object sender, InputEventArgs e)
        {
            MouseMove?.Invoke(sender, e);
        }

        protected virtual void OnMouseUp(object sender, InputEventArgs e)
        {
            MouseUp?.Invoke(sender, e);
        }

        protected virtual void OnMouseEnter(object sender, InputEventArgs e)
        {
            MouseEnter?.Invoke(sender, e);
        }

        protected virtual void OnMouseLeave(object sender, InputEventArgs e)
        {
            MouseLeave?.Invoke(sender, e);
        }
    }
}

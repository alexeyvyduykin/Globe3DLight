using System;

namespace TimeDataViewer.Core
{
    public abstract class UIElement : SelectableElement
    {
        public event EventHandler<OxyMouseDownEventArgs>? MouseDown;

        public event EventHandler<OxyMouseEventArgs>? MouseMove;

        public event EventHandler<OxyMouseEventArgs>? MouseUp;

        protected internal virtual void OnMouseDown(OxyMouseDownEventArgs e)
        {
            MouseDown?.Invoke(this, e);
        }

        protected internal virtual void OnMouseMove(OxyMouseEventArgs e)
        {
            MouseMove?.Invoke(this, e);
        }

        protected internal virtual void OnMouseUp(OxyMouseEventArgs e)
        {
            MouseUp?.Invoke(this, e);
        }

        public HitTestResult? HitTest(HitTestArguments args)
        {
            return HitTestOverride(args);
        }

        protected virtual HitTestResult? HitTestOverride(HitTestArguments args)
        {
            return null;
        }
    }
}

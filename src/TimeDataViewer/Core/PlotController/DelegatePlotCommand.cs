using System;

namespace TimeDataViewer.Core
{
    public class DelegatePlotCommand<T> : DelegateViewCommand<T> where T : OxyInputEventArgs
    {
        public DelegatePlotCommand(Action<IPlotView, IController, T> handler) : base((v, c, e) => handler((IPlotView)v, c, e))
        {
        }
    }
}

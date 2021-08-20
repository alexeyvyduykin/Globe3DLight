using System;

namespace TimeDataViewer.Core
{
    public class DelegateViewCommand<T> : IViewCommand<T> where T : OxyInputEventArgs
    {
        private readonly Action<IView, IController, T> _handler;

        public DelegateViewCommand(Action<IView, IController, T> handler)
        {
            _handler = handler;
        }

        public void Execute(IView view, IController controller, T args)
        {
            _handler(view, controller, args);
        }

        public void Execute(IView view, IController controller, OxyInputEventArgs args)
        {
            _handler(view, controller, (T)args);
        }
    }
}

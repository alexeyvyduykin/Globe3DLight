using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Editor;


namespace Globe3DLight.Editor
{
    public class AvaloniaEditorCanvasPlatform : ObservableObject, IEditorCanvasPlatform
    {
        private readonly IServiceProvider _serviceProvider;

        private Action _invalidateControl;
   
        public Action InvalidateControl
        {
            get => _invalidateControl;
            set => Update(ref _invalidateControl, value);
        }

        public AvaloniaEditorCanvasPlatform(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}

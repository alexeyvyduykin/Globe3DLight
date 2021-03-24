using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.ViewModels;
using Globe3DLight.Models.Editor;

namespace Globe3DLight.Editor
{
    public class AvaloniaEditorCanvasPlatform : ViewModelBase, IEditorCanvasPlatform
    {
        private readonly IServiceProvider _serviceProvider;

        private Action _invalidateControl;
   
        public Action InvalidateControl
        {
            get => _invalidateControl;
            set => RaiseAndSetIfChanged(ref _invalidateControl, value);
        }

        public AvaloniaEditorCanvasPlatform(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
    }
}

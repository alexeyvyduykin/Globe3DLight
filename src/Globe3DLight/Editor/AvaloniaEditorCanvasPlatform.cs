#nullable disable
using System;
using Globe3DLight.Models.Editor;
using Globe3DLight.ViewModels;

namespace Globe3DLight.Editor
{
    public class AvaloniaEditorCanvasPlatform : ViewModelBase, IEditorCanvasPlatform
    {
        private readonly IServiceProvider _serviceProvider;
        private Action _invalidateControl;

        public AvaloniaEditorCanvasPlatform(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Action InvalidateControl
        {
            get => _invalidateControl;
            set => RaiseAndSetIfChanged(ref _invalidateControl, value);
        }
    }
}

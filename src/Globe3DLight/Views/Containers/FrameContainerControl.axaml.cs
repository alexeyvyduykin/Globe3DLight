using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Globe3DLight.Views.Containers
{
    public class FrameContainerControl : UserControl
    {
        public FrameContainerControl()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

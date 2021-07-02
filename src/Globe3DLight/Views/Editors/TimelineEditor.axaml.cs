using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Globe3DLight.Views.Editors
{
    public partial class TimelineEditor : UserControl
    {
        public TimelineEditor()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

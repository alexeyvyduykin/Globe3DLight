using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Globe3DLight.Views
{
    public class EditorMenuControl : UserControl
    {
        public EditorMenuControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

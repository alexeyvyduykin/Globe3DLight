using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Globe3DLight.AvaloniaUI.Views.ScenarioObjects
{
    public class SatelliteTaskControl : UserControl
    {
        public SatelliteTaskControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

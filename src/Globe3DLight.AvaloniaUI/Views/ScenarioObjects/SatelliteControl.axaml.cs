using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Globe3DLight.AvaloniaUI.Views.ScenarioObjects
{
    public class SatelliteControl : UserControl
    {
        public SatelliteControl()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

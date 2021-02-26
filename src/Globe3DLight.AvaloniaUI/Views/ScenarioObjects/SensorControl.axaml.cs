using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Globe3DLight.AvaloniaUI.Views.ScenarioObjects
{
    public class SensorControl : UserControl
    {
        public SensorControl()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

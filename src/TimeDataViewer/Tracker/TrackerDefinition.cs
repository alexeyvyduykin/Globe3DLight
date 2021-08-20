using Avalonia;
using Avalonia.Markup.Xaml.Templates;

namespace TimeDataViewer
{
    public class TrackerDefinition : AvaloniaObject
    {
        public static readonly StyledProperty<string> TrackerKeyProperty = AvaloniaProperty.Register<TrackerDefinition, string>(nameof(TrackerKey));
        public static readonly StyledProperty<ControlTemplate> TrackerTemplateProperty = AvaloniaProperty.Register<TrackerDefinition, ControlTemplate>(nameof(TrackerTemplate));

        public string TrackerKey
        {
            get
            {
                return GetValue(TrackerKeyProperty);
            }

            set
            {
                SetValue(TrackerKeyProperty, value);
            }
        }

        public ControlTemplate TrackerTemplate
        {
            get
            {
                return GetValue(TrackerTemplateProperty);
            }

            set
            {
                SetValue(TrackerTemplateProperty, value);
            }
        }
    }
}

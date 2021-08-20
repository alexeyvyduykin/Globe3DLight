using Avalonia;
using Avalonia.Input;
using Avalonia.Markup.Xaml.Templates;

namespace TimeDataViewer
{
    public partial class TimelineBase
    {
        public static readonly StyledProperty<ControlTemplate> DefaultTrackerTemplateProperty =
            AvaloniaProperty.Register<TimelineBase, ControlTemplate>(nameof(DefaultTrackerTemplate));

        public ControlTemplate DefaultTrackerTemplate
        {
            get
            {
                return GetValue(DefaultTrackerTemplateProperty);
            }

            set
            {
                SetValue(DefaultTrackerTemplateProperty, value);
            }
        }

        public static readonly StyledProperty<ControlTemplate> ZoomRectangleTemplateProperty =
            AvaloniaProperty.Register<TimelineBase, ControlTemplate>(nameof(ZoomRectangleTemplate));

        public ControlTemplate ZoomRectangleTemplate
        {
            get
            {
                return GetValue(ZoomRectangleTemplateProperty);
            }

            set
            {
                SetValue(ZoomRectangleTemplateProperty, value);
            }
        }

        public static readonly StyledProperty<Cursor> PanCursorProperty =
            AvaloniaProperty.Register<TimelineBase, Cursor>(nameof(PanCursor), new Cursor(StandardCursorType.Hand));

        public Cursor PanCursor
        {
            get
            {
                return GetValue(PanCursorProperty);
            }

            set
            {
                SetValue(PanCursorProperty, value);
            }
        }

        public static readonly StyledProperty<Cursor> PanHorizontalCursorProperty =
            AvaloniaProperty.Register<TimelineBase, Cursor>(nameof(PanHorizontalCursor), new Cursor(StandardCursorType.SizeWestEast));

        public Cursor PanHorizontalCursor
        {
            get
            {
                return GetValue(PanHorizontalCursorProperty);
            }

            set
            {
                SetValue(PanHorizontalCursorProperty, value);
            }
        }

        public static readonly StyledProperty<Cursor> ZoomHorizontalCursorProperty =
            AvaloniaProperty.Register<TimelineBase, Cursor>(nameof(ZoomHorizontalCursor), new Cursor(StandardCursorType.SizeWestEast));

        public Cursor ZoomHorizontalCursor
        {
            get
            {
                return GetValue(ZoomHorizontalCursorProperty);
            }

            set
            {
                SetValue(ZoomHorizontalCursorProperty, value);
            }
        }

        public static readonly StyledProperty<Cursor> ZoomRectangleCursorProperty =
            AvaloniaProperty.Register<TimelineBase, Cursor>(nameof(ZoomRectangleCursor), new Cursor(StandardCursorType.SizeAll));

        public Cursor ZoomRectangleCursor
        {
            get
            {
                return GetValue(ZoomRectangleCursorProperty);
            }

            set
            {
                SetValue(ZoomRectangleCursorProperty, value);
            }
        }

        public static readonly StyledProperty<Cursor> ZoomVerticalCursorProperty =
            AvaloniaProperty.Register<TimelineBase, Cursor>(nameof(ZoomVerticalCursor), new Cursor(StandardCursorType.SizeNorthSouth));

        public Cursor ZoomVerticalCursor
        {
            get
            {
                return GetValue(ZoomVerticalCursorProperty);
            }

            set
            {
                SetValue(ZoomVerticalCursorProperty, value);
            }
        }
    }
}

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Controls.Templates;
using System.Globalization;
using System;
using System.IO;
using System.Linq;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Threading;
using Avalonia.Utilities;
using Avalonia.Controls.Metadata;


namespace Globe3DLight.Controls
{
    public class Editor : UserControl
    {
        public static readonly StyledProperty<object> HeaderContentProperty =
            AvaloniaProperty.Register<Editor, object>(nameof(HeaderContent));

        public static readonly StyledProperty<object> PanelContentProperty =
            AvaloniaProperty.Register<Editor, object>(nameof(PanelContent));

        public static readonly StyledProperty<object> IconProperty = 
            AvaloniaProperty.Register<Editor, object>(nameof(Icon));

        public object HeaderContent
        {
            get { return GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        public object PanelContent
        {
            get { return GetValue(PanelContentProperty); }
            set { SetValue(PanelContentProperty, value); }
        }
        
        public object Icon
        {
            get { return GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
    }
}

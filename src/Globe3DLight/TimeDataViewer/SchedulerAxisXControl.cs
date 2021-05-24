using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Metadata;
using Avalonia.Styling;
using Avalonia.VisualTree;
using TimeDataViewer.ViewModels;
using TimeDataViewer.Core;
using TimeDataViewer.Models;
using System.Xml;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Controls.Metadata;
using Avalonia.Input.GestureRecognizers;
using Avalonia.Input.TextInput;
using Avalonia.Interactivity;
using Avalonia.Controls.Primitives;
using TimeDataViewer.Shapes;
using System.Globalization;

namespace TimeDataViewer
{
    public class SchedulerAxisXControl : UserControl
    {
        private record Label(Point Position, string Text);
        
        private readonly ObservableCollection<Label> _labels;
        private readonly Typeface _typeface;
        private double _tickSize;
        private double _labelFontSize;
        private readonly double _labelMargin;
        private readonly IBrush _foreground;
        private readonly IBrush _foregroundDynamicLabel;
        private Label? _leftLabel;
        private Label? _rightLabel;
        private Label? _dynamicLabel;
        private readonly Pen _defaultRectPen;
        private double _width;
        private double _height;
        private readonly bool _labelRectangleVisible = false;
        private bool _isDynamicLabel;
        private readonly IBrush _brush = new SolidColorBrush() { Color = Color.Parse("#F5F5F5") /*Colors.WhiteSmoke*/ };

        public SchedulerAxisXControl()
        {
            _labels = new ObservableCollection<Label>();

            _foreground = new SolidColorBrush() { Color = Colors.Black };

            _foregroundDynamicLabel = new SolidColorBrush() { Color = Colors.Red };

            _typeface = new Typeface(new FontFamily("Consolas"), FontStyle.Normal, FontWeight.Normal);
            
            _labelMargin = 0.0;

            _isDynamicLabel = false;

            _defaultRectPen = new Pen(Brushes.Black, 1.0);   
        }

        protected override void OnDataContextBeginUpdate()
        {
            base.OnDataContextBeginUpdate();

            if (DataContext is not null && DataContext is ISchedulerControl scheduler)
            {
                scheduler.AxisX.OnAxisChanged -= OnAxisChanged;
                scheduler.PointerEnter -= OnMapEnter;
                scheduler.PointerLeave -= OnMapLeave;
            }
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            base.OnDataContextChanged(e);

            if(DataContext is ISchedulerControl scheduler)
            {
                scheduler.AxisX.OnAxisChanged += OnAxisChanged;
                scheduler.PointerEnter += OnMapEnter;
                scheduler.PointerLeave += OnMapLeave;
            }
        }

        private void OnMapEnter(object? s, EventArgs e)
        {
            _isDynamicLabel = true;

            InvalidateVisual();
        }

        private void OnMapLeave(object? s, EventArgs e)
        {
            _isDynamicLabel = false;

            InvalidateVisual();
        }

        protected override void ArrangeCore(Rect finalRect)
        {
            base.ArrangeCore(finalRect);

            _width = finalRect.Width;
            _height = finalRect.Height;
      
            //_tickSize = _height / 5.0;
            //_labelFontSize = _height * 2.0 / 5.0 - 2.0 * _labelMargin;

            _tickSize = _height / 3.0;
            _labelFontSize = _height / 3.0;
        }

        private void OnAxisChanged(object? s, EventArgs e)
        {
            if (s is IAxis axis && axis?.AxisInfo is AxisInfo axisInfo)
            {
                _labels.Clear();

                double wth = axisInfo.MaxValue - axisInfo.MinValue;
                foreach (var item in axisInfo.Labels)
                {
                    double x = _width * (item.Value - axisInfo.MinValue) / wth;

                    var pend = new Point(x, _tickSize);

                    _labels.Add(new Label(pend, item.Label));
                }

                if (axisInfo.DynamicLabel != null && axisInfo.DynamicLabel is AxisLabelPosition dynLab)
                {                  
                    double W = _width;
                    double width = axisInfo.MaxValue - axisInfo.MinValue;

                    double x = W * (dynLab.Value - axisInfo.MinValue) / width;

                    Point pend = new Point(x, _tickSize);

                    _dynamicLabel = new Label(pend, dynLab.Label);
                }

                if (axisInfo.MinLabel is not null && axisInfo.MinLabel.Equals(string.Empty) == false)
                {
                    _leftLabel = new Label(new Point(0, _tickSize + _labelFontSize), axisInfo.MinLabel);
                }

                if (axisInfo.MaxLabel is not null && axisInfo.MaxLabel.Equals(string.Empty) == false)
                {
                    _rightLabel = new Label(new Point(_width, _tickSize + _labelFontSize), axisInfo.MaxLabel);
                }

                base.InvalidateVisual();
            }
        }

        public override void Render(DrawingContext context)
        {
            context.FillRectangle(_brush, new Rect(0, 0, Bounds.Width, Bounds.Height));

            foreach (var label in _labels)
            {
                DrawTick(context, label, _tickSize);
                DrawLabel(context, label);
            }

            if (_leftLabel is not null)
            {
                DrawTick(context, _leftLabel, _tickSize + _labelFontSize);
                DrawLabel(context, _leftLabel);
            }

            if (_rightLabel is not null)
            {
                DrawTick(context, _rightLabel, _tickSize + _labelFontSize);
                DrawLabel(context, _rightLabel);
            }

            if (_isDynamicLabel == true && _dynamicLabel?.Text is not null)
            {
                DrawDynamicLabel(context, _dynamicLabel);
            }
        }

        private void DrawTick(DrawingContext context, Label label, double tickSize)
        {
            var p0 = new Point(label.Position.X, 0);
            var p1 = new Point(label.Position.X, tickSize);

            context.DrawLine(_defaultRectPen, p0, p1);
        }

        private void DrawLabel(DrawingContext context, Label label)
        {
            var p = label.Position;
            var labelText = label.Text;

            var formattedText = new FormattedText()
            {
                Text = labelText,
                Typeface = _typeface,
                FontSize = _labelFontSize,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.NoWrap,
                Constraint = Size.Infinity,
            };

            double w = formattedText.Bounds.Width;
            double h = _labelFontSize;

            var offsetX = p.X - w / 2.0;
            var offsetY = p.Y + _labelMargin;
         
            if (offsetX < 0)
            {
                offsetX = 0.0;
            }

            if (offsetX + w > _width)
            {
                offsetX = _width - w;
            }

            using (context.PushPreTransform(new TranslateTransform(offsetX, offsetY).Value))
            {
                if (_labelRectangleVisible == true)
                {
                    context.DrawRectangle(
                        null,
                        _defaultRectPen,
                        new Rect(new Point(-_labelMargin, -_labelMargin),
                        new Point(w + _labelMargin, h + _labelMargin)));
                }

                context.DrawText(_foreground, new Point(0, 0), formattedText);
            }
        }

        private void DrawDynamicLabel(DrawingContext context, Label label)
        {
            var p = label.Position;
            var labelText = label.Text;

            var formattedText = new FormattedText()
            {
                Text = labelText,
                Typeface = _typeface,
                FontSize = _labelFontSize,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.NoWrap,
                Constraint = Size.Infinity,
            };

            double w = formattedText.Bounds.Width;
            double h = _labelFontSize;

            var brush = new SolidColorBrush() { Color = Colors.LightYellow, Opacity = 0.8 };
            var offsetX = p.X - w / 2.0;
            var offsetY = 0.0 + _labelMargin;

            if (offsetX < 0)
            {
                offsetX = 0.0;
            }

            if (offsetX + w > _width)
            {
                offsetX = _width - w;
            }

            using (context.PushPreTransform(new TranslateTransform(offsetX, offsetY).Value))              
            {
                if (_labelRectangleVisible == true)
                {
                    context.DrawRectangle(
                        brush,
                        new Pen(Brushes.Red, 2),
                        new Rect(new Point(-_labelMargin, -_labelMargin),
                        new Point(w + _labelMargin, h + _labelMargin)));
                }

                context.DrawText(_foregroundDynamicLabel, new Point(0, 0), formattedText);
            }  
        }
    }
}

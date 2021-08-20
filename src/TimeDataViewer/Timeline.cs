#nullable enable
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Styling;
using Avalonia.VisualTree;
using TimeDataViewer.Core;

namespace TimeDataViewer
{
    public partial class Timeline : TimelineBase
    {
        private readonly PlotModel _internalModel;
        private readonly IPlotController _defaultController;

        public Timeline()
        {
            _series = new ObservableCollection<Series>();
            _axises = new ObservableCollection<Axis>();

            _series.CollectionChanged += OnSeriesChanged;
            _axises.CollectionChanged += OnAxesChanged;

            _defaultController = new PlotController();
            _internalModel = new PlotModel();
            ((IPlotModel)_internalModel).AttachPlotView(this);
        }

        protected override void RenderAxisX(CanvasRenderContext contextAxis, CanvasRenderContext contextPlot)
        {
            foreach (var item in Axises)
            {
                if (item.InternalAxis.IsHorizontal() == true)
                {
                    item.Render(contextAxis, contextPlot);
                }
            }
        }

        protected override void RenderSeries(Canvas canvasPlot, DrawCanvas drawCanvas)
        {
            drawCanvas.RenderSeries(Series.Where(s => s.IsVisible).ToList());
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            if (_panelX == null)
            {
                return;
            }

            // TODO: replace from Timeline to TimelineBase
            _panelX.PointerPressed += _panelX_PointerPressed;
            _panelX.PointerMoved += _panelX_PointerMoved;
            _panelX.PointerReleased += _panelX_PointerReleased;
        }

        private bool _isPressed = false;

        private void _panelX_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            _isPressed = false;
        }

        private void _panelX_PointerMoved(object? sender, PointerEventArgs e)
        {
            if (_isPressed == true)
            {
                base.OnPointerMoved(e);
                if (e.Handled)
                {
                    return;
                }

                e.Pointer.Capture(_panelX);

                var point = e.GetPosition(_panelX).ToScreenPoint();

                foreach (var a in Axises)
                {
                    if (a.InternalAxis.IsHorizontal() == true && a is DateTimeAxis axis)
                    {
                        var value = axis.InternalAxis.InverseTransform(point.X);

                        DateTime TimeOrigin = new DateTime(1899, 12, 31, 0, 0, 0, DateTimeKind.Utc);
                        Slider.IsTracking = false;
                        Slider.CurrentValue = TimeOrigin.AddDays(value - 1);
                        Slider.IsTracking = true;
                    }
                }
            }
        }

        public void SliderTo(double value)
        {
            foreach (var a in Axises)
            {
                if (a.InternalAxis.IsHorizontal() == true)
                {
                    DateTime TimeOrigin = new DateTime(1899, 12, 31, 0, 0, 0, DateTimeKind.Utc);
                    Slider.IsTracking = false;
                    Slider.CurrentValue = TimeOrigin.AddDays(value - 1);
                    Slider.IsTracking = true;
                }
            }
        }

        private void _panelX_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (e.Handled)
            {
                return;
            }

            Focus();
            e.Pointer.Capture(_panelX);

            var point = e.GetPosition(_panelX).ToScreenPoint();

            foreach (var a in Axises)
            {
                if (a.InternalAxis.IsHorizontal() == true && a is DateTimeAxis axis)
                {
                    var value = axis.InternalAxis.InverseTransform(point.X);

                    DateTime TimeOrigin = new DateTime(1899, 12, 31, 0, 0, 0, DateTimeKind.Utc);
                    Slider.IsTracking = false;
                    Slider.CurrentValue = TimeOrigin.AddDays(value - 1);
                    Slider.IsTracking = true;

                    _isPressed = true;
                }
            }
        }

        protected override void RenderSlider(CanvasRenderContext contextAxis, CanvasRenderContext contextPlot)
        {
            // TODO: Remove update method from render and replace to UpdateModel (present correct not work) 
            UpdateSlider();
            Slider.Render(contextAxis, contextPlot);
        }

        public override PlotModel ActualModel => _internalModel;

        public override IPlotController ActualController => _defaultController;

        // Updates the model. If Model==<c>null</c>, an internal model will be created.
        // The ActualModel.Update will be called (updates all series data).
        protected override void UpdateModel(bool updateData = true)
        {
            SynchronizeProperties();
            SynchronizeSeries();
            SynchronizeAxes();

            base.UpdateModel(updateData);

            //UpdateSlider();
        }

        // Called when the visual appearance is changed.     
        protected void OnAppearanceChanged()
        {
            InvalidatePlot(false);
        }

        // Called when the visual appearance is changed.
        private static void AppearanceChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            ((Timeline)d).OnAppearanceChanged();
        }

        private static void SliderChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            ((Timeline)d).SyncLogicalTree(e);
        }

        private void OnAxesChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            SyncLogicalTree(e);
        }

        private void OnSeriesChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            SyncLogicalTree(e);
        }

        private void SyncLogicalTree(NotifyCollectionChangedEventArgs e)
        {
            // In order to get DataContext and binding to work with the series, axes and annotations
            // we add the items to the logical tree
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems.OfType<ISetLogicalParent>())
                {
                    item.SetParent(this);
                }
                LogicalChildren.AddRange(e.NewItems.OfType<ILogical>());
                VisualChildren.AddRange(e.NewItems.OfType<IVisual>());
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems.OfType<ISetLogicalParent>())
                {
                    item.SetParent(null);
                }
                foreach (var item in e.OldItems)
                {
                    LogicalChildren.Remove((ILogical)item);
                    VisualChildren.Remove((IVisual)item);
                }
            }
        }

        private void SyncLogicalTree(AvaloniaPropertyChangedEventArgs e)
        {
            // In order to get DataContext and binding to work with the series, axes and annotations
            // we add the items to the logical tree
            if (e.NewValue != null)
            {
                ((ISetLogicalParent)e.NewValue).SetParent(this);

                LogicalChildren.Add((ILogical)e.NewValue);
                VisualChildren.Add((IVisual)e.NewValue);
            }

            if (e.OldValue != null)
            {
                ((ISetLogicalParent)e.OldValue).SetParent(null);

                LogicalChildren.Remove((ILogical)e.OldValue);
                VisualChildren.Remove((IVisual)e.OldValue);
            }
        }

        // Synchronize properties in the internal Plot model
        private void SynchronizeProperties()
        {
            var m = _internalModel;

            m.PlotMarginLeft = PlotMargins.Left;
            m.PlotMarginTop = PlotMargins.Top;
            m.PlotMarginRight = PlotMargins.Right;
            m.PlotMarginBottom = PlotMargins.Bottom;

            //     m.Padding = Padding.ToOxyThickness();

            //  m.DefaultColors = DefaultColors.Select(c => c.ToOxyColor()).ToArray();

            //   m.AxisTierDistance = AxisTierDistance;
        }

        // Synchronizes the axes in the internal model.      
        private void SynchronizeAxes()
        {
            _internalModel.Axises.Clear();
            foreach (var a in Axises)
            {
                _internalModel.Axises.Add(a.CreateModel());
            }
        }

        // Synchronizes the series in the internal model.       
        private void SynchronizeSeries()
        {
            _internalModel.Series.Clear();
            foreach (var s in Series)
            {
                _internalModel.Series.Add(s.CreateModel());
            }
        }

        private void UpdateSlider()
        {
            Slider.UpdateMinMax(ActualModel);
        }
    }
}

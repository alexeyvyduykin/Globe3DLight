using System;
using System.Collections;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Styling;
using Avalonia.LogicalTree;
using TimeDataViewer.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Threading;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Metadata;
using Avalonia.VisualTree;
using TimeDataViewer.Shapes;
using TimeDataViewer.Spatial;
using System.Xml;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Controls.Metadata;
using Avalonia.Input.GestureRecognizers;
using Avalonia.Input.TextInput;
using Avalonia.Interactivity;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Primitives;
using TimeDataViewer.Views;
using System.Threading.Tasks;
using TimeDataViewer.Models;

namespace TimeDataViewer
{
    public record Interval(double Left, double Right);

    public class Series : ItemsControl, IStyleable, ISeriesControl
    {
        Type IStyleable.StyleKey => typeof(ItemsControl);

        private readonly Factory _factory;
        private ISeries? _seriesViewModel;
        private BaseIntervalVisual _intervalTemplate;
       
        public event EventHandler? OnInvalidateData;

        public Series()
        {
            _factory = new Factory();
        }

        public ISeries? SeriesViewModel 
        {
            get => _seriesViewModel; 
            set => _seriesViewModel = value; 
        }

        public bool DirtyItems { get; set; } = false;

        public static readonly StyledProperty<BaseIntervalVisual> IntervalTemplateProperty =
            AvaloniaProperty.Register<Series, BaseIntervalVisual>(nameof(IntervalTemplate));

        public BaseIntervalVisual IntervalTemplate
        {
            get { return _intervalTemplate; }
            set { SetAndRaise(IntervalTemplateProperty, ref _intervalTemplate, value); }
        }

        public static readonly StyledProperty<Control> TooltipProperty =    
            AvaloniaProperty.Register<Series, Control>(nameof(Tooltip), new IntervalTooltip());

        public Control Tooltip
        {
            get { return GetValue(TooltipProperty); }
            set { SetValue(TooltipProperty, value); }
        }

        public static readonly StyledProperty<string> LeftBindingPathProperty =    
            AvaloniaProperty.Register<Series, string>(nameof(LeftBindingPath), string.Empty);

        public string LeftBindingPath
        {
            get { return GetValue(LeftBindingPathProperty); }
            set { SetValue(LeftBindingPathProperty, value); }
        }

        public static readonly StyledProperty<string> RightBindingPathProperty =    
            AvaloniaProperty.Register<Series, string>(nameof(RightBindingPath), string.Empty);

        public string RightBindingPath
        {
            get { return GetValue(RightBindingPathProperty); }
            set { SetValue(RightBindingPathProperty, value); }
        }

        public static readonly StyledProperty<string> CategoryProperty =    
            AvaloniaProperty.Register<Series, string>(nameof(Category), string.Empty);

        public string Category
        {
            get { return GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromLogicalTree(e);

            if (OnInvalidateData is not null)
            {
                foreach (var d in OnInvalidateData.GetInvocationList())
                {
                    OnInvalidateData -= (EventHandler)d;
                }
            }
        }

        protected override void ItemsChanged(AvaloniaPropertyChangedEventArgs e)
        {
            base.ItemsChanged(e);

            if (e.NewValue is not null && e.NewValue is IEnumerable items)
            {
                if (DirtyItems == false)
                {                                  
                    Update(items);
                    DirtyItems = true;
                    OnInvalidateData?.Invoke(this, EventArgs.Empty);
                    //Debug.WriteLine($"Series -> OnInvalidateData -> Count = {OnInvalidateData?.GetInvocationList().Length}");
                }
            }
        }

        private void Update(IEnumerable items)
        {
            IList<Interval> list;

            if (items is IEnumerable<Interval> ivals)
            {
                list = new List<Interval>(ivals);
            }
            else
            {
                list = UpdateItems(items);
            }

            _seriesViewModel = _factory.CreateSeries(Category, this);

            var intervals = list.Select(s => _factory.CreateInterval(s.Left, s.Right, this));

            _seriesViewModel.ReplaceIntervals(intervals);
        }

        private IList<Interval> UpdateItems(IEnumerable items)
        {
            if (string.IsNullOrWhiteSpace(LeftBindingPath) == false && string.IsNullOrWhiteSpace(RightBindingPath) == false)
            {
                var list = new List<Interval>();

                foreach (var item in items)
                {
                    var propertyInfoLeft = item.GetType().GetProperty(LeftBindingPath);
                    var propertyInfoRight = item.GetType().GetProperty(RightBindingPath);

                    var valueLeft = propertyInfoLeft?.GetValue(item, null);
                    var valueRight = propertyInfoRight?.GetValue(item, null);

                    if (valueLeft is not null && valueRight is not null && valueLeft is double left && valueRight is double right)
                    {
                        list.Add(new Interval(left, right));
                    }
                }
                return list;             
            }

            return new List<Interval>();
        }

        public SchedulerControl? Scheduler => (((ILogical)this).LogicalParent is SchedulerControl scheduler) ? scheduler : null;

        public virtual IntervalTooltipViewModel CreateTooltip(IInterval marker)
        {
            return new IntervalTooltipViewModel(marker);
        }

        public virtual IShape CreateIntervalShape(IInterval interval)
        {
            return IntervalTemplate.Clone(interval);
        }

        public IShape CreateSeriesShape()
        {                
            return new SeriesVisual() { DataContext = this };            
        }
    }
}

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
using Avalonia.Input;
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

namespace TimeDataViewer
{
    public record Interval(double Left, double Right);

    public class Series : ItemsControl, IStyleable // TemplatedControl
    {
        Type IStyleable.StyleKey => typeof(ItemsControl);

        private readonly Factory _factory;

        public Series()
        {
            _factory = new Factory();
            IntervalTemplate = new IntervalVisual() { Series = this };

            IntervalTemplateProperty.Changed.AddClassHandler<Series>((d, e) => d.IntervalTemplateChanged(e));
        }

        public SeriesViewModel? String { get; set; }

        public IEnumerable<IntervalViewModel>? Ivals { get; set; }

        public bool DirtyItems { get; set; } = false;

        private void IntervalTemplateChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if(e.NewValue is not null && e.NewValue is BaseIntervalVisual ival)
            {
                ival.Series = this;
            }
        }

        public static readonly StyledProperty<BaseIntervalVisual> IntervalTemplateProperty =    
            AvaloniaProperty.Register<Series, BaseIntervalVisual>(nameof(IntervalTemplate));

        public BaseIntervalVisual IntervalTemplate
        {
            get { return GetValue(IntervalTemplateProperty); }
            set { SetValue(IntervalTemplateProperty, value); }
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

        private void ItemsSource_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            PassingLogicalTree(e);
        }

        private void PassingLogicalTree(NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems is not null)
            {
                foreach (var item in e.NewItems.OfType<ISetLogicalParent>())
                {
                    item.SetParent(this);
                }
                LogicalChildren.AddRange(e.NewItems.OfType<ILogical>());
                VisualChildren.AddRange(e.NewItems.OfType<IVisual>());
            }

            if (e.OldItems is not null)
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

        protected override void ItemsChanged(AvaloniaPropertyChangedEventArgs e)
        {
            base.ItemsChanged(e);

            if (e.NewValue is not null && e.NewValue is IEnumerable items)
            {
                if (items is IEnumerable<Interval>)
                {
                    String = _factory.CreateSeries(Category);
                    Ivals = ((IList<Interval>)Items).Select(s => _factory.CreateInterval(s, String, IntervalTemplate));

                    DirtyItems = true;
                }
                else
                {
                    UpdateItems(items);
                }
            }
        }

        private void UpdateItems(IEnumerable items)
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

                Items = new ObservableCollection<Interval>(list);
            }
        }

        public SchedulerControl? Map => (((ILogical)this).LogicalParent is SchedulerControl map) ? map : null;

        public virtual IntervalTooltipViewModel CreateTooltip(IntervalViewModel marker)
        {
            return new IntervalTooltipViewModel(marker);
        }
    }
}

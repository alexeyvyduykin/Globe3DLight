using System;
using System.Collections;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Utilities;

namespace TimeDataViewer
{
    public abstract class Series : ItemsControl
    {
        public static readonly StyledProperty<Color> ColorProperty =
            AvaloniaProperty.Register<Series, Color>(nameof(Color), Colors.Transparent);

        private readonly EventListener _eventListener;

        static Series()
        {
            IsVisibleProperty.Changed.AddClassHandler<Series>(AppearanceChanged);
            BackgroundProperty.Changed.AddClassHandler<Series>(AppearanceChanged);
            ColorProperty.Changed.AddClassHandler<Series>(AppearanceChanged);
        }

        protected Series()
        {
            _eventListener = new EventListener(OnCollectionChanged);
        }

        public Color Color
        {
            get
            {
                return GetValue(ColorProperty);
            }

            set
            {
                SetValue(ColorProperty, value);
            }
        }

        public Core.Series InternalSeries { get; protected set; }

        public abstract Core.Series CreateModel();

        protected static void AppearanceChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            ((Series)d).OnVisualChanged();
        }

        protected static void DataChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            ((Series)d).OnDataChanged();
        }

        protected void OnDataChanged()
        {
            if (Parent is Core.IPlotView pc)
            {
                pc.InvalidatePlot();
            }
        }

        protected override void ItemsChanged(AvaloniaPropertyChangedEventArgs e)
        {
            base.ItemsChanged(e);
            SubscribeToCollectionChanged(e.OldValue as IEnumerable, e.NewValue as IEnumerable);
            OnDataChanged();
        }

        protected void OnVisualChanged()
        {
            if (Parent is Core.IPlotView pc)
            {
                pc.InvalidatePlot(false);
            }
        }

        protected virtual void SynchronizeProperties(Core.Series s)
        {
            s.IsVisible = IsVisible;
        }

        /// <summary>
        /// If the ItemsSource implements INotifyCollectionChanged update the visual when the collection changes.
        /// </summary>
        /// <param name="oldValue">The old ItemsSource</param>
        /// <param name="newValue">The new ItemsSource</param>
        private void SubscribeToCollectionChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            var collection = oldValue as INotifyCollectionChanged;
            if (collection != null)
            {
                WeakSubscriptionManager.Unsubscribe(collection, "CollectionChanged", _eventListener);
            }

            collection = newValue as INotifyCollectionChanged;
            if (collection != null)
            {
                WeakSubscriptionManager.Subscribe(collection, "CollectionChanged", _eventListener);
            }
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            OnDataChanged();
        }

        /// <summary>
        /// Listens to and forwards any collection changed events
        /// </summary>
        private class EventListener : IWeakSubscriber<NotifyCollectionChangedEventArgs>
        {
            private readonly EventHandler<NotifyCollectionChangedEventArgs> _onCollectionChanged;

            public EventListener(EventHandler<NotifyCollectionChangedEventArgs> onCollectionChanged)
            {
                _onCollectionChanged = onCollectionChanged;
            }

            public void OnEvent(object sender, NotifyCollectionChangedEventArgs e)
            {
                _onCollectionChanged(sender, e);
            }
        }
    }
}

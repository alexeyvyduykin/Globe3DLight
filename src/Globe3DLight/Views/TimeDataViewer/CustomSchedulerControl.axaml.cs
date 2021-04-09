using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Globe3DLight.ViewModels.TimeDataViewer.Data;
using System.Collections.Generic;

namespace Globe3DLight.Views.TimeDataViewer
{
    public class CustomSchedulerControl : UserControl
    {
        public CustomSchedulerControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            Scheduler = this.FindControl<SchedulerControl>("Scheduler");
        }
    
        SchedulerControl Scheduler;

        void Init()
        {
            BaseModelView model = new BaseModelView(BaseModelView.EDataType.New/*Old*/, RealDataPool.ETimePeriod.Week/*Day*/);

            foreach (var str in model.Strings)
            {
                var markerStr = new SchedulerString(str.Name);
                {
                    var shape = new StringVisual(markerStr);

             //       shape.Tooltip.SetValues(new StringData() { Name = str.Name, Description = str.Description });

                    markerStr.Shape = shape;
                }

                List<SchedulerInterval> ivalMarkers = new List<SchedulerInterval>();

                foreach (var itemIval in str.Intervals)
                {
                    var markerIval = new SchedulerInterval(itemIval.Left, itemIval.Right);
                    {
                        markerIval.String = markerStr;
                        var shape = new IntervalVisual(markerIval);
                        //shape.Tooltip.SetValues(
                        //    new IntervalData()
                        //    {
                        //        Name = itemIval.Name,
                        //        Id = itemIval.Id,
                        //        Begin = itemIval.Left,
                        //        End = itemIval.Right,
                        //        StringName = itemIval.Group.Name
                        //    });

                        markerIval.Shape = shape;
                    }
                    ivalMarkers.Add(markerIval);
                }

                Scheduler.AddIntervals(ivalMarkers, markerStr);
            }

        }
    }
}

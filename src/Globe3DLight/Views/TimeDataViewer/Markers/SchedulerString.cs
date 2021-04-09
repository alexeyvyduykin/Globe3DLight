#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Drawing;
using System.ComponentModel;
using Avalonia.Media;
using System.Windows;
using Globe3DLight.Spatial;

namespace Globe3DLight.Views.TimeDataViewer
{
    public class SchedulerString : SchedulerTargetMarker
    {
        static Random r = new Random();
        public SchedulerString(string name) : base()
        {
            base.Name = name;

            string[] descr = new string[]
            {
                "Satellites times vision",
                "Sunlight satellite subpoint",
                "Satellites angle rotation",
                "Satellite received",
                "Sensor daylight",
                "GroundStation work",
                "Satellite orbit correction"
            };

            var index = r.Next(0, descr.Length - 1);

            Description = descr[index];
        }

        private double timeBegin = double.NaN;
        private double timeEnd = double.NaN;

        public double TimeBegin
        {
            get
            {
                if (double.IsNaN(timeBegin) == true)
                {
                    timeBegin = MinTime();
                }

                return timeBegin;
            }
            set
            {
                if (double.IsNaN(timeEnd) == false)
                {
                    if (timeBegin <= timeEnd)
                    {
                        timeBegin = value;
                    }
                }
                else
                {
                    timeBegin = value;
                }
            }
        }

        public double TimeEnd
        {
            get
            {
                if (double.IsNaN(timeEnd) == true)
                {
                    timeEnd = MaxTime();
                }

                return timeEnd;
            }
            set
            {
                if (double.IsNaN(timeBegin) == false)
                {
                    if (timeEnd >= timeBegin)
                    {
                        timeEnd = value;
                    }
                }
                else
                {
                    timeEnd = value;
                }
            }
        }

        //new void UpdateLocalPosition()
        //{
        //    if (Map != null)
        //    {
        //      //  SCPoint p = Map.FromSchedulerPointToLocal(new SCSchedulerPoint(Position__.X, Map.ActualHeight * Position__.Y));
        //        int x = Map.FromLocalValueToPixelX(base.PositionX);
        //        int y = Map.FromLocalValueToPixelY(base.Map.ActualHeight * (double)base.PositionY);
        //        var p = new SCPoint(x, y);

        //        p.Offset(-(int)Map.SchedulerTranslateTransform.X, -(int)Map.SchedulerTranslateTransform.Y);

        //        LocalPositionX = (int)(p.X + (int)(Offset.X));
        //        LocalPositionY = (int)(p.Y + (int)(Offset.Y));
        //    }
        //}

        public readonly ObservableCollection<SchedulerInterval> Intervals = new ObservableCollection<SchedulerInterval>();

        public override Point2D Offset
        {
            get
            {
                return base.Offset;
            }
            set
            {
                base.Offset = value;

                Intervals.ToList().ForEach(s => s.Offset = value);
            }
        }

        private double MinTime()
        {
            return Intervals.Min(s => s.Left);
        }
        private double MaxTime()
        {
            return Intervals.Max(s => s.Right);
        }

        public string Description { get; set; }
    }


}

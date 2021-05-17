#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using TimeDataViewer.Spatial;

namespace TimeDataViewer.ViewModels
{
    public class IntervalViewModel : MarkerViewModel
    {
        private SeriesViewModel _series;
   
        public IntervalViewModel(double left, double right) : base()
        {
            Left = left;
            Right = right;
        }
        
        public SeriesViewModel Series
        {
            get
            {
                return _series;
            }
            set
            {
                _series = value;


                base.SetLocalPosition(Left + (Right - Left) / 2.0, _series.LocalPosition.Y);

                //   base.PositionX = Left + (Right - Left) / 2.0;
                //   base.PositionY = _string.PositionY;

                //  base.Position = new SCSchedulerPoint(Left + (Right - Left) / 2.0, _string.Position.Y);
            }
        }
     
        public double Left { get; set; }

        public double Right { get; set; }

        public double Length => Right - Left;
    }
}

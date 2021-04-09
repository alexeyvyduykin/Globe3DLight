#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Spatial;

namespace Globe3DLight.Views.TimeDataViewer
{
    public class SchedulerInterval : SchedulerMarker
    {
        public SchedulerInterval(double left, double right) : base()
        {
            this.Left = left;
            this.Right = right;

            Name = string.Format("Interval_{0}_{1}", (int)left, (int)right);

            _id = Guid.NewGuid();
        }

        SchedulerString _string;
        public SchedulerString String
        {
            get
            {
                return _string;
            }
            set
            {
                _string = value;


                base.SetLocalPosition(Left + (Right - Left) / 2.0, _string.LocalPosition.Y);

                //   base.PositionX = Left + (Right - Left) / 2.0;
                //   base.PositionY = _string.PositionY;

                //  base.Position = new SCSchedulerPoint(Left + (Right - Left) / 2.0, _string.Position.Y);
            }
        }

        public Point2I Center
        {
            get
            {
                int x = base.AbsolutePositionX;
                int y = base.AbsolutePositionY;

                return new Point2I(x, y);
            }
        }

        Guid _id;
        public string Id { get { return _id.ToString(); } }

        public string Line { get { return Id; } }

        public string Name { get; set; }

        public double Left { get; set; }

        public double Right { get; set; }

        public double Length { get { return Right - Left; } }

    }

}

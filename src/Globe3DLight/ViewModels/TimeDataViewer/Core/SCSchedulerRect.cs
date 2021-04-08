#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public struct SCSchedulerRect
    {
        public static readonly SCSchedulerRect Empty;

        private double x;
        private double y;
        private double width;
        private double height;

        public SCSchedulerRect(double x, double y, double width, double height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            NotEmpty = true;
        }

        public double X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
            }
        }

        public double Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
            }
        }

        public double Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }

        public double Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }

        public double Left
        {
            get
            {
                return this.x;
            }
        }

        public double Right
        {
            get
            {
                return this.y + this.width;
            }
        }

        public double Bottom
        {
            get
            {
                return this.y;
            }
        }

        public double Top
        {
            get
            {
                return this.y + this.height;
            }
        }




        bool NotEmpty;

        // returns true if coordinates wasn't assigned       
        public bool IsEmpty
        {
            get
            {
                return !NotEmpty;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SCSchedulerRect))
            {
                return false;
            }
            SCSchedulerRect ef = (SCSchedulerRect)obj;
            return ((((ef.X == this.X) && (ef.Y == this.Y)) && (ef.Width == this.Width)) && (ef.Height == this.Height));
        }

        public static bool operator ==(SCSchedulerRect left, SCSchedulerRect right)
        {
            return ((((left.X == right.X) && (left.Y == right.Y)) && (left.Width == right.Width)) && (left.Height == right.Height));
        }

        public static bool operator !=(SCSchedulerRect left, SCSchedulerRect right)
        {
            return !(left == right);
        }

        public bool Contains(double primary, double secondary)
        {
            return ((((this.x <= primary) && (primary < (this.x + this.Width))) && (this.Y >= secondary)) && (secondary > (this.Y - this.Height)));
        }

        public bool Contains(Point2 pt)
        {
            return this.Contains(pt.X, pt.Y);
        }

        public bool Contains(SCSchedulerRect rect)
        {
            return ((((this.X <= rect.X) && ((rect.X + rect.Width) <= (this.X + this.Width))) && (this.Y >= rect.Y)) && ((rect.Y - rect.Height) >= (this.Y - this.Height)));
        }

        public override int GetHashCode()
        {
            if (this.IsEmpty)
            {
                return 0;
            }
            return (((this.X.GetHashCode() ^ this.Y.GetHashCode()) ^ this.Width.GetHashCode()) ^ this.Height.GetHashCode());
        }

        public void Offset(Point2 pos)
        {
            this.Offset(pos.X, pos.Y);
        }

        public void Offset(double x, double y)
        {
            this.x += x;
            this.y -= y;
        }

        public override string ToString()
        {
            return ("{X = " + this.X.ToString(CultureInfo.CurrentCulture) + ", Y = " + this.Y.ToString(CultureInfo.CurrentCulture) + " , Width = " + this.Width.ToString(CultureInfo.CurrentCulture) + ", Height = " + this.Height.ToString(CultureInfo.CurrentCulture) + "}");
        }

        static SCSchedulerRect()
        {
            Empty = new SCSchedulerRect();
        }
    }
}

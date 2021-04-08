#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{

    public struct SCViewport
    {
        public static readonly SCViewport Empty = new SCViewport();

        private double x;
        private double y;
        private double width;
        private double height;

        public SCViewport(double x, double y, double width, double height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public SCViewport(SCViewport viewport)
        {
            this.x = viewport.x;
            this.y = viewport.y;
            this.width = viewport.width;
            this.height = viewport.height;
        }

        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public double Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        public double Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        public double Left
        {
            get
            {
                return x;
            }
        }

        public double Right
        {
            get
            {
                return x + width;
            }
        }

        public double Bottom
        {
            get
            {
                return y;
            }
        }

        public double Top
        {
            get
            {
                return y + height;
            }
        }

        public Point2D P0
        {
            get
            {
                return new Point2D(x, y);
            }
        }

        public Point2D P1
        {
            get
            {
                return new Point2D(x + width, y + height);
            }
        }

        public double MiddleX
        {
            get
            {
                return x + width / 2.0;
            }
        }

        public double MiddleY
        {
            get
            {
                return y + height / 2.0;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return height == 0.0 && width == 0.0 && x == 0.0 && y == 0.0;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SCViewport))
                return false;

            SCViewport comp = (SCViewport)obj;

            return (comp.X == this.X) && (comp.Y == this.Y) &&
                   (comp.Width == this.Width) && (comp.Height == this.Height);
        }

        public static bool operator ==(SCViewport left, SCViewport right)
        {
            return (left.X == right.X && left.Y == right.Y &&
                    left.Width == right.Width && left.Height == right.Height);
        }

        public static bool operator !=(SCViewport left, SCViewport right)
        {
            return !(left == right);
        }

        public bool Contains(double x, double y)
        {
            return this.X <= x && x < this.X + this.Width &&
               this.Y <= y && y < this.Y + this.Height;
        }

        public bool Contains(SCViewport rect)
        {
            return (this.X <= rect.X) && ((rect.X + rect.Width) <= (this.X + this.Width)) &&
                   (this.Y <= rect.Y) && ((rect.Y + rect.Height) <= (this.Y + this.Height));
        }

        public override int GetHashCode()
        {
            if (this.IsEmpty)
            {
                return 0;
            }
            return (int)((((int)this.X ^ (((int)this.Y << 13) | ((int)this.Y >> 0x13))) ^ (((int)this.Width << 0x1a) | ((int)this.Width >> 6))) ^ (((int)this.Height << 7) | ((int)this.Height >> 0x19)));
        }

        public void Offset(double x, double y)
        {
            this.X += x;
            this.Y += y;
        }

        public override string ToString()
        {
            return "{X=" + X.ToString(CultureInfo.CurrentCulture) + ",Y=" + Y.ToString(CultureInfo.CurrentCulture) +
               ",Width=" + Width.ToString(CultureInfo.CurrentCulture) +
               ",Height=" + Height.ToString(CultureInfo.CurrentCulture) + "}";
        }
    }
}

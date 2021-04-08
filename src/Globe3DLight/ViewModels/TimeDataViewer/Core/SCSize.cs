#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public struct SCSize
    {
        public static readonly SCSize Empty = new SCSize();

        private int width;
        private int height;

        public SCSize(SCPoint pt)
        {
            width = pt.X;
            height = pt.Y;
        }

        public SCSize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public static SCSize operator +(SCSize sz1, SCSize sz2)
        {
            return Add(sz1, sz2);
        }

        public static SCSize operator -(SCSize sz1, SCSize sz2)
        {
            return Subtract(sz1, sz2);
        }

        public static bool operator ==(SCSize sz1, SCSize sz2)
        {
            return sz1.Width == sz2.Width && sz1.Height == sz2.Height;
        }

        public static bool operator !=(SCSize sz1, SCSize sz2)
        {
            return !(sz1 == sz2);
        }

        public static explicit operator SCPoint(SCSize size)
        {
            return new SCPoint(size.Width, size.Height);
        }

        public bool IsEmpty
        {
            get
            {
                return width == 0 && height == 0;
            }
        }

        public int Width
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

        public int Height
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

        public static SCSize Add(SCSize sz1, SCSize sz2)
        {
            return new SCSize(sz1.Width + sz2.Width, sz1.Height + sz2.Height);
        }

        public static SCSize Subtract(SCSize sz1, SCSize sz2)
        {
            return new SCSize(sz1.Width - sz2.Width, sz1.Height - sz2.Height);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SCSize))
                return false;

            SCSize comp = (SCSize)obj;
            // Note value types can't have derived classes, so we don't need to
            //
            return (comp.width == this.width) &&
                      (comp.height == this.height);
        }

        public override int GetHashCode()
        {
            if (this.IsEmpty)
            {
                return 0;
            }
            return (Width.GetHashCode() ^ Height.GetHashCode());
        }

        public override string ToString()
        {
            return "{Width=" + width.ToString(CultureInfo.CurrentCulture) + ", Height=" + height.ToString(CultureInfo.CurrentCulture) + "}";
        }
    }
}

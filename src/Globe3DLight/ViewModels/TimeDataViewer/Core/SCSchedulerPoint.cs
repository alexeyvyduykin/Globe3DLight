#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    //public struct SCSchedulerPoint
    //{
    //    private double x;
    //    private double y;

    //    public static readonly SCSchedulerPoint Empty = new SCSchedulerPoint();

    //    public SCSchedulerPoint(double x, double y)
    //    {
    //        this.x = x;
    //        this.y = y;

    //        NotEmpty = true;
    //    }

    //    public double Y
    //    {
    //        get
    //        {
    //            return this.y;
    //        }
    //        set
    //        {
    //            this.y = value;
    //            NotEmpty = true;
    //        }
    //    }

    //    public double X
    //    {
    //        get
    //        {
    //            return this.x;
    //        }
    //        set
    //        {
    //            this.x = value;
    //            NotEmpty = true;
    //        }
    //    }

    //    bool NotEmpty;

    //    // returns true if coordinates wasn't assigned      
    //    public bool IsEmpty
    //    {
    //        get
    //        {
    //            return !NotEmpty;
    //        }
    //    }


    //    public void Offset(SCSchedulerPoint pos)
    //    {
    //        this.Offset(pos.X, pos.Y);
    //    }

    //    public void Offset(double x, double y)
    //    {
    //        this.x += x;
    //        this.y -= y;
    //    }


    //    public static SCSchedulerPoint operator +(SCSchedulerPoint pt, SCSchedulerSize sz)
    //    {
    //        return Add(pt, sz);
    //    }

    //    public static SCSchedulerPoint operator -(SCSchedulerPoint pt, SCSchedulerSize sz)
    //    {
    //        return Subtract(pt, sz);
    //    }

    //    public static SCSchedulerSize operator -(SCSchedulerPoint pt1, SCSchedulerPoint pt2)
    //    {
    //        return new SCSchedulerSize(pt1.X - pt2.X, pt2.Y - pt1.Y);
    //    }

    //    public static bool operator ==(SCSchedulerPoint left, SCSchedulerPoint right)
    //    {
    //        return ((left.X == right.X) && (left.Y == right.Y));
    //    }

    //    public static bool operator !=(SCSchedulerPoint left, SCSchedulerPoint right)
    //    {
    //        return !(left == right);
    //    }


    //    public static SCSchedulerPoint Add(SCSchedulerPoint pt, SCSchedulerSize sz)
    //    {
    //        return new SCSchedulerPoint(pt.X - sz.SizeX, pt.Y + sz.SizeY);
    //    }

    //    public static SCSchedulerPoint Subtract(SCSchedulerPoint pt, SCSchedulerSize sz)
    //    {
    //        return new SCSchedulerPoint(pt.X + sz.SizeX, pt.Y - sz.SizeY);
    //    }

    //    public override string ToString()
    //    {
    //        return ("{X = " + this.x.ToString(CultureInfo.CurrentCulture) + ", Y = " + this.y.ToString(CultureInfo.CurrentCulture) + "}");
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        if (!(obj is SCSchedulerPoint))
    //        {
    //            return false;
    //        }
    //        SCSchedulerPoint tf = (SCSchedulerPoint)obj;
    //        return (((tf.x == this.x) && (tf.y == this.y)) && tf.GetType().Equals(base.GetType()));
    //    }

    //    public override int GetHashCode()
    //    {
    //        return (this.x.GetHashCode() ^ this.y.GetHashCode());
    //    }
    //}
}

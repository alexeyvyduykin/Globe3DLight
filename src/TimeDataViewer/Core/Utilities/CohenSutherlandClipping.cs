using TimeDataViewer.Spatial;

namespace TimeDataViewer.Core
{
    /// <summary>
    /// Provides a line clipping algorithm.
    /// </summary>
    /// <remarks>See http://en.wikipedia.org/wiki/Cohen%E2%80%93Sutherland</remarks>
    public class CohenSutherlandClipping
    {
        private const int Bottom = 4; // 0100
        private const int Inside = 0; // 0000
        private const int Left = 1; // 0001
        private const int Right = 2; // 0010
        private const int Top = 8; // 1000
        private readonly double _xmax;
        private readonly double _xmin;
        private readonly double _ymax;
        private readonly double _ymin;

        public CohenSutherlandClipping(OxyRect rect)
        {
            _xmin = rect.Left;
            _xmax = rect.Right;
            _ymin = rect.Top;
            _ymax = rect.Bottom;
        }

        /// <summary>
        /// Cohen–Sutherland clipping algorithm clips a line from
        /// P0 = (x0, y0) to P1 = (x1, y1) against a rectangle with
        /// diagonal from <code>(xmin, ymin)</code> to <code>(xmax, ymax)</code>.
        /// </summary>
        /// <param name="p0">The point P0.</param>
        /// <param name="p1">The point P1.</param>
        /// <returns><c>true</c> if the line is inside</returns>
        public bool ClipLine(ref ScreenPoint p0, ref ScreenPoint p1)
        {
            // compute out codes for P0, P1, and whatever point lies outside the clip rectangle

            // the following method is inlined manually
            // int outcode0 = ComputeOutCode(p0.x, p0.y);           
            int outcode0 = Inside; // initialized as being inside of clip window

            if (p0.X < _xmin)
            {
                // to the left of clip window
                outcode0 |= Left;
            }
            else if (p0.X > _xmax)
            {
                // to the right of clip window
                outcode0 |= Right;
            }

            if (p0.Y < _ymin)
            {
                // below the clip window
                outcode0 |= Bottom;
            }
            else if (p0.Y > _ymax)
            {
                // above the clip window
                outcode0 |= Top;
            }

            // the following method is inlined manually
            // int outcode1 = ComputeOutCode(p1.x, p1.y);
            int outcode1 = Inside; // initialized as being inside of clip window

            if (p1.X < _xmin)
            {
                // to the left of clip window
                outcode1 |= Left;
            }
            else if (p1.X > _xmax)
            {
                // to the right of clip window
                outcode1 |= Right;
            }

            if (p1.Y < _ymin)
            {
                // below the clip window
                outcode1 |= Bottom;
            }
            else if (p1.Y > _ymax)
            {
                // above the clip window
                outcode1 |= Top;
            }

            bool accept = false;

            while (true)
            {
                if ((outcode0 | outcode1) == 0)
                {
                    // logical or is 0. Trivially accept and get out of loop
                    accept = true;
                    break;
                }

                if ((outcode0 & outcode1) != 0)
                {
                    // logical and is not 0. Trivially reject and get out of loop
                    break;
                }

                // failed both tests, so calculate the line segment to clip
                // from an outside point to an intersection with clip edge
                double x = 0, y = 0;

                // At least one endpoint is outside the clip rectangle; pick it.
                int outcodeOut = outcode0 != 0 ? outcode0 : outcode1;

                // Now find the intersection point;
                // use formulas y = y0 + slope * (x - x0), x = x0 + (1 / slope) * (y - y0)
                if ((outcodeOut & Top) != 0)
                {
                    // point is above the clip rectangle
                    x = p0.X + ((p1.X - p0.X) * (_ymax - p0.Y) / (p1.Y - p0.Y));
                    y = _ymax;
                }
                else if ((outcodeOut & Bottom) != 0)
                {
                    // point is below the clip rectangle
                    x = p0.X + ((p1.X - p0.X) * (_ymin - p0.Y) / (p1.Y - p0.Y));
                    y = _ymin;
                }
                else if ((outcodeOut & Right) != 0)
                {
                    // point is to the right of clip rectangle
                    y = p0.Y + ((p1.Y - p0.Y) * (_xmax - p0.X) / (p1.X - p0.X));
                    x = _xmax;
                }
                else if ((outcodeOut & Left) != 0)
                {
                    // point is to the left of clip rectangle
                    y = p0.Y + ((p1.Y - p0.Y) * (_xmin - p0.X) / (p1.X - p0.X));
                    x = _xmin;
                }

                // Now we move outside point to intersection point to clip
                // and get ready for next pass.
                if (outcodeOut == outcode0)
                {
                    p0._x = x;
                    p0._y = y;

                    // the following code is inlined
                    // outcode0 = ComputeOutCode(p0.x, p0.y);
                    outcode0 = Inside; // initialized as being inside of clip window

                    if (p0.X < _xmin)
                    {
                        // to the left of clip window
                        outcode0 |= Left;
                    }
                    else if (p0.X > _xmax)
                    {
                        // to the right of clip window
                        outcode0 |= Right;
                    }

                    if (p0.Y < _ymin)
                    {
                        // below the clip window
                        outcode0 |= Bottom;
                    }
                    else if (p0.Y > _ymax)
                    {
                        // above the clip window
                        outcode0 |= Top;
                    }
                }
                else
                {
                    p1._x = x;
                    p1._y = y;

                    // the following method is inlined manually
                    // outcode1 = ComputeOutCode(p1.x, p1.y);
                    outcode1 = Inside; // initialized as being inside of clip window

                    if (p1.X < _xmin)
                    {
                        // to the left of clip window
                        outcode1 |= Left;
                    }
                    else if (p1.X > _xmax)
                    {
                        // to the right of clip window
                        outcode1 |= Right;
                    }

                    if (p1.Y < _ymin)
                    {
                        // below the clip window
                        outcode1 |= Bottom;
                    }
                    else if (p1.Y > _ymax)
                    {
                        // above the clip window
                        outcode1 |= Top;
                    }
                }
            }

            return accept;
        }

        public bool IsInside(ScreenPoint s)
        {
            if (s.X < _xmin)
            {
                return false;
            }

            if (s.X > _xmax)
            {
                return false;
            }

            if (s.Y < _ymin)
            {
                return false;
            }

            if (s.Y > _ymax)
            {
                return false;
            }

            return true;
        }
    }
}

#nullable enable
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Avalonia.Controls;
using Avalonia.Media;
using System.Diagnostics;
using System;
using Globe3DLight.Spatial;
using Globe3DLight.ViewModels.TimeDataViewer;

namespace Globe3DLight.Views.TimeDataViewer
{
    public class SchedulerTargetMarker : SchedulerMarker, ITargetMarker
    {
        public SchedulerTargetMarker()
        {
        }

        public override int AbsolutePositionX
        {
            get
            {
                return base.AbsolutePositionX;
            }
            set
            {
                if (base.AbsolutePositionX != value)
                {
                    isPositionChange = true;

                    base.AbsolutePositionX = value;
                }
            }
        }

        public override int AbsolutePositionY
        {
            get
            {
                return base.AbsolutePositionY;
            }
            set
            {
                if (base.AbsolutePositionY != value)
                {
                    isPositionChange = true;

                    base.AbsolutePositionY = value;
                }
            }
        }


        protected override void UpdateAbsolutePosition()
        {
            base.UpdateAbsolutePosition();

            if (OnTargetMarkerPositionChanged != null)
            {
                OnTargetMarkerPositionChanged(this);
            }
        }



        bool isPositionChange = true;

        Point2D _localPosition;
        public Point2D LocalPosition_Unknown
        {
            get
            {
                //            if(isPositionChange == false)
                //             {
                //                 return _localPosition;
                //             }

                isPositionChange = false;


                if (Map != null)
                {
                    Point2D pos__ = Map.FromAbsoluteToLocal(base.AbsolutePositionX, base.AbsolutePositionY);

                    _localPosition = new Point2D(pos__.X, pos__.Y);
                }

                //  _localPosition = new SCSchedulerPoint(base.PositionX, base.PositionY);

                return _localPosition;
            }
        }

        public string Name { get; set; }

        public event SCTargetMarkerPositionChanged OnTargetMarkerPositionChanged;
    }
}

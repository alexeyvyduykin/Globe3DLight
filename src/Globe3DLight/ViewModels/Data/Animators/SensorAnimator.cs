using System;
using System.Collections.Generic;
using GlmSharp;
using System.Linq;
using Globe3DLight.Models.Data;

namespace Globe3DLight.ViewModels.Data
{
    public class Scan : ViewModelBase
    {
        private dvec3 _p0;
        private dvec3 _p1;
        private dvec3 _p2;
        private dvec3 _p3;

        public dvec3 p0
        {
            get => _p0;
            set => RaiseAndSetIfChanged(ref _p0, value);
        }

        public dvec3 p1
        {
            get => _p1;
            set => RaiseAndSetIfChanged(ref _p1, value);
        }

        public dvec3 p2
        {
            get => _p2;
            set => RaiseAndSetIfChanged(ref _p2, value);
        }

        public dvec3 p3
        {
            get => _p3;
            set => RaiseAndSetIfChanged(ref _p3, value);
        }


        public (dvec3 p0, dvec3 p1) Slice(double d)
        {
            var dp = Math.Max(Math.Min(d, 1.0), 0.0);

            var v0 = p1 - p0;
            var v1 = p2 - p3;

            return (p0 + v0 * dp, p3 + v1 * dp);
        }


        public override bool IsDirty()
        {
            var isDirty = base.IsDirty();
            return isDirty;
        }

        public override void Invalidate()
        {
            base.Invalidate();
        }
    }

    public class Shoot : ViewModelBase
    {
        private dvec3 _p0;
        private dvec3 _p1;
        private dvec3 _pos;

        public dvec3 p0
        {
            get => _p0;
            set => RaiseAndSetIfChanged(ref _p0, value);
        }
        public dvec3 p1
        {
            get => _p1;
            set => RaiseAndSetIfChanged(ref _p1, value);
        }
        public dvec3 Pos
        {
            get => _pos;
            set => RaiseAndSetIfChanged(ref _pos, value);
        }

        public override bool IsDirty()
        {
            var isDirty = base.IsDirty();
            return isDirty;
        }
    
        public override void Invalidate()
        {
            base.Invalidate();
        }
    }

    public class SensorAnimator : BaseState, IAnimator
    {
        private readonly SensorData _data;
        private IEventList<SensorInterval> _shootingEvents;
        private bool _enable;
        private Shoot _shoot;
        private Scan _scan;
        private int _direction;
        private bool _first = true;

        public SensorAnimator(SensorData data)
        {
            _data = data;
            //   _shootingEvents =
            //    data.Shootings.Select(s => new SensorInterval(s.BeginTime, s.EndTime, s.Gam1, s.Gam2, s.Range1, s.Range2)).ToEventList();
        }

        public bool Enable
        {
            get => _enable;
            protected set => RaiseAndSetIfChanged(ref _enable, value);
        }

        public Shoot Shoot
        {
            get => _shoot;
            protected set => RaiseAndSetIfChanged(ref _shoot, value);
        }

        public Scan Scan
        {
            get => _scan;
            protected set => RaiseAndSetIfChanged(ref _scan, value);
        }

        public int Direction
        {
            get => _direction;
            protected set => RaiseAndSetIfChanged(ref _direction, value);
        }

        private void Init(double t)
        {
            _shootingEvents = new EventList<SensorInterval>();
     
            if (Owner is SatelliteAnimator satAnimator)
            {
                foreach (var item in _data.Shootings)
                {
                    var begin = (item.BeginTime <= 86400.0) ? item.BeginTime : 86400.0;
                    var end = (item.EndTime <= 86400.0) ? item.EndTime : 86400.0;
                    var center = (end - begin) / 2.0;

                    satAnimator.Animate(begin);
                    var mat0 = satAnimator.ModelMatrix;

                    satAnimator.Animate(end);
                    var mat1 = satAnimator.ModelMatrix;

                    satAnimator.Animate(center);
                    var mat = satAnimator.ModelMatrix;

                    var direction = (item.Gam1 >= 0.0 && item.Gam2 >= 0.0) ? 1 : -1;
                    double yScale1 = -item.Range1;
                    double yScale2 = -item.Range2;
                    double angRot1 = -Math.Abs(glm.Radians(item.Gam1)) * direction;
                    double angRot2 = -Math.Abs(glm.Radians(item.Gam2)) * direction;

                    var p0 = new dvec3(0.0, Math.Cos(angRot1) * yScale1, Math.Sin(angRot1) * yScale1);
                    var p1 = new dvec3(0.0, Math.Cos(angRot2) * yScale2, Math.Sin(angRot2) * yScale2);

                    var ap0 = (mat0 * p0.ToDvec4()).ToDvec3();
                    var ap1 = (mat1 * p0.ToDvec4()).ToDvec3();
                    var ap2 = (mat1 * p1.ToDvec4()).ToDvec3();
                    var ap3 = (mat0 * p1.ToDvec4()).ToDvec3();
                    var cp0 = (mat * p0.ToDvec4()).ToDvec3();
                    var cp1 = (mat * p1.ToDvec4()).ToDvec3();

                    var scan = new Scan()
                    {
                        p0 = cp0 - (ap1 - ap0) / 2.0,
                        p1 = cp0 + (ap1 - ap0) / 2.0,
                        p2 = cp1 + (ap2 - ap3) / 2.0,
                        p3 = cp1 - (ap2 - ap3) / 2.0,
                    };

                    var ival = new SensorInterval(begin, end, direction, scan);

                    _shootingEvents.Add(ival);
                }

                // return true modelView
                satAnimator.Animate(t);
            }

            _first = false;
        }

        public void Animate(double t)
        {
            if (_first == true)
            {
                Init(t);                
            }

            var activeInterval = _shootingEvents.ActiveInterval(t);

            Enable = activeInterval != default;

            if (Enable == true)
            {
                if (Owner is SatelliteAnimator satAnimator)
                {
                    var activeState = activeInterval.Animate(t);

                    var begin = activeInterval.BeginTime;
                    var end = activeInterval.EndTime;

                    var dt = (t - begin) / (end - begin);

                    var (p0, p1) = activeState.Scan.Slice(dt);

                    var pos = satAnimator.Position;

                    Shoot = new Shoot() { p0 = p0, p1 = p1, Pos = pos };

                    Scan = activeState.Scan;

                    Direction = activeState.Direction;
                }
            }
        }
    }
}

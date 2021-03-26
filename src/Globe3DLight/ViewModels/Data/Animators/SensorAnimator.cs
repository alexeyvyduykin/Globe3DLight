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
               
            var rotationNode = Owner;
            if (rotationNode is RotationAnimator)
            {
                var satelliteNode = rotationNode.Owner;
                if (satelliteNode is SatelliteAnimator satAnimator)
                {
                    foreach (var item in _data.Shootings)
                    {
                        var begin = (item.BeginTime <= 86400.0) ? item.BeginTime : 86400.0;
                        var end = (item.EndTime <= 86400.0) ? item.EndTime : 86400.0;

                        satAnimator.Animate(begin);
                        var mat0 = satAnimator.ModelMatrix;

                        satAnimator.Animate(end);
                        var mat1 = satAnimator.ModelMatrix;

                        var ival = new SensorInterval(begin, end, item.Gam1, item.Gam2, item.Range1, item.Range2, mat0, mat1);

                        _shootingEvents.Add(ival);
                    }

                    // return true modelView
                    satAnimator.Animate(t);
                }
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
                var activeState = activeInterval.Animate(t);

                Shoot = activeState.Shoot;

                Scan = activeState.Scan;

                Direction = activeState.Direction;
            }
        }
    }
}

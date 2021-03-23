using System;
using System.Collections.Generic;
using GlmSharp;
using System.Linq;

namespace Globe3DLight.Data
{

    public interface ISensorState : IState, IAnimator
    {
        bool Enable { get; }

        IShoot Shoot { get; }

        int Direction { get; }
    }

    public interface IScan //: IObservableObject
    {
        dvec3 p0 { get; set; }
        dvec3 p1 { get; set; }
        dvec3 p2 { get; set; }
        dvec3 p3 { get; set; }
    }

    public interface IShoot //: IObservableObject
    {
        dvec3 p0 { get; set; }
        dvec3 p1 { get; set; }
        dvec3 p2 { get; set; }
        dvec3 p3 { get; set; }
    }

    public class Scan : ObservableObject, IScan
    {
        private dvec3 _p0;
        private dvec3 _p1;
        private dvec3 _p2;
        private dvec3 _p3;

        public dvec3 p0
        {
            get => _p0;
            set => Update(ref _p0, value);
        }
        public dvec3 p1
        {
            get => _p1;
            set => Update(ref _p1, value);
        }
        public dvec3 p2
        {
            get => _p2;
            set => Update(ref _p2, value);
        }
        public dvec3 p3
        {
            get => _p3;
            set => Update(ref _p3, value);
        }

        public override bool IsDirty()
        {
            var isDirty = base.IsDirty();
            return isDirty;
        }

        /// <inheritdoc/>
        public override void Invalidate()
        {
            base.Invalidate();
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }

    public class Shoot : ObservableObject, IShoot
    {
        private dvec3 _p0;
        private dvec3 _p1;
        private dvec3 _p2;
        private dvec3 _p3;

        public dvec3 p0
        {
            get => _p0;
            set => Update(ref _p0, value);
        }
        public dvec3 p1
        {
            get => _p1;
            set => Update(ref _p1, value);
        }
        public dvec3 p2
        {
            get => _p2;
            set => Update(ref _p2, value);
        }
        public dvec3 p3
        {
            get => _p3;
            set => Update(ref _p3, value);
        }

        public override bool IsDirty()
        {
            var isDirty = base.IsDirty();
            return isDirty;
        }

        /// <inheritdoc/>
        public override void Invalidate()
        {
            base.Invalidate();
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }

    public class SensorAnimator : ObservableObject, ISensorState
    {      
        private readonly IEventList<SensorInterval> _shootingEvents;
        private bool _enable;
        private IShoot _shoot;
        private int _direction;

        public bool Enable
        {
            get => _enable;
            protected set => Update(ref _enable, value);
        }

        public IShoot Shoot
        {
            get => _shoot;
            protected set => Update(ref _shoot, value);
        }

        public int Direction
        {
            get => _direction;
            protected set => Update(ref _direction, value);
        }

        public SensorAnimator(SensorData data)
        {                      
            _shootingEvents = 
                data.Shootings.Select(s => new SensorInterval(s.BeginTime, s.EndTime, s.Gam1, s.Gam2, s.Range1, s.Range2)).ToEventList();
        }

        public void Animate(double t)
        {
            var activeInterval = _shootingEvents.ActiveInterval(t);

            Enable = activeInterval != default;

            if (Enable == true)
            {
                var activeState = activeInterval.Animate(t);

                Shoot = activeState.Shoot;

                Direction = activeState.Direction;
            }
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}

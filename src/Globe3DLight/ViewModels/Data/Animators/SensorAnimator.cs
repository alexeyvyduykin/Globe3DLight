using System;
using System.Collections.Generic;
using GlmSharp;
using System.Linq;
using Globe3DLight.Models.Data;

namespace Globe3DLight.ViewModels.Data
{
    public record Scan(dvec3 P0, dvec3 P1, dvec3 P2, dvec3 P3) 
    { 
        public (dvec3 p0, dvec3 p1) Slice(double d)
        {
            var dp = Math.Max(Math.Min(d, 1.0), 0.0);

            var v0 = P1 - P0;
            var v1 = P2 - P3;

            return (P0 + v0 * dp, P3 + v1 * dp);
        }
    }

    public record Shoot(dvec3 Pos, dvec3 P0, dvec3 P1);

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
                    var center = begin + (end - begin) / 2.0;

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

                    ap0 = ap0.Normalized * (6371.0 + 10.0);
                    ap1 = ap1.Normalized * (6371.0 + 10.0);
                    ap2 = ap2.Normalized * (6371.0 + 10.0);
                    ap3 = ap3.Normalized * (6371.0 + 10.0);
                    cp0 = cp0.Normalized * (6371.0 + 10.0);
                    cp1 = cp1.Normalized * (6371.0 + 10.0);

                    var scan = new Scan(
                        cp0 - (ap1 - ap0) / 2.0,
                        cp0 + (ap1 - ap0) / 2.0,
                        cp1 + (ap2 - ap3) / 2.0,
                        cp1 - (ap2 - ap3) / 2.0);

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

                    Shoot = new Shoot(pos, p0, p1);

                    Scan = activeState.Scan;

                    Direction = activeState.Direction;
                }
            }
        }
    }
}

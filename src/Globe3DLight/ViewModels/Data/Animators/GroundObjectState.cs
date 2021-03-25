using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
using Globe3DLight.Models.Data;
using Globe3DLight.Models;

namespace Globe3DLight.ViewModels.Data
{
    public class GroundObjectState : BaseState, IFrameable
    {
        private dvec3 _position;       
        private double _lon;
        private double _lat;
        private readonly double _earthRadius;
     
        public GroundObjectState(GroundObjectData data)
        {
            _lon = data.Lon;
            _lat = data.Lat;
            _earthRadius = data.EarthRadius;

            Update();
        }

        public dvec3 Position
        {
            get => _position;
            protected set => RaiseAndSetIfChanged(ref _position, value);
        }

        public double Lon
        {
            get => _lon;
            protected set => RaiseAndSetIfChanged(ref _lon, value);
        }

        public double Lat
        {
            get => _lat;
            protected set => RaiseAndSetIfChanged(ref _lat, value);
        }

        private void Update()
        {
            double lon = glm.Radians(_lon);
            double lat = glm.Radians(_lat);
            double r = _earthRadius;

            dmat3 model3x3 = new dmat3();
            model3x3.m02 = -Math.Cos(lon) * Math.Sin(lat);
            model3x3.m12 = Math.Cos(lon) * Math.Cos(lat);
            model3x3.m22 = -Math.Sin(lon);
            model3x3.m00 = -Math.Sin(lon) * Math.Sin(lat);
            model3x3.m10 = Math.Sin(lon) * Math.Cos(lat);
            model3x3.m20 = Math.Cos(lon);
            model3x3.m01 = Math.Cos(lat);
            model3x3.m11 = Math.Sin(lat);
            model3x3.m21 = 0.0;

            ModelMatrix = new dmat4(model3x3) * dmat4.Translate(new dvec3(0.0, r, 0.0));
            _position = new dvec3(ModelMatrix.Column3);
        }
    }
}

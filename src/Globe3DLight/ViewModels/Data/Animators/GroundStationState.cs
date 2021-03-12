﻿using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;

namespace Globe3DLight.Data
{
    public interface IGroundStationState : IState, IFrameable
    {    
        dvec3 Position { get; }

        double Lon { get; }

        double Lat { get; }

        double Elevation { get; }
    }


    public class GroundStationState : ObservableObject, IGroundStationState
    {        
        private dvec3 _position;
        private dmat4 _modelMatrix;

        private double _lon;
        private double _lat;
        private double _elevation;
        private readonly double _earthRadius;
 
        public GroundStationState(GroundStationData data)
        {
            _lon = data.Lon;
            _lat = data.Lat;
            _elevation = data.Elevation;
            _earthRadius = data.EarthRadius;

            Update();
        }

        public dvec3 Position
        {
            get => _position;
            protected set => Update(ref _position, value);
        }

        public dmat4 ModelMatrix
        {
            get => _modelMatrix;
            protected set => Update(ref _modelMatrix, value);
        }

        public double Lon
        {
            get => _lon;
            protected set => Update(ref _lon, value);
        }

        public double Lat
        {
            get => _lat;
            protected set => Update(ref _lat, value);
        }

        public double Elevation
        {
            get => _elevation;
            protected set => Update(ref _elevation, value);
        }

        private void Update()
        {

            double lon = glm.Radians(_lon);
            double lat = glm.Radians(_lat);
            double r = _elevation + _earthRadius;

            dmat3 model3x3 = new dmat3();
            model3x3.m02 = -Math.Cos(lon) * Math.Sin(lat); model3x3.m12 = Math.Cos(lon) * Math.Cos(lat); model3x3.m22 = -Math.Sin(lon);
            model3x3.m00 = -Math.Sin(lon) * Math.Sin(lat); model3x3.m10 = Math.Sin(lon) * Math.Cos(lat); model3x3.m20 = Math.Cos(lon);
            model3x3.m01 = Math.Cos(lat); model3x3.m11 = Math.Sin(lat); model3x3.m21 = 0.0;

            this.ModelMatrix = new dmat4(model3x3) * dmat4.Translate(new dvec3(0.0, r, 0.0));
            this.Position = new dvec3(ModelMatrix.Column3);
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}

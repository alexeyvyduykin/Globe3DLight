using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Data.Database;


namespace Globe3DLight.Data.Animators
{
    public interface IGroundStationData : IData
    {
        dvec3 Position { get; }

        dmat4 ModelMatrix { get; }

        double Lon { get; }

        double Lat { get; }

        double Elevation { get; }
    }


    public class GroundStationData : ObservableObject, IGroundStationData
    {

        private readonly IGroundStationDatabase _groundStationDatabase;

        private dvec3 _position;
        private dmat4 _modelMatrix;

        private double _lon;
        private double _lat;
        private double _elevation;

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


        public GroundStationData(IGroundStationDatabase groundStationDatabase)
        {
            this._groundStationDatabase = groundStationDatabase;

            this.Lon = groundStationDatabase.Lon;
            this.Lat = groundStationDatabase.Lat;
            this.Elevation = groundStationDatabase.Elevation;

            Update();
        }

        private void Update()
        {

            double lon = glm.Radians(_groundStationDatabase.Lon);
            double lat = glm.Radians(_groundStationDatabase.Lat);
            double r = _groundStationDatabase.Elevation + _groundStationDatabase.EarthRadius;

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

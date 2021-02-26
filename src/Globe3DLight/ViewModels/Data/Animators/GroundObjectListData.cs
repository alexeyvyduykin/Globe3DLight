using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
using Globe3DLight.Data.Database;


namespace Globe3DLight.Data.Animators
{
    public interface IGroundObjectListData : IData
    {
        IDictionary<string, dvec3> Positions { get; }
        IDictionary<string, dmat4> ModelMatrices { get; }
        IDictionary<string, (double lon, double lat)> SourcePositions { get; }
    }


    public class GroundObjectListData : ObservableObject, IGroundObjectListData
    {
        private readonly IGroundObjectListDatabase _db;

        private IDictionary<string, dvec3> _positions;
        private IDictionary<string, dmat4> _modelMatrices;
        private IDictionary<string, (double lon, double lat)> _sourcePositions;

        public GroundObjectListData(IGroundObjectListDatabase db)
        {
            _db = db;

            _sourcePositions = db.Positions;

            _positions = new Dictionary<string, dvec3>();

            _modelMatrices = new Dictionary<string, dmat4>();

            Update();
        }

        public IDictionary<string, dvec3> Positions
        {
            get => _positions;
            protected set => Update(ref _positions, value);
        }

        public IDictionary<string, dmat4> ModelMatrices
        {
            get => _modelMatrices;
            protected set => Update(ref _modelMatrices, value);
        }

        public IDictionary<string, (double lon, double lat)> SourcePositions
        {
            get => _sourcePositions;
            protected set => Update(ref _sourcePositions, value);
        }

        private void Update()
        {
            foreach (var item in _db.Positions)
            {
                var name = item.Key;
                var lonDeg = item.Value.lon;
                var latDeg = item.Value.lat;

                double lon = glm.Radians(lonDeg);
                double lat = glm.Radians(latDeg);
                double r = _db.EarthRadius;

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

                var modelMatrix = new dmat4(model3x3) * dmat4.Translate(new dvec3(0.0, r, 0.0));
                var position = new dvec3(modelMatrix.Column3);

                _modelMatrices.Add(name, modelMatrix);
                _positions.Add(name, position);
            }
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}

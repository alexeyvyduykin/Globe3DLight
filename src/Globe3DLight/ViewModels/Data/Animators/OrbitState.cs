using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Data
{
    public interface IOrbitState : IState
    {
        IList<(double x, double y, double z, double u)> Vertices { get; }
    }


    public class OrbitState : ObservableObject, IOrbitState
    {
        private IList<(double x, double y, double z, double u)> _vertices;

        public OrbitState(OrbitData data)
        {
            _vertices = data.Records.Select(s => (s[0], s[1], s[2], s[3])).ToList();
        }

        public IList<(double x, double y, double z, double u)> Vertices 
        { 
            get => _vertices;
            protected set => Update(ref value, _vertices);
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}

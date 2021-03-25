using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Models.Data;
using GlmSharp;

namespace Globe3DLight.ViewModels.Data
{
    public class OrbitState : BaseState
    {
        private readonly IList<(double x, double y, double z, double u)> _vertices;

        public OrbitState(OrbitData data)
        {
            _vertices = data.Records.Select(s => (s[0], s[1], s[2], s[3])).ToList();

            ModelMatrix = dmat4.Identity;
        }

        public IList<(double x, double y, double z, double u)> Vertices 
        { 
            get => _vertices;
            protected set => RaiseAndSetIfChanged(ref value, _vertices);
        }
    }
}

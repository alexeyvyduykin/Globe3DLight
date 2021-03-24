using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Models.Geometry;

namespace Globe3DLight.ViewModels.Geometry
{
    public abstract class BaseIndices : IIndices
    {
        private readonly IndicesType _type;

        protected BaseIndices(IndicesType type)
        {
            this._type = type;
        }

        public IndicesType Datatype => _type;              
    }
}

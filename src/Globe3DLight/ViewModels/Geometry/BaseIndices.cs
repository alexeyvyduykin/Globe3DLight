using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Geometry
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

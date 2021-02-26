using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Renderer
{
   
    public class ShaderVertexAttribute : IShaderVertexAttribute
    {
        public ShaderVertexAttribute(string name, int location, ActiveAttribType type, int length)
        {
            this.name = name;
            this.location = location;
            this.type = type;
            this.length = length;
        }

        public string Name
        {
            get { return name; }
        }

        public int Location
        {
            get { return location; }
        }

        public ActiveAttribType Datatype
        {
            get { return type; }
        }

        public int Length
        {
            get { return length; }
        }

        private string name;
        private int location;
        private ActiveAttribType type;
        private int length;
    }
}

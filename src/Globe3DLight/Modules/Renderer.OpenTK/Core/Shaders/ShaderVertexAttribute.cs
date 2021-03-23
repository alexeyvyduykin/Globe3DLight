using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A = OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK.Core
{
   
    internal class ShaderVertexAttribute
    {
        public ShaderVertexAttribute(string name, int location, A.ActiveAttribType type, int length)
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

        public A.ActiveAttribType Datatype
        {
            get { return type; }
        }

        public int Length
        {
            get { return length; }
        }

        private string name;
        private int location;
        private A.ActiveAttribType type;
        private int length;
    }
}

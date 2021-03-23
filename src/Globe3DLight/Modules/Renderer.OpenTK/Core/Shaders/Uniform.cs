using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A = OpenTK.Graphics.OpenGL;


namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class Uniform
    {
        protected Uniform(string name, A.ActiveUniformType type)
        {
            this.name = name;
            this.type = type;
        }

        public string Name
        {
            get { return name; }
        }

        public A.ActiveUniformType Datatype
        {
            get { return type; }
        }

        private string name;
        private A.ActiveUniformType type;
    }

    internal abstract class Uniform<T> : Uniform
    {
        protected Uniform(string name, A.ActiveUniformType type)
            : base(name, type)
        {
        }

        public abstract T Value { set; get; }
    }

}

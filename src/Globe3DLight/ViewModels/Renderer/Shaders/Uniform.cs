using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Globe3DLight.Renderer
{
    public class Uniform : IUniform
    {
        private string _name;
        private ActiveUniformType _type;

        protected Uniform(string name, ActiveUniformType type)
        {
            this._name = name;
            this._type = type;
        }

        public string Name => _name;         
        public ActiveUniformType Datatype => _type;         
    }

    public abstract class Uniform<T> : Uniform, IUniform<T>
    {
        protected Uniform(string name, ActiveUniformType type)
            : base(name, type)
        {
        }

        public abstract T Value { set; get; }
    }

}

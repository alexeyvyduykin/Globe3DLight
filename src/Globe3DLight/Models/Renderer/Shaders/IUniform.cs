using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Models.Renderer
{

    public interface IUniform
    {
        string Name { get; }

        ActiveUniformType Datatype { get; }
    }

    public interface IUniform<T> : IUniform
    {
        T Value { set; get; }
    }

}

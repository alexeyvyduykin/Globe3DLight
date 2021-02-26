﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Geometry
{
    public interface IIndices
    {
        IndicesType Datatype { get; }
    }

    public interface IIndices<T> : IIndices
    {
        IList<T> Values { get; }
    }
}

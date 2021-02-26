using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Geometry;
using Globe3DLight.Geometry.Models;

namespace Globe3DLight
{
    public interface IModelLoader
    {
        bool LoadFromFile(string path, bool withTexture);

        IEnumerable<IAMesh> AMeshes { get; }

        IModel LoadModel(string path, bool withTexture);
    }
}

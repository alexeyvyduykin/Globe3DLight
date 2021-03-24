using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Models.Geometry;
using Globe3DLight.Models.Geometry.Models;

namespace Globe3DLight.Models
{
    public interface IModelLoader
    {
        bool LoadFromFile(string path, bool withTexture);

        IEnumerable<IAMesh> AMeshes { get; }

        IModel LoadModel(string path, bool withTexture);
    }
}

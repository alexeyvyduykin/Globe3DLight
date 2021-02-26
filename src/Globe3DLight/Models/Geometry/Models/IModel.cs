using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Geometry.Models
{
    //public interface IModel
    //{
    //    IList<IMesh> Meshes { get; }
    //}
    
    public interface IModel
    {
        IList<IMesh> Meshes { get; }

        IList<IMaterial> Materials { get; }
    }
}

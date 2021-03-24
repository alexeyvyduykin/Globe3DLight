using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.ViewModels.Geometry.Models;

namespace Globe3DLight.Models.Geometry.Models
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

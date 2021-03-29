using System;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.ViewModels.Entities;

namespace Globe3DLight.Models.Entities
{
    public interface IAssetable<T> 
    {
        ImmutableArray<T> Assets { get; set; }
    }
}

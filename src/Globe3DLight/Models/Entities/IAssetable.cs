using System;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.ViewModels.Entities;

namespace Globe3DLight.Models.Entities
{
    public interface IAssetable 
    {
        ImmutableArray<BaseEntity> Assets { get; set; }
    }
}

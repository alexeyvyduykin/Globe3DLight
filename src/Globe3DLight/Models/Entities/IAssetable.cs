using System;
using System.Collections.Immutable;
using System.Text;

namespace Globe3DLight.Entities
{
    public interface IAssetable 
    {
        ImmutableArray<IEntity> Assets { get; set; }
    }
}

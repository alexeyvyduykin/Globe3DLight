using System;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.ScenarioObjects;

namespace Globe3DLight
{
    public interface IAssetable 
    {
        ImmutableArray<IScenarioObject> Assets { get; set; }
    }
}

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.ScenarioObjects
{
    public interface IChildren : IObservableObject
    {
        ImmutableArray<IScenarioObject> Children { get; set; }
    }
}

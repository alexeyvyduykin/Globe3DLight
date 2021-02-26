using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.ScenarioObjects
{
    public interface IGroundObject : IScenarioObject
    {
        IGroundObjectList ParentList { get; set; }
        bool IsVisible { get; set; }
    }
}

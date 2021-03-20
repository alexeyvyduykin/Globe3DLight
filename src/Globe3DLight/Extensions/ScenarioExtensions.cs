using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Containers;
using Globe3DLight.Entities;

namespace Globe3DLight
{
    public static class ScenarioExtensions
    {
        public static void AddSatelliteTask(this IScenarioContainer scenario, ISatelliteTask task)
        {
            if (scenario?.Tasks != null && task != null)
            {
                var builder = scenario.Tasks.ToBuilder();
                builder.Add(task);
                scenario.Tasks = builder.ToImmutable();
            }
        }
    }
}

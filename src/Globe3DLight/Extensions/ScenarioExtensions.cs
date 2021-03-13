using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Containers;
using Globe3DLight.ScenarioObjects;

namespace Globe3DLight
{
    public static class ScenarioExtensions
    {
        public static void AddSatelliteTask(this IScenarioContainer scenario, ISatelliteTask task)
        {
            if (scenario.SatelliteTasks != null && task != null)
            {
                var builder = scenario.SatelliteTasks.ToBuilder();
                builder.Add(task);
                scenario.SatelliteTasks = builder.ToImmutable();
            }
        }
    }
}

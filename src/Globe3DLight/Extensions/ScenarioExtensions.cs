using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels.Entities;

namespace Globe3DLight
{
    public static class ScenarioExtensions
    {
        public static void AddSatelliteTask(this ScenarioContainerViewModel scenario, SatelliteTask task)
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

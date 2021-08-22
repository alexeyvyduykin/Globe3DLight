using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels.Entities;

namespace Globe3DLight
{
    public static class ScenarioExtensions
    {
        //public static void AddSatelliteTask(this ScenarioContainerViewModel scenario, SatelliteTask task)
        //{
        //    if (scenario?.TaskListEditor.Tasks != null && task != null)
        //    {
        //        var builder = scenario.TaskListEditor.Tasks.ToBuilder();
        //        builder.Add(task);
        //        scenario.TaskListEditor.Tasks = builder.ToImmutable();
        //    }
        //}

        //public static void AddSatelliteTasks(this ScenarioContainerViewModel scenario, IList<SatelliteTask> tasks)
        //{
        //    if (scenario?.TaskListEditor.Tasks != null && tasks != null)
        //    {
        //        var builder = scenario.TaskListEditor.Tasks.ToBuilder();
        //        builder.AddRange(tasks);
        //        scenario.TaskListEditor.Tasks = builder.ToImmutable();
        //    }
        //}

        //public static void AddSatelliteTask(this ScenarioContainerViewModel scenario, SatelliteTask task)
        //{
        //    if (scenario?.TaskListEditor.Tasks != null && task != null)
        //    {
        //        scenario.TaskListEditor.Tasks.Add(task);

        //        var list = scenario.TaskListEditor.Tasks;//.ToList();
        //        list.Add(task);
        //        scenario.TaskListEditor.Tasks = list;// new SourceList<SatelliteTask>(list);
        //    }
        //}

        public static void AddSatelliteTasks(this ScenarioContainerViewModel scenario, IList<SatelliteTask> tasks)
        {
            if (scenario?.TaskListEditor.Tasks != null && tasks != null)
            {
                //scenario.TaskListEditor.Tasks.AddRange(tasks);

                var list = scenario.TaskListEditor.Tasks;//.ToList();
                list.AddRange(tasks);
                scenario.TaskListEditor.Tasks = new ObservableCollection<SatelliteTask>(list);// new SourceList<SatelliteTask>(list);
            }
        }

        public static void AddGroundObjectList(this ScenarioContainerViewModel scenario, GroundObjectList list)
        {                
            scenario.GroundObjectList = list;            
        }
    }
}

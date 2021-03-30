using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels.Scene;
using System.Linq;
using Globe3DLight.ViewModels.Entities;
using Globe3DLight.ViewModels.Data;

namespace Globe3DLight.ViewModels.Editor
{
    public static class ProjectExtensions
    {

        public static BaseEntity AddEntity(this ProjectContainerViewModel project, BaseEntity entity)
        {
            if (project?.Scenarios != null && entity != null)
            {       
                var builder = project.CurrentScenario.Entities.ToBuilder();
                builder.Add(entity);
                project.CurrentScenario.Entities = builder.ToImmutable();
            }

            return entity;
        }

        public static IList<T> AddEntities<T>(this ProjectContainerViewModel project, IList<T> entities) where T : BaseEntity
        {
            if (project?.Scenarios != null && entities != null)
            {
                var builder = project.CurrentScenario.Entities.ToBuilder();
                builder.AddRange(entities);
                project.CurrentScenario.Entities = builder.ToImmutable();
            }

            return entities;
        }

        public static void AddScenario(this ProjectContainerViewModel project, ScenarioContainerViewModel scenario)
        {
            if (project?.Scenarios != null && scenario != null)
            {             
                var next = project.Scenarios.Add(scenario);           
                project.Scenarios = next;
            }
        }

        public static void AddChildFrame(this ProjectContainerViewModel project, LogicalViewModel node, LogicalViewModel child)
        {
            if (node != null && child != null)
            {
                node.AddChild(child);
            }
        }

        public static void RemoveScenario(this ProjectContainerViewModel project, ScenarioContainerViewModel scenario)
        {
            if (project?.Scenarios != null && scenario != null)
            {            
                var next = project.Scenarios.Remove(scenario);     
                project.Scenarios = next;
            }
        }

        //public static void RemoveLogicalNode(this IProjectContainer project, ILogicalTreeNode node)
        //{
        //    if(node != null)
        //    {
        //        var scenario = (IScenarioContainer)node.GetRoot().Owner;

        //        if (scenario != null)
        //        {
        //            var root = scenario.LogicalTreeNodeRoot.GetRoot();

        //            root.RemoveChild(node);
                        
        //            scenario.LogicalTreeNodeRoot = ImmutableArray.Create(root);                                       
        //        }
        //    }

        //    return;
        //}

        public static void RemoveLogicalNode(this ProjectContainerViewModel project, LogicalViewModel node)
        {
            if (node != null)
            {
                var root = node.GetRoot();

                if (root != null)
                {
                    root.RemoveChild(node);

                    project.CurrentScenario.LogicalRoot = ImmutableArray.Create(root);         
                }
            }

            return;
        }
    }
}

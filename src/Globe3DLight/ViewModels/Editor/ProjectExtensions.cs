using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.Containers;
using Globe3DLight.Scene;
using System.Linq;
using Globe3DLight.Entities;

namespace Globe3DLight.Editor
{
    public static class ProjectExtensions
    {

        public static void AddEntity(this ProjectContainer project, BaseEntity entity)
        {
            if (project?.Scenarios != null && entity != null)
            {       
                var builder = project.CurrentScenario.Entities.ToBuilder();
                builder.Add(entity);
                project.CurrentScenario.Entities = builder.ToImmutable();
            }            
        }

        public static void AddEntities(this ProjectContainer project, IEnumerable<BaseEntity> entities)
        {
            if (project?.Scenarios != null && entities != null)
            {
                var builder = project.CurrentScenario.Entities.ToBuilder();
                builder.AddRange(entities);
                project.CurrentScenario.Entities = builder.ToImmutable();
            }
        }

        public static void AddScenario(this ProjectContainer project, ScenarioContainer scenario)
        {
            if (project?.Scenarios != null && scenario != null)
            {             
                var next = project.Scenarios.Add(scenario);           
                project.Scenarios = next;
            }
        }

        public static void AddChildFrame(this ProjectContainer project, Logical node, Logical child)
        {
            if (node != null && child != null)
            {
                node.AddChild(child);
            }
        }

        public static void RemoveScenario(this ProjectContainer project, ScenarioContainer scenario)
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

        public static void RemoveLogicalNode(this ProjectContainer project, Logical node)
        {
            if (node != null)
            {
                var root = node.GetRoot();

                if (root != null)
                {
                    root.RemoveChild(node);

                    project.CurrentScenario.LogicalTreeNodeRoot = ImmutableArray.Create(root);         
                }
            }

            return;
        }


    }
}

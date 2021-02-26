using System;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.Containers;
using Globe3DLight.Scene;
using System.Linq;
using Globe3DLight.ScenarioObjects;

namespace Globe3DLight.Editor
{
    public static class ProjectExtensions
    {

        public static void AddScenarioObject(this IProjectContainer project, IScenarioObject scenarioObject)
        {
            if (project?.Scenarios != null && scenarioObject != null)
            {       
                var builder = project.CurrentScenario.ScenarioObjects.ToBuilder();
                builder.Add(scenarioObject);
                project.CurrentScenario.ScenarioObjects = builder.ToImmutable();
            }            
        }

        public static void AddScenario(this IProjectContainer project, IScenarioContainer scenario)
        {
            if (project?.Scenarios != null && scenario != null)
            {             
                var next = project.Scenarios.Add(scenario);           
                project.Scenarios = next;
            }
        }

        public static void AddChildFrame(this IProjectContainer project, ILogicalTreeNode node, ILogicalTreeNode child)
        {
            if (node != null && child != null)
            {
                node.AddChild(child);
            }
        }

        public static void RemoveScenario(this IProjectContainer project, IScenarioContainer scenario)
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

        public static void RemoveLogicalNode(this IProjectContainer project, ILogicalTreeNode node)
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

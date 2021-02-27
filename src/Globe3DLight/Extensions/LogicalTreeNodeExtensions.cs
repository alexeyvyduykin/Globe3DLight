using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Containers;
using System.Collections.Immutable;
using System.Linq;

namespace Globe3DLight
{
    public static class LogicalTreeNodeExtensions
    {
        public static void AddChild(this ILogicalTreeNode node, ILogicalTreeNode child) 
        {
            if (child != null)
            {
                var builder = node.Children.ToBuilder();

                child.Owner = node;

                child.State.Owner = node.State;

                builder.Add(child);

                node.Children = builder.ToImmutable();
            }
        }

        public static void RemoveChild(this ILogicalTreeNode node, ILogicalTreeNode child)
        {
            if (child != null)
            {
                if (node.Children.Contains(child) == true)
                {
                    var builder = node.Children.ToImmutableArray();

                    var nodes = builder.Remove(child);

                    node.Children = nodes;
                }
                else
                {
                    foreach (var item in node.Children)
                    {
                        item.RemoveChild(child);
                    }
                }
            }

        }

        public static ILogicalTreeNode GetRoot(this ILogicalTreeNode node)
        {
            var root = node;

            while (root?.Owner != null)
            {
                root = (ILogicalTreeNode)root.Owner;
            }

            return root;
        }

        public static ILogicalTreeNode GetRoot(this ImmutableArray<ILogicalTreeNode> nodes)
        {
            return nodes.FirstOrDefault().GetRoot();
        }


        public static ILogicalTreeNode Find(this ILogicalTreeNode node, ILogicalTreeNode find) 
        {
            if (node.Equals(find) == true)
                return node;

            foreach (var item in node.Children)
            {
                var res = item.Find(find);
                if (res != default)
                    return res;
            }

            return default;
        }

        public static string GetFullName(this ILogicalTreeNode node) 
        {
            if (node.Owner == null)
            {
                return node.State.Name;
            }

            return string.Format("{0}.{1}", ((ILogicalTreeNode)node.Owner).GetFullName(), node.State.Name);
        }
    }
}

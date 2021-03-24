using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.ViewModels.Containers;
using System.Collections.Immutable;
using System.Linq;
using Globe3DLight.ViewModels;

namespace Globe3DLight
{
    public static class LogicalTreeNodeExtensions
    {
        public static void AddChild(this LogicalViewModel node, ViewModelBase child) 
        {
            if (child != null)
            {
                var builder = node.Children.ToBuilder();

                child.Owner = node;

                //child.State.Owner = node.State;

                builder.Add(child);

                node.Children = builder.ToImmutable();
            }
        }

        public static void AddValue(this LogicalCollectionViewModel collection, LogicalViewModel value)
        {
            if (value != null)
            {
                var builder = collection.Values.ToBuilder();

                value.Owner = collection;

                //child.State.Owner = node.State;

                builder.Add(value);

                collection.Values = builder.ToImmutable();
            }
        }

        public static void RemoveChild(this LogicalViewModel node, LogicalViewModel child)
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
                    foreach (LogicalViewModel item in node.Children)
                    {
                        item.RemoveChild(child);
                    }
                }
            }

        }

        public static LogicalViewModel GetRoot(this LogicalViewModel node)
        {
            var root = node;

            while (root?.Owner != null)
            {
                root = (LogicalViewModel)root.Owner;
            }

            return root;
        }

        //public static ILogical GetRoot(this ImmutableArray<ILogical> nodes)
        //{
        //    return nodes.FirstOrDefault().GetRoot();
        //}


        public static LogicalViewModel Find(this LogicalViewModel node, LogicalViewModel find) 
        {
            if (node.Equals(find) == true)
                return node;

            foreach (LogicalViewModel item in node.Children)
            {
                var res = item.Find(find);
                if (res != default)
                    return res;
            }

            return default;
        }

        //public static string GetFullName(this ILogical node) 
        //{
        //    if (node.Owner == null)
        //    {
        //        return node.State.Name;
        //    }

        //    return string.Format("{0}.{1}", ((ILogical)node.Owner).GetFullName(), node.State.Name);
        //}
    }
}

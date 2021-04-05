#nullable enable
using System.Collections.Immutable;
using Globe3DLight.ViewModels;
using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels.Data;

namespace Globe3DLight
{
    public static class LogicalExtensions
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

            while (root.Owner is not null)
            {
                root = (LogicalViewModel)root.Owner;
            }

            return root;
        }

        public static LogicalViewModel? Find(this LogicalViewModel node, LogicalViewModel find)
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
    }
}

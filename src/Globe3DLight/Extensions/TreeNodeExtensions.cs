using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Containers;
using System.Collections.Immutable;
using System.Linq;


namespace Globe3DLight
{

    //public static class TreeNodeExtensions
    //{
    //    public static void AddChild<T>(this ITreeNode<T> node, ITreeNode<T> child) where T : IObservableObject
    //    {
    //        if (child != null)
    //        {
    //            var builder = node.Children.ToBuilder();

    //            child.Parent = node;

    //            child.Content.Owner = node.Content;

    //            builder.Add(child);

    //            node.Children = builder.ToImmutable();            
    //        }
    //    }

    //    public static void RemoveChild<T>(this ITreeNode<T> node, ITreeNode<T> child) where T : IObservableObject
    //    {
    //        if(child != null)
    //        {
    //            if (node.Children.Contains(child) == true)
    //            {
    //                var builder = node.Children.ToImmutableArray();

    //                var nodes = builder.Remove(child);

    //                node.Children = nodes;
    //            }
    //            else
    //            {
    //                foreach (var item in node.Children)
    //                {
    //                    item.RemoveChild(child);
    //                }
    //            }
    //        }

    //    }

    //    public static ITreeNode<T> GetRoot<T>(this ITreeNode<T> node) where T : IObservableObject
    //    {
    //        var root = node;

    //        while (root?.Parent != null)
    //        {
    //            root = root.Parent;
    //        }
           
    //        return root;
    //    }

    //    public static ITreeNode<T> GetRoot<T>(this ImmutableArray<ITreeNode<T>> nodes) where T : IObservableObject
    //    {
    //        return nodes.FirstOrDefault().GetRoot();
    //    }


    //    public static ITreeNode<T> Find<T>(this ITreeNode<T> node, ITreeNode<T> find) where T : IObservableObject
    //    {         
    //        if (node.Equals(find) == true)
    //            return node;

    //        foreach (var item in node.Children)
    //        {
    //            var res = item.Find(find);
    //            if (res != default)
    //                return res;
    //        }

    //        return default;
    //    }

    //    public static string GetFullName<T>(this ITreeNode<T> node) where T : IObservableObject 
    //    {
    //        if (node.Parent == null)
    //        {
    //            return node.Content.Name;
    //        }

    //        return string.Format("{0}.{1}", node.Parent.GetFullName(), node.Content.Name);
    //    }
    //}
}

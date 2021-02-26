using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Globe3DLight.Containers
{
    //public class TreeNode<T> : ObservableObject, ITreeNode<T> where T : IObservableObject
    //{
    //    private ITreeNode<T> _parent;
    //    private ImmutableArray<ITreeNode<T>> _children;
    //    private T _content;
    //    private bool _isExpanded = true;

    //    public bool IsExpanded 
    //    {
    //        get => _isExpanded; 
    //        set => Update(ref _isExpanded, value);
    //    }

    //    public new string Name
    //    {
    //        get => (this.Content != null) ? this.Content.Name : string.Empty;
    //        set
    //        {
    //            if(this.Content != null)
    //            {
    //                this.Content.Name = value;
    //            }
    //        }
    //    }


    //    public ITreeNode<T> Parent 
    //    {
    //        get => _parent; 
    //        set => Update(ref _parent, value); 
    //    }

    //    public ImmutableArray<ITreeNode<T>> Children
    //    {
    //        get => _children; 
    //        set => Update(ref _children, value); 
    //    }

    //    public T Content
    //    {
    //        get => _content; 
    //        set => Update(ref _content, value);
    //    }

    //    public override bool IsDirty()
    //    {
    //        var isDirty = base.IsDirty();

    //        if (Content != null)
    //        {
    //            isDirty |= Content.IsDirty();
    //        }

    //        foreach (var child in Children)
    //        {
    //            isDirty |= child.IsDirty();
    //        }

    //        return isDirty;
    //    }

    //    /// <inheritdoc/>
    //    public override void Invalidate()
    //    {
    //        base.Invalidate();

    //        Content?.Invalidate();

    //        foreach (var child in Children)
    //        {
    //            child.Invalidate();
    //        }
    //    }


    //    public override object Copy(IDictionary<object, object> shared)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}

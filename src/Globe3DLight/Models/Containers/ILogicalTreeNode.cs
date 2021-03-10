using System;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.Scene;
using Globe3DLight.Data;
using System.Collections.Generic;

namespace Globe3DLight.Containers
{
    //public interface ILogicalTreeNode : IObservableObject
    //{        
    //    ImmutableArray<ILogicalTreeNode> Children { get; set; }

    //    IState State { get; set; }
    //}

    public interface ILogical<T>
    {
        T State { get; set; }
    }

    public interface ILogical : IObservableObject, ILogical<IState>
    {
        ImmutableArray<IObservableObject> Children { get; set; }
    }

    public interface ILogicalCollection : IObservableObject//, ILogical<IEnumerable<IState>>
    {
        ImmutableArray<ILogical> Values { get; set; }
    }
}

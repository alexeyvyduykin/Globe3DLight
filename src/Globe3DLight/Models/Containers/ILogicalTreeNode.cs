using System;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.Scene;
using Globe3DLight.Data;


namespace Globe3DLight.Containers
{
    public interface ILogicalTreeNode : IObservableObject
    {        
        ImmutableArray<ILogicalTreeNode> Children { get; set; }

        IState State { get; set; }
    }
}

using System;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.Scene;
using Globe3DLight.Data;


namespace Globe3DLight.Containers
{
    //public interface ILogicalTreeNode : IObservableObject
    //{

    //}


    public interface ILogicalTreeNode : IObservableObject//ILogicalTreeNode
    {        
        ImmutableArray<ILogicalTreeNode> Children { get; set; }

        IData Data { get; set; }

      //  ILibrary<IDataProvider> DataProviderLibrary { get; set; }

      //  IDataProvider CurrentDataProvider { get; set; }

    }
}

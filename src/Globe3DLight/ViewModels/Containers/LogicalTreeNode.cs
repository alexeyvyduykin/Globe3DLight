using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Immutable;
using Globe3DLight.Scene;
using Globe3DLight.Data;


namespace Globe3DLight.Containers
{
    public class LogicalTreeNode : ObservableObject, ILogicalTreeNode
    {       
        private ImmutableArray<ILogicalTreeNode> _children;    
        private bool _isExpanded = true;
        private IData _data;
   //     private ILibrary<IDataProvider> _dataProviderLibrary;
   //     private IDataProvider _currentDataProvider;

        public bool IsExpanded
        {
            get => _isExpanded;
            set => Update(ref _isExpanded, value);
        }



        //public new string Name
        //{
        //    get => (this.Data != null) ? this.Data.Name : string.Empty;
        //    set
        //    {
        //        if (this.Data != null)
        //        {
        //            this.Data.Name = value;
        //        }
        //    }
        //}


        public ImmutableArray<ILogicalTreeNode> Children
        {
            get => _children;
            set => Update(ref _children, value);
        }

        public IData Data
        {
            get => _data;
            set => Update(ref _data, value);
        }

        //public ILibrary<IDataProvider> DataProviderLibrary 
        //{
        //    get => _dataProviderLibrary; 
        //    set => Update(ref _dataProviderLibrary, value);
        //}

        //public IDataProvider CurrentDataProvider 
        //{
        //    get => _currentDataProvider; 
        //    set => Update(ref _currentDataProvider, value); 
        //}

        public override bool IsDirty()
        {
            var isDirty = base.IsDirty();

            if (Data != null)
            {
                isDirty |= Data.IsDirty();
            }

            foreach (var child in Children)
            {
                isDirty |= child.IsDirty();
            }

            return isDirty;
        }

        /// <inheritdoc/>
        public override void Invalidate()
        {
            base.Invalidate();

            Data?.Invalidate();

            foreach (var child in Children)
            {
                child.Invalidate();
            }
        }



        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.ScenarioObjects;
using Globe3DLight.Containers;


namespace Globe3DLight.Data
{
    public interface IDataUpdater : IObservableObject
    {
        void Update(double t, ILogicalTreeNode logicalTreeNode);
    }


    public class DataUpdater : ObservableObject, IDataUpdater
    {
        private readonly IServiceProvider _serviceProvider;



        public DataUpdater(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }

        public void Update(double t, ILogicalTreeNode logicalTreeNode)
        {
            if(logicalTreeNode.State is IAnimator animator)
            {
                animator.Animate(t);
            }

            foreach (var item in logicalTreeNode.Children)
            {
                Update(t, item);
            }
        }

        
    }
}

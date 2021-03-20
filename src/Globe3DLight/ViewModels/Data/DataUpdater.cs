using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Containers;
using System.Linq;

namespace Globe3DLight.Data
{
    public interface IDataUpdater
    {
        void Update(double t, IObservableObject obj);
    }

    public class DataUpdater : IDataUpdater
    {
        public void Update(double t, IObservableObject obj)
        {
            if (obj is ILogical logical)
            {
                if (logical.State is IAnimator animator)
                {
                    animator.Animate(t);
                }

                foreach (var item in logical.Children)
                {
                    Update(t, item);
                }
            }
            else if (obj is ILogicalCollection collection)
            {
                var first = collection.Values.FirstOrDefault();

                if (first.State is IAnimator animator)
                {
                    foreach (IAnimator item in collection.Values.Select(s => s.State))
                    {
                        item.Animate(t);
                    }
                }
                // collection states not is animate
                //foreach (var item in collection)
                //{
                //    if (item.State is IAnimator animator)
                //    {
                //        animator.Animate(t);
                //    }
                //}
            }
        }        
    }
}

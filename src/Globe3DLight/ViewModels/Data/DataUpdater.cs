using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.ViewModels.Containers;
using System.Linq;
using Globe3DLight.Models.Data;

namespace Globe3DLight.ViewModels.Data
{
    public class DataUpdater : IDataUpdater
    {
        public void Update(double t, ViewModelBase obj)
        {
            if (obj is LogicalViewModel logical)
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
            else if (obj is LogicalCollectionViewModel collection)
            {
                var first = collection.Values.FirstOrDefault();

                if (first.State is IAnimator animator)
                {
                    foreach (var item in collection.Values.Select(s => s.State))
                    {
                        if (item is IAnimator a)
                        {
                            a.Animate(t);
                        }
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

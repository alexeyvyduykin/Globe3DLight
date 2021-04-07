using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Immutable;
using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels;

namespace Globe3DLight.ViewModels.Data
{
    //public class LogicalViewModel : BaseContainerViewModel
    //{
    //    private ImmutableArray<ViewModelBase> _children;       
       
    //    public ImmutableArray<ViewModelBase> Children
    //    {
    //        get => _children;
    //        set => RaiseAndSetIfChanged(ref _children, value);
    //    }

    //    public override bool IsDirty()
    //    {
    //        var isDirty = base.IsDirty();

    //        foreach (var child in Children)
    //        {
    //            isDirty |= child.IsDirty();
    //        }

    //        return isDirty;
    //    }

    //    public override void Invalidate()
    //    {
    //        base.Invalidate();

    //        foreach (var child in Children)
    //        {
    //            child.Invalidate();
    //        }
    //    }
    //}
}

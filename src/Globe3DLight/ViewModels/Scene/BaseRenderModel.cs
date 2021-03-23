using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.Renderer;
using GlmSharp;

namespace Globe3DLight.Scene
{
    public abstract class BaseRenderModel : ObservableObject
    {
        //private ImmutableArray<ISceneObject> _children;

        //public ImmutableArray<ISceneObject> Children
        //{
        //    get => _children; 
        //    set => Update(ref _children, value);
        //}

        //public dmat4 ModelMatrix { get; set; }


        public override bool IsDirty()
        {
            var isDirty = base.IsDirty();

            //foreach (var child in Children)
            //{
            //    isDirty |= child.IsDirty();
            //}

            //   isDirty |= State.IsDirty();
            //   isDirty |= Data.IsDirty();

            return isDirty;
        }
      
        public override void Invalidate()
        {
            base.Invalidate();

            //foreach (var child in Children)
            //{
            //    child.Invalidate();
            //}

            //    State.Invalidate();
            //    Data.Invalidate();
        }

      //  public abstract void DrawShape(object dc, IRenderContext renderer, ISceneState scene);

        public virtual bool Invalidate(IRenderContext renderer)
        {
            return false;
        }

    }
}

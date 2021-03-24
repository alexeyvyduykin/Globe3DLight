using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;
using Globe3DLight.Models.Renderer;
using GlmSharp;
using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.ViewModels.Scene;
using Globe3DLight.Models.Entities;
using Globe3DLight.Models.Scene;
using Globe3DLight.Models;

namespace Globe3DLight.ViewModels.Entities
{
    public class GroundObject : BaseEntity, IDrawable, ITargetable
    {
        private GroundObjectRenderModel _renderModel; 
        private FrameRenderModel _frameRenderModel;
        private LogicalViewModel _logical;

        public GroundObjectRenderModel RenderModel
        {
            get => _renderModel;
            set => RaiseAndSetIfChanged(ref _renderModel, value);
        }

        public FrameRenderModel FrameRenderModel
        {
            get => _frameRenderModel;
            set => RaiseAndSetIfChanged(ref _frameRenderModel, value);
        }

        public LogicalViewModel Logical
        {
            get => _logical;
            set => RaiseAndSetIfChanged(ref _logical, value);
        }

        public dmat4 InverseAbsoluteModel
        {
            get
            {
                if (_logical?.State is IFrameable)
                {
                    if (Logical.State is GroundObjectState groundObjectState)
                    {
                        var collection = Logical.Owner;
                        var parent = (LogicalViewModel)collection.Owner;
                        if (parent.State is EarthAnimator j2000Data)
                        {
                            var modelMatrix = j2000Data.ModelMatrix * groundObjectState.ModelMatrix;
                            return modelMatrix.Inverse;
                        }
                    }
                }

                return dmat4.Identity.Inverse;
            }
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                if (Logical.State is GroundObjectState groundObjectState)
                {
                    var collection = Logical.Owner;
                    var parent = (LogicalViewModel)collection.Owner;
                    if (parent.State is EarthAnimator j2000Data)
                    {
                        var m = j2000Data.ModelMatrix;

                        var matrix = m * groundObjectState.ModelMatrix;

                        renderer.DrawFrame(dc, FrameRenderModel, matrix, scene);

                        renderer.DrawGroundObject(dc, RenderModel, matrix, scene);
                    }
                }
            }
        }
   
        public bool Invalidate(IRenderContext renderer)
        {
            return false;
        }
    }
}

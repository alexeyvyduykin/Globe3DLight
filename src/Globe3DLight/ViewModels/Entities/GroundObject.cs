#nullable disable
using GlmSharp;
using Globe3DLight.Models;
using Globe3DLight.Models.Renderer;
using Globe3DLight.Models.Scene;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.ViewModels.Scene;

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
                if (_logical is IFrameable)
                {
                    if (Logical is GroundObjectState groundObjectState)
                    {
                        var collection = Logical.Owner;
                        var parent = (LogicalViewModel)collection.Owner;
                        if (parent is EarthAnimator j2000Data)
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
                if (Logical is GroundObjectState groundObjectState)
                {
                    var collection = Logical.Owner;
                    var parent = (LogicalViewModel)collection.Owner;
                    if (parent is EarthAnimator j2000Data)
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

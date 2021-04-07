#nullable disable
using GlmSharp;
using Globe3DLight.Models;
using Globe3DLight.Models.Renderer;
using Globe3DLight.Models.Scene;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.ViewModels.Scene;

namespace Globe3DLight.ViewModels.Entities
{
    public class GroundStation : BaseEntity, IDrawable, ITargetable
    {
        private RenderModel _renderModel;     
        private FrameViewModel _frame;

        public FrameViewModel Frame
        {
            get => _frame;
            set => RaiseAndSetIfChanged(ref _frame, value);
        }

        public RenderModel RenderModel
        {
            get => _renderModel;
            set => RaiseAndSetIfChanged(ref _renderModel, value);
        }

        public dmat4 InverseAbsoluteModel
        {
            get
            {
                if (Frame.State is IFrameable)
                {
                    if (Frame.State is GroundStationState groundStationData)
                    {
                        var collection = Frame.Parent;
                        var parent = collection.Parent;
                        if (parent.State is EarthAnimator j2000Data)
                        {
                            var modelMatrix = j2000Data.ModelMatrix * groundStationData.ModelMatrix;
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
                if (Frame.State is GroundStationState groundStationData)
                {
                    var collection = Frame.Parent;
                    var parent = collection.Parent;
                    if (parent.State is EarthAnimator j2000Data)
                    {
                        var m = j2000Data.ModelMatrix;

                        var groundStationModelMatrix = m * groundStationData.ModelMatrix;

                        renderer.DrawGroundStation(dc, RenderModel, groundStationModelMatrix, scene);
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

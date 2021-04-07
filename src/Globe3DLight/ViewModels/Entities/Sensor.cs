#nullable disable
using GlmSharp;
using Globe3DLight.Models;
using Globe3DLight.Models.Renderer;
using Globe3DLight.Models.Scene;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.ViewModels.Scene;

namespace Globe3DLight.ViewModels.Entities
{
    public class Sensor : BaseEntity, IDrawable
    {
        private SensorRenderModel _renderModel;        
        private FrameViewModel _frame;

        public FrameViewModel Frame
        {
            get => _frame;
            set => RaiseAndSetIfChanged(ref _frame, value);
        }

        public SensorRenderModel RenderModel
        {
            get => _renderModel;
            set => RaiseAndSetIfChanged(ref _renderModel, value);
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                if (Frame.State is SensorAnimator state)
                {
                    if (state.Enable == true)
                    {
                        RenderModel.Shoot = state.Shoot;
                        RenderModel.Scan = state.Scan;

                        renderer.DrawSensor(dc, RenderModel, dmat4.Identity, scene);
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

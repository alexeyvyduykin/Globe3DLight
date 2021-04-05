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
        private BaseState _logical;

        public SensorRenderModel RenderModel
        {
            get => _renderModel;
            set => RaiseAndSetIfChanged(ref _renderModel, value);
        }

        public BaseState Logical
        {
            get => _logical;
            set => RaiseAndSetIfChanged(ref _logical, value);
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                if (Logical is SensorAnimator sensorData)
                {
                    if (sensorData.Enable == true)
                    {
                        RenderModel.Shoot = sensorData.Shoot;
                        RenderModel.Scan = sensorData.Scan;

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

using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.Models.Renderer;
using Globe3DLight.ViewModels.Scene;
using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using System.Collections.Immutable;
using Globe3DLight.Models.Entities;
using Globe3DLight.Models.Scene;
using Globe3DLight.Models;

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

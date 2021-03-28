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
        private LogicalViewModel _logical;

        public SensorRenderModel RenderModel
        {
            get => _renderModel; 
            set => RaiseAndSetIfChanged(ref _renderModel, value); 
        }

        public LogicalViewModel Logical 
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
                        var orbitNode = Logical.Owner;
                        if (orbitNode is SatelliteAnimator satelliteState)
                        {
                            RenderModel.Shoot = sensorData.Shoot;
                            RenderModel.Scan = sensorData.Scan;

                            renderer.DrawSensor(dc, RenderModel, dmat4.Identity/* satelliteState.*//*Absolute*//*ModelMatrix*/, scene);
                        }
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

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
        private int _scanCounter = 0;
        private dvec3 _pBegin0, _pBegin1;

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
                if (Logical.State is SensorAnimator sensorData)
                {
                    if (sensorData.Enable == true)
                    {
                        var rotationNode = (LogicalViewModel)Logical.Owner;
                        if (rotationNode.State is RotationAnimator /*rotationData*/)
                        {
                            var orbitNode = (LogicalViewModel)rotationNode.Owner;
                            if (orbitNode.State is SatelliteAnimator satelliteState)
                            {
                                //   double r = orbitData.Position.Length;
                                //   var orbitRadius = r * scene.WorldScale;

                                //   dmat4 translate = dmat4.Translate(glm.Normalized(orbitData.Position) * orbitRadius);

                                var orbitModelMatrix = satelliteState.ModelMatrix /** scene.WorldScale*/;// translate * orbitData.mtxRot;//.Inverse;     

                                var satelliteModelMatrix = orbitModelMatrix;// * rotationData.RotationMatrix;
                                          
                                RenderModel.Shoot = new Shoot() 
                                {                                  
                                    p0 = sensorData.Shoot.p0,
                                    p1 = sensorData.Shoot.p1,
                                    p2 = sensorData.Shoot.p2,
                                    p3 = sensorData.Shoot.p3,
                                };
     
                                dvec3 pEnd0 = new dvec3(satelliteModelMatrix * new dvec4(RenderModel.Shoot.p0, 1.0));
                                dvec3 pEnd1 = new dvec3(satelliteModelMatrix * new dvec4(RenderModel.Shoot.p1, 1.0));

                                if (_scanCounter == 0)
                                {
                                    _pBegin0 = pEnd0;
                                    _pBegin1 = pEnd1;                                   
                                }

                                _scanCounter++;

                                RenderModel.Scan = new Scan() { p0 = _pBegin1, p1 = _pBegin0, p2 = pEnd0, p3 = pEnd1 };


                                renderer.DrawSensor(dc, RenderModel, satelliteModelMatrix, scene);
                            }
                        }
                    }
                    else
                    {
                        _scanCounter = 0;
                    }
                }
            }
            else
            {
                _scanCounter = 0;
            }
        }

        public bool Invalidate(IRenderContext renderer)
        {
            return false;
        }
    }
}

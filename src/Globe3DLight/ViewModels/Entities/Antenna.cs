using Globe3DLight.ViewModels.Containers;
using Globe3DLight.Models.Data;
using Globe3DLight.Models.Renderer;
using Globe3DLight.ViewModels.Scene;
using Globe3DLight.Models.Scene;
using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using System.Collections.Immutable;
using System.Linq;
using Globe3DLight.Models.Entities;
using Globe3DLight.Models;
using Globe3DLight.ViewModels.Data;

namespace Globe3DLight.ViewModels.Entities
{
    public class Antenna : BaseEntity, IDrawable, IAssetable
    {
        private AntennaRenderModel _renderModel; 
        private FrameRenderModel _frameRenderModel;

        private LogicalViewModel _logical; 
        private ImmutableArray<BaseEntity> _assets;
        
        public ImmutableArray<BaseEntity> Assets
        {
            get => _assets;
            set => RaiseAndSetIfChanged(ref _assets, value);
        }

        public AntennaRenderModel RenderModel
        {
            get => _renderModel;
            set => RaiseAndSetIfChanged(ref _renderModel, value);
        }

        public LogicalViewModel Logical
        {
            get => _logical;
            set => RaiseAndSetIfChanged(ref _logical, value);
        }
        public FrameRenderModel FrameRenderModel
        {
            get => _frameRenderModel;
            set => RaiseAndSetIfChanged(ref _frameRenderModel, value);
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                if (Logical is AntennaAnimator antennaData)
                {
                    dvec3 targetPosition = default;
                    bool enable = antennaData.Enable;

                    if (enable == true)
                    {
                        var target = antennaData.Target;

                        foreach (var item in Assets)
                        {
                            if (item is GroundStation groundStation)
                            {
                                if (groundStation.Name.Equals(target) == true)
                                {
                                    if (groundStation.Logical is GroundStationState groundStationData)
                                    {
                                        var collection = (LogicalCollectionViewModel)groundStation.Logical.Owner;
                                        var j2000Node = (LogicalViewModel)collection.Owner;
                                        if (j2000Node is EarthAnimator j2000Data)
                                        {
                                            targetPosition = new dvec3(j2000Data.ModelMatrix * new dvec4(groundStationData.Position, 1.0));
                                        }
                                    }
                                }
                            }
                        }

                        foreach (var item in Assets)
                        {
                            if (item is Retranslator retranslator)
                            {
                                if (retranslator.Name.Equals(target) == true)
                                {
                                    if (retranslator.Logical is RetranslatorAnimator retranslatorData)
                                    {
                                        targetPosition = retranslatorData.Position;
                                    }
                                }
                            }
                        }
                    }

                    var rotationNode = (LogicalViewModel)Logical.Owner;
                    if (rotationNode is RotationAnimator rotationData)
                    {
                        var orbitNode = (LogicalViewModel)rotationNode.Owner;
                        if (orbitNode is SatelliteAnimator satelliteState)
                        {
                            var attach = RenderModel.AttachPosition;

                            //   double r = orbitData.Position.Length;
                            //   var orbitRadius = r * scene.WorldScale;

                            //   dmat4 translate = dmat4.Translate(glm.Normalized(orbitData.Position) * orbitRadius);

                            var orbitModelMatrix = satelliteState.ModelMatrix;// translate * orbitData.mtxRot;//.Inverse;     

                            var satelliteModelMatrix = orbitModelMatrix * rotationData.ModelMatrix;// RotationMatrix;

                            var antennaModelMatrix = satelliteModelMatrix * dmat4.Translate(attach);

                            renderer.DrawFrame(dc, FrameRenderModel, antennaModelMatrix, scene);

                            if (enable == true)
                            {
                                RenderModel.TargetPostion = targetPosition;

                                renderer.DrawAntenna(dc, RenderModel, antennaModelMatrix, scene);
                            }
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

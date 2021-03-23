using Globe3DLight.Containers;
using Globe3DLight.Data;
using Globe3DLight.Renderer;
using Globe3DLight.Scene;
using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using System.Collections.Immutable;
using System.Linq;


namespace Globe3DLight.Entities
{
    public class Antenna : BaseEntity, IDrawable, IAssetable
    {
        private AntennaRenderModel _renderModel; 
        private FrameRenderModel _frameRenderModel;

        private Logical _logical; 
        private ImmutableArray<BaseEntity> _assets;
        
        public ImmutableArray<BaseEntity> Assets
        {
            get => _assets;
            set => Update(ref _assets, value);
        }

        public AntennaRenderModel RenderModel
        {
            get => _renderModel;
            set => Update(ref _renderModel, value);
        }

        public Logical Logical
        {
            get => _logical;
            set => Update(ref _logical, value);
        }
        public FrameRenderModel FrameRenderModel
        {
            get => _frameRenderModel;
            set => Update(ref _frameRenderModel, value);
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                if (Logical.State is IAntennaState antennaData)
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
                                    if (groundStation.Logical.State is IGroundStationState groundStationData)
                                    {
                                        var collection = (LogicalCollection)groundStation.Logical.Owner;
                                        var j2000Node = (Logical)collection.Owner;
                                        if (j2000Node.State is IJ2000State j2000Data)
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
                                    if (retranslator.Logical.State is IRetranslatorState retranslatorData)
                                    {
                                        targetPosition = retranslatorData.Position;
                                    }
                                }
                            }
                        }
                    }

                    var rotationNode = (Logical)Logical.Owner;
                    if (rotationNode.State is IRotationState rotationData)
                    {
                        var orbitNode = (Logical)rotationNode.Owner;
                        if (orbitNode.State is ISatelliteState satelliteState)
                        {
                            var attach = RenderModel.AttachPosition;

                            //   double r = orbitData.Position.Length;
                            //   var orbitRadius = r * scene.WorldScale;

                            //   dmat4 translate = dmat4.Translate(glm.Normalized(orbitData.Position) * orbitRadius);

                            var orbitModelMatrix = satelliteState.ModelMatrix;// translate * orbitData.mtxRot;//.Inverse;     

                            var satelliteModelMatrix = orbitModelMatrix * rotationData.RotationMatrix;

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

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


namespace Globe3DLight.ScenarioObjects
{
    public class Antenna : BaseScenarioObject, IAntenna, IDrawable
    {
        private IAntennaRenderModel _renderModel; 
        private IFrameRenderModel _frameRenderModel;

        private ILogicalTreeNode _logicalTreeNode; 
        private ImmutableArray<IScenarioObject> _assets;
        
        public ImmutableArray<IScenarioObject> Assets
        {
            get => _assets;
            set => Update(ref _assets, value);
        }

        public IAntennaRenderModel RenderModel
        {
            get => _renderModel;
            set => Update(ref _renderModel, value);
        }

        public ILogicalTreeNode LogicalTreeNode
        {
            get => _logicalTreeNode;
            set => Update(ref _logicalTreeNode, value);
        }
        public IFrameRenderModel FrameRenderModel
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
                if (LogicalTreeNode.State is IAntennaState antennaData)
                {
                    //        if (antennaData.Enable == true)
                    //       {

                    dvec3 targetPosition = default;
                    bool enable = antennaData.Enable;

                    if (enable == true)
                    {
                        var target = antennaData.Target;

                        foreach (var item in Assets)
                        {
                            if(item is IGroundStation groundStation)
                            {
                                if(groundStation.UniqueName.Name.Equals(target) == true)
                                {                        
                                    if (groundStation.LogicalTreeNode.State is IGroundStationState groundStationData)
                                    {
                                        var j2000Node = (ILogicalTreeNode)groundStation.LogicalTreeNode.Owner;
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
                            if (item is IRetranslator retranslator)
                            {
                                if (retranslator.UniqueName.Name.Equals(target) == true)
                                {
                                    if (retranslator.LogicalTreeNode.State is IRetranslatorState retranslatorData)
                                    {
                                        targetPosition = retranslatorData.Position;                               
                                    }
                                }
                            }
                        }
                    }


                        var rotationNode = (ILogicalTreeNode)LogicalTreeNode.Owner;
                        if (rotationNode.State is IRotationState rotationData)
                        {
                            var orbitNode = (ILogicalTreeNode)rotationNode.Owner;
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

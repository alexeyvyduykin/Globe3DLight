using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Containers;
using Globe3DLight.Renderer;
using Globe3DLight.Scene;
using Globe3DLight.Data;
using System.Collections.Immutable;

namespace Globe3DLight.Entities
{
    public class EntityList : BaseEntity, IEntityList//IGroundStationList
    {
        //private IGroundStationRenderModel _renderModel;
        private ImmutableArray<IEntity> _values;

        //public IGroundStationRenderModel RenderModel
        //{
        //    get => _renderModel;
        //    set => Update(ref _renderModel, value);
        //}
        
        public ImmutableArray<IEntity> Values 
        {
            get => _values; 
            set => Update(ref _values, value); 
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                var proto = Values.FirstOrDefault();

                if (proto is IGroundObject groundObject)
                {
                    if (groundObject.IsVisible == true)
                    {
                        if (groundObject.Logical.State is IGroundObjectState groundObjectState)
                        {
                            var collection = (ILogicalCollection)groundObject.Logical.Owner;
                            var parent = (ILogical)collection.Owner;
                            if (parent.State is IJ2000State j2000Data)
                            {
                                var m = j2000Data.ModelMatrix;

                                //foreach (var item in collection.Values)
                                //{
                                //    var matrix = m * ((IGroundObjectState)item.State).ModelMatrix;

                                //    renderer.DrawGroundObject(dc, groundObject.RenderModel, matrix, scene);
                                //}

                                var matrices = collection.Values.Select(s => m * ((IGroundObjectState)s.State).ModelMatrix);
                                renderer.DrawFrameList(dc, groundObject.FrameRenderModel, matrices, scene);
                                renderer.DrawGroundObjectList(dc, groundObject.RenderModel, matrices, scene);
                            }
                        }
                    }
                }
                else if(proto is IGroundStation groundStation)
                {
                    if (groundStation.IsVisible == true)
                    {
                        if (groundStation.Logical.State is IGroundStationState groundStationData)
                        {
                            var collection = (ILogicalCollection)groundStation.Logical.Owner;
                            var parent = (ILogical)collection.Owner;
                            if (parent.State is IJ2000State j2000Data)
                            {
                                var m = j2000Data.ModelMatrix;

                                foreach (var item in collection.Values)
                                {
                                    var matrix = m * ((IGroundStationState)item.State).ModelMatrix;

                                    renderer.DrawFrame(dc, groundStation.FrameRenderModel, matrix, scene);

                                    renderer.DrawGroundStation(dc, groundStation.RenderModel, matrix, scene);
                                }
                            }
                        }
                    }
                }
                else if (proto is IRetranslator retranslator)
                {
                    if (retranslator.IsVisible == true)
                    {
                        if (retranslator.Logical.State is IRetranslatorState retranslatorData)
                        {
                            var collection = (ILogicalCollection)retranslator.Logical.Owner;

                            foreach (var item in collection.Values)
                            {
                                var matrix = ((IRetranslatorState)item.State).ModelMatrix;

                                renderer.DrawRetranslator(dc, retranslator.RenderModel, matrix, scene);
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
        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}

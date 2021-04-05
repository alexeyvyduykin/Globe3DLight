#nullable disable
using System.Collections.Immutable;
using System.Linq;
using Globe3DLight.Models;
using Globe3DLight.Models.Renderer;
using Globe3DLight.Models.Scene;
using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels.Data;

namespace Globe3DLight.ViewModels.Entities
{
    public class EntityList : BaseEntity, IDrawable, Globe3DLight.Models.Entities.ICollection<BaseEntity>
    {
        //private IGroundStationRenderModel _renderModel;
        private ImmutableArray<BaseEntity> _values;

        //public IGroundStationRenderModel RenderModel
        //{
        //    get => _renderModel;
        //    set => Update(ref _renderModel, value);
        //}

        public ImmutableArray<BaseEntity> Values
        {
            get => _values;
            set => RaiseAndSetIfChanged(ref _values, value);
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                var proto = Values.FirstOrDefault();

                if (proto is GroundObject groundObject)
                {
                    if (groundObject.IsVisible == true)
                    {
                        if (groundObject.Logical is GroundObjectState groundObjectState)
                        {
                            var collection = (LogicalCollectionViewModel)groundObject.Logical.Owner;
                            var parent = (LogicalViewModel)collection.Owner;
                            if (parent is EarthAnimator j2000Data)
                            {
                                var m = j2000Data.ModelMatrix;

                                //foreach (var item in collection.Values)
                                //{
                                //    var matrix = m * ((IGroundObjectState)item.State).ModelMatrix;

                                //    renderer.DrawGroundObject(dc, groundObject.RenderModel, matrix, scene);
                                //}

                                var matrices = collection.Values.Select(s => m * ((GroundObjectState)s).ModelMatrix);
                                renderer.DrawFrameList(dc, groundObject.FrameRenderModel, matrices, scene);
                                renderer.DrawGroundObjectList(dc, groundObject.RenderModel, matrices, scene);
                            }
                        }
                    }
                }
                else if (proto is GroundStation groundStation)
                {
                    if (groundStation.IsVisible == true)
                    {
                        if (groundStation.Logical is GroundStationState groundStationData)
                        {
                            var collection = (LogicalCollectionViewModel)groundStation.Logical.Owner;
                            var parent = (LogicalViewModel)collection.Owner;
                            if (parent is EarthAnimator j2000Data)
                            {
                                var m = j2000Data.ModelMatrix;

                                foreach (var item in collection.Values)
                                {
                                    var matrix = m * ((GroundStationState)item).ModelMatrix;

                                    renderer.DrawFrame(dc, groundStation.RenderModel.Frame, matrix, scene);

                                    renderer.DrawGroundStation(dc, groundStation.RenderModel, matrix, scene);
                                }
                            }
                        }
                    }
                }
                else if (proto is Retranslator retranslator)
                {
                    if (retranslator.IsVisible == true)
                    {
                        if (retranslator.Logical is RetranslatorAnimator retranslatorData)
                        {
                            var collection = (LogicalCollectionViewModel)retranslator.Logical.Owner;

                            foreach (var item in collection.Values)
                            {
                                var matrix = ((RetranslatorAnimator)item).ModelMatrix;

                                renderer.DrawFrame(dc, retranslator.RenderModel.Frame, matrix, scene);
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
    }
}

#nullable disable
using System.Collections.Immutable;
using System.Linq;
using Globe3DLight.Models;
using Globe3DLight.Models.Renderer;
using Globe3DLight.Models.Scene;

namespace Globe3DLight.ViewModels.Entities
{
    public class EntityList : BaseEntity, IDrawable, Globe3DLight.Models.Entities.ICollection<BaseEntity>
    {
        private ImmutableArray<BaseEntity> _values;

        public ImmutableArray<BaseEntity> Values
        {
            get => _values;
            set => RaiseAndSetIfChanged(ref _values, value);
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                var first = Values.FirstOrDefault();

                if (first is GroundObject groundObject)
                {
                    if (groundObject.IsVisible == true)
                    {
                        var collection = groundObject.Frame.Parent;
                        var matrices = collection.Children.Select(s => s.State.AbsoluteModelMatrix);
                        renderer.DrawGroundObjectList(dc, groundObject.RenderModel, matrices, scene);
                    }
                }
                else if (first is GroundStation groundStation)
                {
                    if (groundStation.IsVisible == true)
                    {
                        var collection = groundStation.Frame.Parent;
                        foreach (var item in collection.Children)
                        {
                            var matrix = item.State.AbsoluteModelMatrix;
                            renderer.DrawGroundStation(dc, groundStation.RenderModel, matrix, scene);
                        }
                    }
                }
                else if (first is Retranslator retranslator)
                {
                    if (retranslator.IsVisible == true)
                    {
                        var collection = retranslator.Frame.Parent;
                        foreach (var item in collection.Children)
                        {
                            var matrix = item.State.ModelMatrix;
                            renderer.DrawRetranslator(dc, retranslator.RenderModel, matrix, scene);
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

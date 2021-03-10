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

namespace Globe3DLight.ScenarioObjects
{
    public class ScenarioObjectList : BaseScenarioObject, IScenarioObjectList//IGroundStationList
    {
        //private IGroundStationRenderModel _renderModel;
        private ImmutableArray<IScenarioObject> _values;

        //public IGroundStationRenderModel RenderModel
        //{
        //    get => _renderModel;
        //    set => Update(ref _renderModel, value);
        //}
        
        public ImmutableArray<IScenarioObject> Values 
        {
            get => _values; 
            set => Update(ref _values, value); 
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                var proto = Values.FirstOrDefault();

                if (proto is IDrawableCollection obj)
                {
                    obj.DrawShapeCollection(dc, renderer, scene);
                }
                else
                {
                    foreach (IDrawable item in Values)
                    {
                        //item.RenderModel = RenderModel;
                        item.DrawShape(dc, renderer, scene);

                        //renderer.DrawGroundObject(dc, RenderModel, matrix, scene);
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

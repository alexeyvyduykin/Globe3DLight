using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Models.Renderer;
using Globe3DLight.ViewModels.Containers;

namespace Globe3DLight.ViewModels.Renderer.Presenters
{
    public class EditorPresenter : IContainerPresenter
    {
        public void Render(object dc, IRenderContext renderer, ScenarioContainerViewModel container)
        {
            renderer.DrawScenario(dc, container);

            //if (container.WorkingLayer != null)
            //{
            //    renderer.DrawLayer(dc, container.WorkingLayer);
            //}

            //if (container.HelperLayer != null)
            //{
            //    renderer.DrawLayer(dc, container.HelperLayer);
            //}
        }
    }
}

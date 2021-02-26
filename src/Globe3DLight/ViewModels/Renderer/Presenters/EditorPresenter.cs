using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Renderer;
using Globe3DLight.Containers;

namespace Globe3DLight.Renderer.Presenters
{
    public class EditorPresenter : IContainerPresenter
    {
        /// <inheritdoc/>
        public void Render(object dc, IRenderContext renderer, IScenarioContainer container)
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

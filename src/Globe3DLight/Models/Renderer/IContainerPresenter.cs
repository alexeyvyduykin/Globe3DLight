using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Renderer;
using Globe3DLight.Containers;


namespace Globe3DLight.Renderer
{
    public interface IContainerPresenter
    {
        void Render(object dc, IRenderContext renderer, ScenarioContainer container);
    }
}

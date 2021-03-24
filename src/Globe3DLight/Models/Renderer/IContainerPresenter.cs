using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Renderer;
using Globe3DLight.ViewModels.Containers;

namespace Globe3DLight.Models.Renderer
{
    public interface IContainerPresenter
    {
        void Render(object dc, IRenderContext renderer, ScenarioContainerViewModel container);
    }
}

using Globe3DLight.ViewModels.Containers;
using Globe3DLight.Models.Renderer;
using Globe3DLight.ViewModels.Scene;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.Models.Entities;
using Globe3DLight.Models.Scene;
using Globe3DLight.Models;

namespace Globe3DLight.ViewModels.Entities
{
    public class Spacebox : BaseEntity, IDrawable
    {
        private SpaceboxRenderModel _renderModel;
        private LogicalViewModel _logical;

        public SpaceboxRenderModel RenderModel
        {
            get => _renderModel; 
            set => RaiseAndSetIfChanged(ref _renderModel, value);
        }

        public LogicalViewModel Logical
        {
            get => _logical;
            set => RaiseAndSetIfChanged(ref _logical, value);
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                if (Logical.State is FrameState frameData)
                {
                    renderer.DrawSpacebox(dc, RenderModel, frameData.ModelMatrix, scene);
                }
            }
        }

        public bool Invalidate(IRenderContext renderer)
        {
            return false;
        }
    }
}

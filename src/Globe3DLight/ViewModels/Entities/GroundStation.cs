﻿using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.ViewModels.Scene;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Globe3DLight.Models.Renderer;
using GlmSharp;
using Globe3DLight.Models;
using Globe3DLight.Models.Scene;


namespace Globe3DLight.ViewModels.Entities
{
    public class GroundStation : BaseEntity, IDrawable, ITargetable
    { 
        private GroundStationRenderModel _renderModel; 
        private FrameRenderModel _frameRenderModel;
        private BaseState _logical;
      
        public GroundStationRenderModel RenderModel
        {
            get => _renderModel;
            set => RaiseAndSetIfChanged(ref _renderModel, value);
        }

        public FrameRenderModel FrameRenderModel
        {
            get => _frameRenderModel;
            set => RaiseAndSetIfChanged(ref _frameRenderModel, value);
        }

        public BaseState Logical
        {
            get => _logical;
            set => RaiseAndSetIfChanged(ref _logical, value);
        }
        public dmat4 InverseAbsoluteModel
        {
            get
            {
                if (_logical is IFrameable)
                {
                    if (Logical is GroundStationState groundStationData)
                    {
                        var collection = Logical.Owner;
                        var parent = (LogicalViewModel)collection.Owner;
                        if (parent is EarthAnimator j2000Data)
                        {                      
                            var modelMatrix = j2000Data.ModelMatrix * groundStationData.ModelMatrix;
                            return modelMatrix.Inverse;
                        }
                    }                   
                }

                return dmat4.Identity.Inverse;
            }
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                if (Logical is GroundStationState groundStationData)
                {
                    var collection = Logical.Owner;
                    var parent = (LogicalViewModel)collection.Owner;
                    if (parent is EarthAnimator j2000Data)
                    {
                        var m = j2000Data.ModelMatrix;

                        var groundStationModelMatrix = m * groundStationData.ModelMatrix;

                        renderer.DrawFrame(dc, FrameRenderModel, groundStationModelMatrix, scene);

                        renderer.DrawGroundStation(dc, RenderModel, groundStationModelMatrix, scene);
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

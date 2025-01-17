﻿using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Models.Scene;
using GlmSharp;

namespace Globe3DLight.Models.Renderer
{
    public interface IDrawNode : IDisposable
    {
        //  IShapeStyle Style { get; set; }
        //   bool ScaleThickness { get; set; }
        //   bool ScaleSize { get; set; }

        void UpdateGeometry();

        void UpdateStyle();

        void Draw(object dc, dmat4 modelMatrix, ISceneState scene);

        void Draw(object dc, IEnumerable<dmat4> modelMatrices, ISceneState scene);

        void OnDraw(object dc, dmat4 modelMatrix, ISceneState scene);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Scene;
using GlmSharp;

namespace Globe3DLight.Renderer
{
    public interface IDrawNode : IDisposable
    {
        //  IShapeStyle Style { get; set; }
        //   bool ScaleThickness { get; set; }
        //   bool ScaleSize { get; set; }


        void UpdateGeometry();
        void UpdateStyle();
        void Draw(object dc, dmat4 modelMatrix, ISceneState scene);
        void OnDraw(object dc, dmat4 modelMatrix, ISceneState scene);
    }
}

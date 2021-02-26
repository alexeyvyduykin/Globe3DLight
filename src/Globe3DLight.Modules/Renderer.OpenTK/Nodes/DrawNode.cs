using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Renderer;
using GlmSharp;
using Globe3DLight.Scene;

namespace Globe3DLight.Renderer.OpenTK
{
    internal abstract class DrawNode : IDrawNode
    {
        //  public IShapeStyle Style { get; set; }
        //  public bool ScaleThickness { get; set; }
        //  public bool ScaleSize { get; set; }
        //   public AM.IBrush Fill { get; set; }
        //   public AM.IPen Stroke { get; set; }
        //   public A.Point Center { get; set; }

        public DrawNode()
        {

        }

        public abstract void UpdateGeometry();

        public virtual void UpdateStyle()
        {
            //     Fill = AvaloniaDrawUtil.ToBrush(Style.Fill);
            //     Stroke = AvaloniaDrawUtil.ToPen(Style, Style.Thickness);
        }


        public virtual void Draw(object dc, dmat4 modelMatrix, ISceneState scene)
        {
            //    var scale = ScaleSize ? 1.0 / zoom : 1.0;
            //    var translateX = 0.0 - (Center.X * scale) + Center.X;
            //    var translateY = 0.0 - (Center.Y * scale) + Center.Y;

            //   double thickness = Style.Thickness;

            //   if (ScaleThickness)
            //   {
            //       thickness /= zoom;
            //   }

            //   if (scale != 1.0)
            //   {
            //       thickness /= scale;
            //   }

            //   if (Stroke.Thickness != thickness)
            //   {
            //       Stroke = AvaloniaDrawUtil.ToPen(Style, thickness);
            //   }

            //     var context = dc as AM.DrawingContext;
            //     var translateDisposable = scale != 1.0 ? context.PushPreTransform(AME.MatrixHelper.Translate(translateX, translateY)) : default(IDisposable);
            //     var scaleDisposable = scale != 1.0 ? context.PushPreTransform(AME.MatrixHelper.Scale(scale, scale)) : default(IDisposable);

            OnDraw(dc, modelMatrix, scene);

            //    scaleDisposable?.Dispose();
            //    translateDisposable?.Dispose();
        }

        public abstract void OnDraw(object dc, dmat4 modelMatrix, ISceneState scene);

        public virtual void Dispose()
        {
        }
    }
}

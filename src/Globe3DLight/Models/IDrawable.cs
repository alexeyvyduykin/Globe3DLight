using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Models.Renderer;
using Globe3DLight.Models.Scene;

namespace Globe3DLight.Models
{
    public interface IDrawable
    {
        /// <summary>
        /// Get or sets shape drawing style.
        /// </summary>
        //IShapeStyle Style { get; set; }

        /// <summary>
        /// Gets or sets flag indicating whether shape is stroked.
        /// </summary>
        //bool IsStroked { get; set; }

        /// <summary>
        /// Gets or sets flag indicating whether shape is filled.
        /// </summary>
        //bool IsFilled { get; set; }

        /// <summary>
        /// Draws shape using current <see cref="IShapeRenderer"/>.
        /// </summary>
        /// <param name="dc">The generic drawing context object.</param>
        /// <param name="renderer">The generic renderer object used to draw shape.</param>
        void DrawShape(object dc, IRenderContext renderer, ISceneState scene);

        /// <summary>
        /// Draws points using current <see cref="IShapeRenderer"/>.
        /// </summary>
        /// <param name="dc">The generic drawing context object.</param>
        /// <param name="renderer">The generic renderer object used to draw points.</param>
        //void DrawPoints(object dc, IShapeRenderer renderer);

        /// <summary>
        /// Invalidates shape renderer cache.
        /// </summary>
        /// <param name="renderer">The generic renderer object used to draw shape.</param>
        /// <returns>Returns true if shape was invalidated; otherwise, returns false.</returns>
        bool Invalidate(IRenderContext renderer);
    }
}

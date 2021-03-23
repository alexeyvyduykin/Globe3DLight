using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Renderer;
using Avalonia;
using Avalonia.Data;

namespace Globe3DLight.Modules.Renderer
{
    public class RendererOptions
    {
        /// <summary>
        /// Renderer options attached property.
        /// </summary>
        public static readonly AttachedProperty<IRenderContext> RendererProperty =
            AvaloniaProperty.RegisterAttached<RendererOptions, AvaloniaObject, IRenderContext>("Renderer", null, true, BindingMode.TwoWay);

        /// <summary>
        /// Gets renderer attached property.
        /// </summary>
        /// <param name="obj">The avalonia object.</param>
        /// <returns>The shape renderer property.</returns>
        public static IRenderContext GetRenderer(AvaloniaObject obj)
        {
            return obj.GetValue(RendererProperty);
        }

        /// <summary>
        /// Sets renderer attached property.
        /// </summary>
        /// <param name="obj">The avalonia object.</param>
        /// <param name="value">The shape render value.</param>
        public static void SetRenderer(AvaloniaObject obj, IRenderContext value)
        {
            obj.SetValue(RendererProperty, value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.ViewModels.Renderer;

namespace Globe3DLight.Renderer.OpenTK
{
    public class OpenTKRenderer : NodeRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvaloniaRenderer"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public OpenTKRenderer(IServiceProvider serviceProvider)
            : base(serviceProvider, new OpenTKDrawNodeFactory())
        {
        }
    }
}

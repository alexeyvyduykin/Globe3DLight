using System;
using System.Collections.Generic;
using System.Text;



namespace Globe3DLight.Renderer.OpenTK
{
    public class OpenTKRenderer : Globe3DLight.Renderer.NodeRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvaloniaRenderer"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public OpenTKRenderer(IServiceProvider serviceProvider)
            : base(serviceProvider, new OpenTKDrawNodeFactory())
        {
        }

        /// <inheritdoc/>
        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}

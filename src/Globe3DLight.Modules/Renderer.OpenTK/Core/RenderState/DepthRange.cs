using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class DepthRange
    {
        public DepthRange()
        {
            Near = 0.0;
            Far = 1.0;
        }

        public double Near { get; set; }
        public double Far { get; set; }
    }
}

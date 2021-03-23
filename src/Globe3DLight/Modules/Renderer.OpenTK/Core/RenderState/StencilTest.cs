using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class StencilTest
    {
        public StencilTest()
        {
            Enabled = false;
            FrontFace = new StencilTestFace();
            BackFace = new StencilTestFace();
        }

        public bool Enabled { get; set; }
        public StencilTestFace FrontFace { get; set; }
        public StencilTestFace BackFace { get; set; }
    }
}

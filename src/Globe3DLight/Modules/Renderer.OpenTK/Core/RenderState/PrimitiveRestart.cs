using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class PrimitiveRestart
    {
        public PrimitiveRestart()
        {
            Enabled = false;
            Index = 0;
        }

        public bool Enabled { get; set; }
        public int Index { get; set; }
    }
}

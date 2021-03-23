using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class FragmentOutputs
    {
        public FragmentOutputs(int program)
        {
            this.program = program;
        }

        public int this[string index]
        {
            get
            {
                int i = GL.GetFragDataLocation(program, index);

                if (i == -1)
                {
                    throw new KeyNotFoundException(index);
                }

                return i;
            }
        }

        private int program;
    }
}

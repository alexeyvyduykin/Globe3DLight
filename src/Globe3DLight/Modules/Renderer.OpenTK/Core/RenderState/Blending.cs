using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
//using System.Drawing;
using OpenTK;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class Blending
    {
        public Blending()
        {
            Enabled = false;
            SourceRGBFactor = BlendingFactorSrc.One;
            SourceAlphaFactor = BlendingFactorSrc.One;
            DestinationRGBFactor = BlendingFactorDest.Zero;
            DestinationAlphaFactor = BlendingFactorDest.Zero;
            RGBEquation = BlendEquationMode.FuncAdd;
            AlphaEquation = BlendEquationMode.FuncAdd;
            Color = Color.FromArgb(0, 0, 0, 0);
        }

        public bool Enabled { get; set; }
        public BlendingFactorSrc SourceRGBFactor { get; set; }
        public BlendingFactorSrc SourceAlphaFactor { get; set; }
        public BlendingFactorDest DestinationRGBFactor { get; set; }
        public BlendingFactorDest DestinationAlphaFactor { get; set; }
        public BlendEquationMode RGBEquation { get; set; }
        public BlendEquationMode AlphaEquation { get; set; }
        public Color Color { get; set; }
    }
}

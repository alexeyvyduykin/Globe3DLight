﻿using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;

namespace Globe3DLight.Models.Scene
{
    public interface IArcballCamera : ICamera
    {
        double Width { get; set; }

        double Height { get; set; }

        double AdjustWidth { get; set; }

        double AdjustHeight { get; set; }

        ivec2 Point0 { get; set; }

        ivec2 Point1 { get; set; }

        void RotateBegin(int x, int y);

        void Rotate(int x, int y);

        void RotateEnd(int x, int y);

        void Zoom(double deltaZ);

        void Resize(int w, int h);
    }
}

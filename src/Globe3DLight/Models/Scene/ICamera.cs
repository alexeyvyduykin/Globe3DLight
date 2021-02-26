using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;

namespace Globe3DLight.Scene
{
    public interface ICamera : IObservableObject
    {
        void LookAt(dvec3 eye, dvec3 target, dvec3 up);
        dmat4 ViewMatrix { get; }
        dvec3 Eye { get; set; }
        dvec3 Target { get; set; }       
        dvec3 Up { get; set; }
        dvec3 Forward { get; }
        dvec3 Right { get; }
    }
}

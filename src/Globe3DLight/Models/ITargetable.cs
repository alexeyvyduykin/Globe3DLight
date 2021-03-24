using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;

namespace Globe3DLight.Models
{
    public interface ITargetable
    {
        dmat4 InverseAbsoluteModel { get; }

   //     dvec3 Eye { get; set; }

   //     dvec3 Target { get; set; }

   //     dvec3 Up { get; set; }
    }
}

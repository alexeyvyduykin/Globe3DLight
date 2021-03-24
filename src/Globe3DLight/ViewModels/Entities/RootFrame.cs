using GlmSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Models.Entities;
using Globe3DLight.Models.Scene;
using Globe3DLight.Models;

namespace Globe3DLight.ViewModels.Entities
{
    public class RootFrame : BaseEntity, ITargetable
    {
        public dmat4 InverseAbsoluteModel => dmat4.Identity.Inverse;

        public dvec3 Eye { get; set; } = new dvec3(0.0, 0.0, 20000.0);

        public dvec3 Target { get; set; } = dvec3.Zero;

        public dvec3 Up { get; set; } = dvec3.UnitY;
    }
}

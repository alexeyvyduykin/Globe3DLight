using GlmSharp;
using System;
using System.Collections.Generic;
using System.Text;


namespace Globe3DLight.ScenarioObjects
{
    public class RootFrame : ObservableObject, ITargetable
    {
        public dmat4 InverseAbsoluteModel => dmat4.Identity.Inverse;

        public dvec3 Eye { get; set; } = new dvec3(0.0, 0.0, 20000.0);

        public dvec3 Target { get; set; } = dvec3.Zero;

        public dvec3 Up { get; set; } = dvec3.UnitY;

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}

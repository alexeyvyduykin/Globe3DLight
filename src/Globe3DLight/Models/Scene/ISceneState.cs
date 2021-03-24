using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;

namespace Globe3DLight.Models.Scene
{
    public interface ISceneState
    {
        dmat4 ViewMatrix { get; }

        dmat4 ProjectionMatrix { get; }

        ICamera Camera { get; set; }

        ITargetable Target { get; set; }

        IDictionary<Type, (dvec3 eye, Func<double, double> func)> CameraBehaviours { get; set; }

        dvec4 LightPosition { get; set; }

        float DiffuseIntensity { get; set; }

        float SpecularIntensity { get; set; }

        float AmbientIntensity { get; set; }

        float Shininess { get; set; }
        
        float HighResolutionSnapScale { get; set; }

        // FOV
        double FieldOfViewX { get; }

        double FieldOfViewY { get; }

        double AspectRatio { get; set; }

        double PerspectiveNearPlaneDistance { get; set; }

        double PerspectiveFarPlaneDistance { get; set; }
    }
}

﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Globe3DLight.ViewModels.Geometry;

namespace Globe3DLight.ViewModels.Scene
{
    public class EarthRenderModel : BaseRenderModel
    {
        public ImmutableArray<Mesh> Meshes { get; set; }
        
        // (string pos_x, string neg_z, string neg_x, string pos_z, string pos_y, string neg_y)
        public IEnumerable<string> DiffuseKeys { get; set; }

        public IEnumerable<string> SpecularKeys { get; set; }

        public IEnumerable<string> NormalKeys { get; set; }

        public IEnumerable<string> NightKeys { get; set; }
    }
}

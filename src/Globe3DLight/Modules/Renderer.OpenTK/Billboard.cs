using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using GlmSharp;
using Globe3DLight.Models.Image;
using B = Globe3DLight.Renderer.OpenTK.Core;
using A = OpenTK.Graphics.OpenGL;
using Globe3DLight.Models.Geometry;
using Globe3DLight.ViewModels.Geometry;
using Globe3DLight.ViewModels.Containers;
using System.Collections.Immutable;
using Globe3DLight.ViewModels.Scene;
using Globe3DLight.Models.Renderer;
using Globe3DLight.ViewModels.Style;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.ViewModels.Geometry.Models;
using Globe3DLight.ViewModels;

namespace Globe3DLight.Renderer.OpenTK
{
    public class Billboard : AMesh
    {
        public Billboard() : base()
        {
            vec2[] billboardVertices = {
                new vec2(-1.0f, -1.0f),
                new vec2(-1.0f, 1.0f),
                new vec2(1.0f, 1.0f),
                new vec2(1.0f, -1.0f)
            };

            ushort[] billboardIndices = { 0, 1, 2, 0, 2, 3 };

            var builder = ImmutableArray.CreateBuilder<IVertexAttribute>();

            var positionsAttribute = new VertexAttribute<vec2>("POSITION", VertexAttributeType.FloatVector2); //new VertexAttributePosition2();
            builder.Add(positionsAttribute);

            base.Attributes = builder.ToImmutable();

            IndicesUnsignedShort indicesBase = new IndicesUnsignedShort();
            base.Indices = indicesBase;

            base.PrimitiveType = Models.Geometry.PrimitiveType.Triangles;
            base.FrontFaceWindingOrder = Models.Geometry.FrontFaceDirection.Cw;

            IList<vec2> positions = positionsAttribute.Values;
            IList<ushort> indices = indicesBase.Values;

            for (int i = 0; i < billboardVertices.Length; i++)
                positions.Add(billboardVertices[i]);

            for (int i = 0; i < billboardIndices.Length; i++)
                indices.Add(billboardIndices[i]);
        }
    }
}

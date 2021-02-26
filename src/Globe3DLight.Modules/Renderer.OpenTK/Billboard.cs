using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using GlmSharp;
using Globe3DLight.Image;
using B = Globe3DLight.Renderer.OpenTK.Core;
using A = OpenTK.Graphics.OpenGL;
using Globe3DLight.Geometry;
using Globe3DLight.Containers;
using System.Collections.Immutable;
using Globe3DLight.Scene;
using Globe3DLight.Renderer;
using Spatial;
using Globe3DLight.Style;
using Globe3DLight.ScenarioObjects;
using Globe3DLight.Data;
using Globe3DLight.Geometry.Models;

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

            base.PrimitiveType = Geometry.PrimitiveType.Triangles;
            base.FrontFaceWindingOrder = Geometry.FrontFaceDirection.Cw;

            IList<vec2> positions = positionsAttribute.Values;
            IList<ushort> indices = indicesBase.Values;

            for (int i = 0; i < billboardVertices.Length; i++)
                positions.Add(billboardVertices[i]);

            for (int i = 0; i < billboardIndices.Length; i++)
                indices.Add(billboardIndices[i]);
        }
    }
}

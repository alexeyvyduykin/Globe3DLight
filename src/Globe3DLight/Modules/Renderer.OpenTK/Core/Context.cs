#nullable enable
using System;
using System.Linq;
using Globe3DLight.ViewModels.Geometry;
using A = OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class Context
    {
        private readonly Device _device;
        private readonly RenderState _renderState;
        private ShaderProgram? _boundShaderProgram;
        private readonly TextureUnits _textureUnits;

        public Context()
        {
            _device = new Device();

            _renderState = _device.CreateRenderState();
            _textureUnits = new TextureUnits();

            ForceApplyRenderState(_renderState);            
        }

        public TextureUnits TextureUnits => _textureUnits;

        public VertexArray CreateVertexArray(Mesh mesh, ShaderVertexAttributeCollection shaderAttributes, A.BufferUsageHint usageHint)
        {
            var va = new VertexArray();

            if (mesh.Indices != null)
            {
                var indexBuffer = _device.CreateIndexBuffer(usageHint, mesh.Indices.Count * sizeof(ushort));
                indexBuffer.CopyFromSystemMemory(mesh.Indices.ToArray());
                va.IndexBuffer = indexBuffer;
            }

            // TODO:  Not tested exhaustively
            foreach (var shaderAttribute in shaderAttributes)
            {

                if (mesh.Vertices.Count != 0 && shaderAttribute.Name == "POSITION")
                {
                    var vertexBuffer = _device.CreateVertexBuffer(mesh.Vertices, usageHint);
                    va.Attributes[shaderAttribute.Location] = new VertexBufferAttribute(vertexBuffer, A.VertexAttribPointerType.Float, 3);
                }
                else if (mesh.Normals.Count != 0 && shaderAttribute.Name == "NORMAL")
                {
                    var vertexBuffer = _device.CreateVertexBuffer(mesh.Normals, usageHint);
                    va.Attributes[shaderAttribute.Location] = new VertexBufferAttribute(vertexBuffer, A.VertexAttribPointerType.Float, 3);
                }
                else if (mesh.TexCoords.Count != 0 && shaderAttribute.Name == "TEXCOORD")
                {
                    var vertexBuffer = _device.CreateVertexBuffer(mesh.TexCoords, usageHint);
                    va.Attributes[shaderAttribute.Location] = new VertexBufferAttribute(vertexBuffer, A.VertexAttribPointerType.Float, 2);
                }
                else if (mesh.Tangents.Count != 0 && shaderAttribute.Name == "TANGENT")
                {
                    var vertexBuffer = _device.CreateVertexBuffer(mesh.Tangents, usageHint);
                    va.Attributes[shaderAttribute.Location] = new VertexBufferAttribute(vertexBuffer, A.VertexAttribPointerType.Float, 3);
                }
                else if (shaderAttribute.Name == "COLOR")
                {
                    throw new Exception();
                }
                else
                {
                    throw new Exception();
                }
            }

            return va;
        }

        private void ApplyPrimitiveRestart(PrimitiveRestart primitiveRestart)
        {
            if (_renderState.PrimitiveRestart.Enabled != primitiveRestart.Enabled)
            {
                Enable(A.EnableCap.PrimitiveRestart, primitiveRestart.Enabled);
                _renderState.PrimitiveRestart.Enabled = primitiveRestart.Enabled;
            }

            if (primitiveRestart.Enabled)
            {
                if (_renderState.PrimitiveRestart.Index != primitiveRestart.Index)
                {
                    A.GL.PrimitiveRestartIndex(primitiveRestart.Index);
                    _renderState.PrimitiveRestart.Index = primitiveRestart.Index;
                }
            }
        }

        private void ApplyFacetCulling(FacetCulling facetCulling)
        {
            if (_renderState.FacetCulling.Enabled != facetCulling.Enabled)
            {
                Enable(A.EnableCap.CullFace, facetCulling.Enabled);
                _renderState.FacetCulling.Enabled = facetCulling.Enabled;
            }

            if (facetCulling.Enabled)
            {
                if (_renderState.FacetCulling.Face != facetCulling.Face)
                {
                    A.GL.CullFace(facetCulling.Face);
                    _renderState.FacetCulling.Face = facetCulling.Face;
                }

                if (_renderState.FacetCulling.FrontFaceWindingOrder != facetCulling.FrontFaceWindingOrder)
                {
                    A.GL.FrontFace(facetCulling.FrontFaceWindingOrder);
                    _renderState.FacetCulling.FrontFaceWindingOrder = facetCulling.FrontFaceWindingOrder;
                }
            }
        }

        private void ApplyProgramPointSize(ProgramPointSize programPointSize)
        {
            if (_renderState.ProgramPointSize != programPointSize)
            {
                Enable(A.EnableCap.ProgramPointSize, programPointSize == ProgramPointSize.Enabled);
                _renderState.ProgramPointSize = programPointSize;
            }
        }

        private void ApplyRasterizationMode(A.PolygonMode rasterizationMode)
        {
            if (_renderState.RasterizationMode != rasterizationMode)
            {
                A.GL.PolygonMode(A.MaterialFace.FrontAndBack, rasterizationMode);
                _renderState.RasterizationMode = rasterizationMode;
            }
        }

        private void ApplyScissorTest(ScissorTest scissorTest)
        {
            var rectangle = scissorTest.Rectangle;

            if (rectangle.Width < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "renderState.ScissorTest.Rectangle.Width must be greater than or equal to zero.",
                    "renderState");
            }

            if (rectangle.Height < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "renderState.ScissorTest.Rectangle.Height must be greater than or equal to zero.",
                    "renderState");
            }

            if (_renderState.ScissorTest.Enabled != scissorTest.Enabled)
            {
                Enable(A.EnableCap.ScissorTest, scissorTest.Enabled);
                _renderState.ScissorTest.Enabled = scissorTest.Enabled;
            }

            if (scissorTest.Enabled)
            {
                if (_renderState.ScissorTest.Rectangle != scissorTest.Rectangle)
                {
                    A.GL.Scissor(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
                    _renderState.ScissorTest.Rectangle = scissorTest.Rectangle;
                }
            }
        }

        //private void ApplyStencilTest(StencilTest stencilTest)
        //{
        //    if (renderState.StencilTest.Enabled != stencilTest.Enabled)
        //    {
        //        Enable(EnableCap.StencilTest, stencilTest.Enabled);
        //        renderState.StencilTest.Enabled = stencilTest.Enabled;
        //    }

        //    if (stencilTest.Enabled)
        //    {
        //        ApplyStencil(StencilFace.Front, renderState.StencilTest.FrontFace, stencilTest.FrontFace);
        //        ApplyStencil(StencilFace.Back, renderState.StencilTest.BackFace, stencilTest.BackFace);
        //    }
        //}

        //private static void ApplyStencil(StencilFace face, StencilTestFace currentTest, StencilTestFace test)
        //{
        //    if ((currentTest.StencilFailOperation != test.StencilFailOperation) ||
        //        (currentTest.DepthFailStencilPassOperation != test.DepthFailStencilPassOperation) ||
        //        (currentTest.DepthPassStencilPassOperation != test.DepthPassStencilPassOperation))
        //    {
        //        GL.StencilOpSeparate(face,
        //            test.StencilFailOperation,
        //            test.DepthFailStencilPassOperation,
        //            test.DepthPassStencilPassOperation);

        //        currentTest.StencilFailOperation = test.StencilFailOperation;
        //        currentTest.DepthFailStencilPassOperation = test.DepthFailStencilPassOperation;
        //        currentTest.DepthPassStencilPassOperation = test.DepthPassStencilPassOperation;
        //    }

        //    if ((currentTest.Function != test.Function) ||
        //        (currentTest.ReferenceValue != test.ReferenceValue) ||
        //        (currentTest.Mask != test.Mask))
        //    {
        //        GL.StencilFuncSeparate(face, test.Function, test.ReferenceValue, test.Mask);

        //        currentTest.Function = test.Function;
        //        currentTest.ReferenceValue = test.ReferenceValue;
        //        currentTest.Mask = test.Mask;
        //    }
        //}

        private void ApplyDepthTest(DepthTest depthTest)
        {
            if (_renderState.DepthTest.Enabled != depthTest.Enabled)
            {
                Enable(A.EnableCap.DepthTest, depthTest.Enabled);
                _renderState.DepthTest.Enabled = depthTest.Enabled;
            }

            if (depthTest.Enabled)
            {
                if (_renderState.DepthTest.Function != depthTest.Function)
                {
                    A.GL.DepthFunc(depthTest.Function);
                    _renderState.DepthTest.Function = depthTest.Function;
                }
            }
        }

        private void ApplyDepthRange(DepthRange depthRange)
        {
            if (depthRange.Near < 0.0 || depthRange.Near > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(depthRange), "renderState.DepthRange.Near must be between zero and one.");
            }

            if (depthRange.Far < 0.0 || depthRange.Far > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(depthRange), "renderState.DepthRange.Far must be between zero and one.");
            }

            if ((_renderState.DepthRange.Near != depthRange.Near) ||
                (_renderState.DepthRange.Far != depthRange.Far))
            {
                A.GL.DepthRange(depthRange.Near, depthRange.Far);

                _renderState.DepthRange.Near = depthRange.Near;
                _renderState.DepthRange.Far = depthRange.Far;
            }
        }

        private void ApplyBlending(Blending blending)
        {
            if (_renderState.Blending.Enabled != blending.Enabled)
            {
                Enable(A.EnableCap.Blend, blending.Enabled);
                _renderState.Blending.Enabled = blending.Enabled;
            }

            if (blending.Enabled)
            {
                if ((_renderState.Blending.SourceRGBFactor != blending.SourceRGBFactor) ||
                    (_renderState.Blending.DestinationRGBFactor != blending.DestinationRGBFactor) ||
                    (_renderState.Blending.SourceAlphaFactor != blending.SourceAlphaFactor) ||
                    (_renderState.Blending.DestinationAlphaFactor != blending.DestinationAlphaFactor))
                {
                    A.GL.BlendFuncSeparate(
                        blending.SourceRGBFactor,
                        blending.DestinationRGBFactor,
                        blending.SourceAlphaFactor,
                        blending.DestinationAlphaFactor);

                    _renderState.Blending.SourceRGBFactor = blending.SourceRGBFactor;
                    _renderState.Blending.DestinationRGBFactor = blending.DestinationRGBFactor;
                    _renderState.Blending.SourceAlphaFactor = blending.SourceAlphaFactor;
                    _renderState.Blending.DestinationAlphaFactor = blending.DestinationAlphaFactor;
                }

                A.GL.BlendFuncSeparate(
    blending.SourceRGBFactor,
    blending.DestinationRGBFactor,
    blending.SourceAlphaFactor,
    blending.DestinationAlphaFactor);

                if ((_renderState.Blending.RGBEquation != blending.RGBEquation) ||
                    (_renderState.Blending.AlphaEquation != blending.AlphaEquation))
                {
                    A.GL.BlendEquationSeparate(blending.RGBEquation, blending.AlphaEquation);

                    _renderState.Blending.RGBEquation = blending.RGBEquation;
                    _renderState.Blending.AlphaEquation = blending.AlphaEquation;
                }

                if (_renderState.Blending.Color != blending.Color)
                {
                    A.GL.BlendColor(blending.Color);
                    _renderState.Blending.Color = blending.Color;
                }

            }
        }

        private void ApplyColorMask(ColorMask colorMask)
        {
            if (_renderState.ColorMask != colorMask)
            {
                A.GL.ColorMask(colorMask.Red, colorMask.Green, colorMask.Blue, colorMask.Alpha);
                _renderState.ColorMask = colorMask;
            }
        }

        private void ApplyDepthMask(bool depthMask)
        {
            if (_renderState.DepthMask != depthMask)
            {
                A.GL.DepthMask(depthMask);
                _renderState.DepthMask = depthMask;
            }
        }

        private static void Enable(A.EnableCap enableCap, bool enable)
        {
            if (enable)
            {
                A.GL.Enable(enableCap);
            }
            else
            {
                A.GL.Disable(enableCap);
            }
        }

        public void ApplyRenderState(RenderState renderState)
        {
            ApplyPrimitiveRestart(renderState.PrimitiveRestart);

            ApplyFacetCulling(renderState.FacetCulling);
            ApplyProgramPointSize(renderState.ProgramPointSize);
            ApplyRasterizationMode(renderState.RasterizationMode);
            ApplyScissorTest(renderState.ScissorTest);
            //         ApplyStencilTest(renderState.StencilTest);
            ApplyDepthTest(renderState.DepthTest);
            ApplyDepthRange(renderState.DepthRange);
            ApplyBlending(renderState.Blending);
            ApplyColorMask(renderState.ColorMask);
            ApplyDepthMask(renderState.DepthMask);
        }

        public static void ApplyVertexArray(VertexArray vertexArray)
        {
            vertexArray.Bind();
            vertexArray.Clean();
        }

        private static void VerifyDraw(DrawState drawState, Globe3DLight.Models.Scene.ISceneState sceneState)
        {
            if (drawState == null)
            {
                throw new ArgumentNullException(nameof(drawState));
            }

            if (drawState.RenderState == null)
            {
                throw new ArgumentNullException(nameof(drawState.RenderState), "");
            }

            if (drawState.ShaderProgram == null)
            {
                throw new ArgumentNullException(nameof(drawState.ShaderProgram), "");
            }

            if (drawState.VertexArray == null)
            {
                throw new ArgumentNullException(nameof(drawState.VertexArray), "");
            }

            if (sceneState == null)
            {
                throw new ArgumentNullException(nameof(sceneState));
            }

            //if (setFramebuffer != null)
            //{
            //    if (drawState.RenderState.DepthTest.Enabled &&
            //        !((setFramebuffer.DepthAttachment != null) ||
            //          (setFramebuffer.DepthStencilAttachment != null)))
            //    {
            //        throw new ArgumentException("The depth test is enabled (drawState.RenderState.DepthTest.Enabled) but the context's Framebuffer property doesn't have a depth or depth/stencil attachment (DepthAttachment or DepthStencilAttachment).", "drawState");
            //    }
            //}
        }

        private void ApplyBeforeDraw(DrawState drawState, Globe3DLight.Models.Scene.ISceneState sceneState)
        {
            ApplyRenderState(drawState.RenderState);
            ApplyVertexArray(drawState.VertexArray);
            ApplyShaderProgram(drawState, sceneState);

            _textureUnits.Clean();
            // ApplyFramebuffer();
        }

        public void ApplyShaderProgram(DrawState drawState, Globe3DLight.Models.Scene.ISceneState _)
        {
            var shaderProgram = drawState.ShaderProgram;

            if (shaderProgram != _boundShaderProgram)
            {
                _boundShaderProgram = shaderProgram;
            }

            _boundShaderProgram.Bind();
            _boundShaderProgram.Clean(this, drawState/*, null*//*sceneState*/);

            //ShaderProgram shaderProgram = drawState.ShaderProgram;

            //if (boundShaderProgram != shaderProgram)
            //{
            //    shaderProgram.Bind();
            //    boundShaderProgram = shaderProgram;
            //}

            //boundShaderProgram.Clean(this, drawState, sceneState);
        }

        private static void ForceApplyRenderState(RenderState renderState)
        {
            //          Enable(EnableCap.PrimitiveRestart, renderState.PrimitiveRestart.Enabled);
            //          GL.PrimitiveRestartIndex(renderState.PrimitiveRestart.Index);

            //           Enable(EnableCap.CullFace, renderState.FacetCulling.Enabled);
            //          GL.CullFace(renderState.FacetCulling.Face);
            //          GL.FrontFace(renderState.FacetCulling.FrontFaceWindingOrder);

            //           Enable(EnableCap.ProgramPointSize, renderState.ProgramPointSize == ProgramPointSize.Enabled);
            //          GL.PolygonMode(MaterialFace.FrontAndBack, renderState.RasterizationMode);

            //           Enable(EnableCap.ScissorTest, renderState.ScissorTest.Enabled);
            //           Rectangle rectangle = renderState.ScissorTest.Rectangle;
            //           GL.Scissor(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);

            //Enable(EnableCap.StencilTest, renderState.StencilTest.Enabled);
            //ForceApplyRenderStateStencil(StencilFace.Front, renderState.StencilTest.FrontFace);
            //ForceApplyRenderStateStencil(StencilFace.Back, renderState.StencilTest.BackFace);

            Enable(A.EnableCap.DepthTest, renderState.DepthTest.Enabled);
            A.GL.DepthFunc(renderState.DepthTest.Function);

            //           GL.DepthRange(renderState.DepthRange.Near, renderState.DepthRange.Far);

            //           Enable(EnableCap.Blend, renderState.Blending.Enabled);
            //          GL.BlendFuncSeparate(
            //              renderState.Blending.SourceRGBFactor,
            //              renderState.Blending.DestinationRGBFactor,
            //              renderState.Blending.SourceAlphaFactor,
            //              renderState.Blending.DestinationAlphaFactor);
            //          GL.BlendEquationSeparate(
            //              renderState.Blending.RGBEquation,
            //               renderState.Blending.AlphaEquation);
            //           GL.BlendColor(renderState.Blending.Color);

            //           GL.DepthMask(renderState.DepthMask);
            //           GL.ColorMask(renderState.ColorMask.Red, renderState.ColorMask.Green,
            //              renderState.ColorMask.Blue, renderState.ColorMask.Alpha);
        }

        public void Draw(A.PrimitiveType primitiveType, DrawState drawState, Globe3DLight.Models.Scene.ISceneState sceneState)
        {
            VerifyDraw(drawState, sceneState);
            ApplyBeforeDraw(drawState, sceneState);

            var vertexArray = drawState.VertexArray;
            var indexBuffer = vertexArray.IndexBuffer;

            if (indexBuffer != null)
            {
                A.GL.DrawRangeElements(primitiveType,
                    0, vertexArray.MaximumArrayIndex(), indexBuffer.Count,
                    TypeConverter.To(indexBuffer.Datatype), new IntPtr());

                // GL.DrawElements(primitiveType, indexBuffer.Count, TypeConverter.To(indexBuffer.Datatype), 0);
            }
            else
            {
                A.GL.DrawArrays(primitiveType, 0, vertexArray.MaximumArrayIndex() + 1);
            }
        }
    }

}

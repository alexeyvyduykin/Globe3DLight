#nullable enable
using System;
using System.Linq;
using GlmSharp;
using Globe3DLight.Models.Renderer;
using Globe3DLight.Renderer.OpenTK.Core;
using Globe3DLight.ViewModels.Geometry;
using A = OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK
{
    internal interface IModelRenderer
    {
        void Draw(ShaderProgram sp);

        void SetupTextures();
    }

    internal class ModelRenderer : IModelRenderer
    {
        private readonly Model _model;
        private readonly int[] _vaos, _vbos, _ebos;
        private readonly int[] _mapDiffuseNames;
        private readonly string[] _mapDiffuseTypes;
        private readonly ICache<string, int> _textureCache;

        private struct Vertex
        {
            public vec3 position;
            public vec3 normal;
            public vec2 texCoords;
        }

        public ModelRenderer(Model model, ICache<string, int> textureCache)
        {
            _model = model;

            _textureCache = textureCache;

            _mapDiffuseNames = new int[_model.Materials.Count];
            _mapDiffuseTypes = new string[_model.Materials.Count];

            _vaos = new int[_model.Meshes.Count];
            _vbos = new int[_model.Meshes.Count];
            _ebos = new int[_model.Meshes.Count];

            SetupMeshes();
        }

        public void Draw(ShaderProgram sp)
        {
            for (int i = 0; i < _model.Meshes.Count; i++)
            {
                var mesh = _model.Meshes[i];
                var material = _model.Materials[mesh.MaterialIndex];

                if (material.HasTextureDiffuse == true)
                {
                    A.GL.ActiveTexture(A.TextureUnit.Texture0 + 0); // Active proper texture unit before binding
                                                                    // Retrieve texture number (the N in diffuse_textureN)
                                                                    // stringstream ss;

                    // Now set the sampler to the correct texture unit
                    A.GL.Uniform1(A.GL.GetUniformLocation(sp.Handle, _mapDiffuseTypes[mesh.MaterialIndex]), 0);
                    // And finally bind the texture
                    A.GL.BindTexture(A.TextureTarget.Texture2D, _mapDiffuseNames[mesh.MaterialIndex]);
                }

                sp.SetUniform("material.ambient", material.Ambient);
                sp.SetUniform("material.diffuse", material.Diffuse);
                sp.SetUniform("material.specular", material.Specular);
                sp.SetUniform("material.emission", material.Emission);
                sp.SetUniform("material.shininess", material.Shininess);

                if (material.HasTextureDiffuse == true)
                {
                    sp.SetUniform("u_isTexture", 1.0f);
                }
                else
                {
                    sp.SetUniform("u_isTexture", 0.0f);
                }

                // Draw mesh
                A.GL.BindVertexArray(_vaos[i]);
                A.GL.DrawElements(A.BeginMode.Triangles, mesh.Indices.Count, A.DrawElementsType.UnsignedShort, 0);
                A.GL.BindVertexArray(0);

                // Always good practice to set everything back to defaults once configured.
                A.GL.ActiveTexture(A.TextureUnit.Texture0 + 0);
                A.GL.BindTexture(A.TextureTarget.Texture2D, 0);
            }
        }

        private void SetupMeshes()
        {
            for (int i = 0; i < _model.Meshes.Count; i++)
            {
                var mesh = _model.Meshes[i];

                var vertices = new Vertex[mesh.Vertices.Count];

                for (int j = 0; j < mesh.Vertices.Count; j++)
                {
                    vertices[j] = new Vertex()
                    {
                        position = mesh.Vertices[j],
                        normal = mesh.Normals[j],
                        texCoords = mesh.TexCoords[j]
                    };
                }

                // Create buffers/arrays
                _vaos[i] = A.GL.GenVertexArray();
                _vbos[i] = A.GL.GenBuffer();
                _ebos[i] = A.GL.GenBuffer();

                A.GL.BindVertexArray(_vaos[i]);
                // Load data into vertex buffers
                A.GL.BindBuffer(A.BufferTarget.ArrayBuffer, _vbos[i]);
                // A great thing about structs is that their memory layout is sequential for all its items.
                // The effect is that we can simply pass a pointer to the struct and it translates perfectly to a glm::vec3/2 array which
                // again translates to 3/2 floats which translates to a byte array.
                A.GL.BufferData(A.BufferTarget.ArrayBuffer, new IntPtr(ArraySizeInBytes.Size<Vertex>(vertices.ToArray())),
                    vertices.ToArray(), A.BufferUsageHint.StaticDraw);

                A.GL.BindBuffer(A.BufferTarget.ElementArrayBuffer, _ebos[i]);
                A.GL.BufferData(A.BufferTarget.ElementArrayBuffer, new IntPtr(ArraySizeInBytes.Size<ushort>(mesh.Indices.ToArray())),
                    mesh.Indices.ToArray(), A.BufferUsageHint.StaticDraw);

                // Set the vertex attribute pointers
                // Vertex Positions           
                A.GL.VertexAttribPointer((int)0, 3, A.VertexAttribPointerType.Float, false, SizeInBytes<Vertex>.Value, 0);
                A.GL.EnableVertexAttribArray((int)0);
                // Vertex Normals            
                A.GL.VertexAttribPointer((int)1, 3, A.VertexAttribPointerType.Float, false, SizeInBytes<Vertex>.Value, SizeInBytes<vec3>.Value);
                A.GL.EnableVertexAttribArray((int)1);
                // Vertex Texture Coords          
                A.GL.VertexAttribPointer((int)2, 2, A.VertexAttribPointerType.Float, false, SizeInBytes<Vertex>.Value, 2 * SizeInBytes<vec3>.Value);
                A.GL.EnableVertexAttribArray((int)2);

                A.GL.BindVertexArray(0);
            }
        }

        public void SetupTextures()
        {
            for (int i = 0; i < _model.Materials.Count; i++)
            {
                var material = _model.Materials[i];
                if (material.HasTextureDiffuse == true)
                {
                    int key;
                    if ((key = _textureCache.Get(material.TextureDiffuseKey)) != default)
                    {
                        _mapDiffuseNames[i] = key;
                        _mapDiffuseTypes[i] = "u_textureDiffuse";
                    }
                    else
                    {
                        key = Load2DTexture(material.TextureDiffusePath, A.TextureWrapMode.Repeat, true);
                        _mapDiffuseNames[i] = key;
                        _mapDiffuseTypes[i] = "u_textureDiffuse";

                        _textureCache.Set(material.TextureDiffuseKey, key);
                    }
                }
            }
        }

        private int Load2DTexture(string path, A.TextureWrapMode mode, bool mipmap)
        {
            var id = A.GL.GenTexture();
            var bitmap = new System.Drawing.Bitmap(path);

            var data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            A.GL.BindTexture(A.TextureTarget.Texture2D, id);

            A.GL.TexImage2D(A.TextureTarget.Texture2D, 0, A.PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    A.PixelFormat.Bgra, A.PixelType.UnsignedByte, data.Scan0);

            if (mipmap)
            {
                A.GL.GenerateMipmap(A.GenerateMipmapTarget.Texture2D);
            }

            A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureWrapS, (int)mode);
            A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureWrapT, (int)mode);
            if (mipmap)
            {
                A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureMinFilter, (int)A.TextureMinFilter.LinearMipmapLinear);
            }
            else
            {
                A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureMinFilter, (int)A.TextureMinFilter.Linear);
            }
            A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureMagFilter, (int)A.TextureMagFilter.Linear);
            A.GL.BindTexture(A.TextureTarget.Texture2D, 0);

            bitmap.UnlockBits(data);

            return id;
        }
    }
}

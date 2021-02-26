using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;


namespace Globe3DLight.Renderer.OpenTK.Core
{

    //internal interface IMesh
    //{ 
    //    IEnumerable<vec3> Vertices { get; }
    //    IEnumerable<vec3> Normals { get; }
    //    IEnumerable<vec2> TexCoords { get; }
    //    IEnumerable<short> Indices { get; }
    //    IMaterial Material { get; }

    //    int MaterialIndex { get; }
    //}



    //internal class Mesh : IMesh
    //{      
    //    private IEnumerable<vec3> _vertices;
    //    private IEnumerable<vec3> _normals;
    //    private IEnumerable<vec2> _texCoords;
    //    private IEnumerable<short> _indices;
    //    private Material _material;
    //    private int _materialIndex;
    //    //      public List<vec3> tangents;      
    //    //      public List<vec3> biTangents;   
    //    private int _vao, _vbo, _ebo;

    //    public IEnumerable<vec3> Vertices => _vertices;
    //    public IEnumerable<vec3> Normals => _normals;
    //    public IEnumerable<vec2> TexCoords => _texCoords;
    //    public IEnumerable<short> Indices => _indices;
    //    public IMaterial Material => _material;
    //    public int MaterialIndex => _materialIndex;

    //    public Mesh(IEnumerable<vec3> vertices, IEnumerable<vec3> normals, IEnumerable<vec2> texCoords, IEnumerable<short> indices, int materialIndex)
    //    {
    //        this._vertices = vertices;
    //        this._normals = normals;
    //        this._texCoords = texCoords;
    //        this._indices = indices;
    //        this._materialIndex = materialIndex;
    //    }

    //    public void draw(ShaderProgram program)
    //    {
    //        if (material.mapDiffuse != null)
    //        {
    //            A.GL.ActiveTexture(A.TextureUnit.Texture0 + 0); // Active proper texture unit before binding
    //                                                            // Retrieve texture number (the N in diffuse_textureN)
    //                                                            //stringstream ss;

    //            // Now set the sampler to the correct texture unit
    //            A.GL.Uniform1(A.GL.GetUniformLocation(program.Handle, material.mapDiffuse.type), 0);
    //            // And finally bind the texture
    //            A.GL.BindTexture(A.TextureTarget.Texture2D, material.mapDiffuse.id);
    //        }

    //        program.SetUniform("material.ambient", material.ambient);
    //        program.SetUniform("material.diffuse", material.diffuse);
    //        program.SetUniform("material.specular", material.specular);
    //        program.SetUniform("material.emission", material.emission);
    //        program.SetUniform("material.shininess", material.shininess);

    //        if (material.mapDiffuse != null)
    //            program.SetUniform("u_isTexture", 1.0f);
    //        else
    //            program.SetUniform("u_isTexture", 0.0f);

    //        // Draw mesh
    //        A.GL.BindVertexArray(VAO);
    //        A.GL.DrawElements(A.BeginMode.Triangles, indices.Count, A.DrawElementsType.UnsignedShort, 0);
    //        A.GL.BindVertexArray(0);

    //        // Always good practice to set everything back to defaults once configured.
    //        A.GL.ActiveTexture(A.TextureUnit.Texture0 + 0);
    //        A.GL.BindTexture(A.TextureTarget.Texture2D, 0);

    //    }

    //    private void setupMesh()
    //    {
    //        // Create buffers/arrays
    //        VAO = A.GL.GenVertexArray();
    //        VBO = A.GL.GenBuffer();
    //        EBO = A.GL.GenBuffer();

    //        A.GL.BindVertexArray(VAO);
    //        // Load data into vertex buffers
    //        A.GL.BindBuffer(A.BufferTarget.ArrayBuffer, VBO);
    //        // A great thing about structs is that their memory layout is sequential for all its items.
    //        // The effect is that we can simply pass a pointer to the struct and it translates perfectly to a glm::vec3/2 array which
    //        // again translates to 3/2 floats which translates to a byte array.
    //        A.GL.BufferData(A.BufferTarget.ArrayBuffer, new IntPtr(ArraySizeInBytes.Size<Vertex>(vertices.ToArray())),
    //            vertices.ToArray(), A.BufferUsageHint.StaticDraw);

    //        A.GL.BindBuffer(A.BufferTarget.ElementArrayBuffer, EBO);
    //        A.GL.BufferData(A.BufferTarget.ElementArrayBuffer, new IntPtr(ArraySizeInBytes.Size<short>(indices.ToArray())),
    //            indices.ToArray(), A.BufferUsageHint.StaticDraw);

    //        // Set the vertex attribute pointers
    //        // Vertex Positions
    //        A.GL.EnableVertexAttribArray((int)0);
    //        A.GL.VertexAttribPointer((int)0, 3, A.VertexAttribPointerType.Float, false, SizeInBytes<Vertex>.Value, 0);
    //        // Vertex Normals
    //        A.GL.EnableVertexAttribArray((int)1);
    //        A.GL.VertexAttribPointer((int)1, 3, A.VertexAttribPointerType.Float, false, SizeInBytes<Vertex>.Value, SizeInBytes<vec3>.Value);
    //        // Vertex Texture Coords
    //        A.GL.EnableVertexAttribArray((int)2);
    //        A.GL.VertexAttribPointer((int)2, 2, A.VertexAttribPointerType.Float, false, SizeInBytes<Vertex>.Value, 2 * SizeInBytes<vec3>.Value);

    //        A.GL.BindVertexArray(0);

    //    }


    //}
}

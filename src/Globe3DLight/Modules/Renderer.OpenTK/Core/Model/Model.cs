﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
using A = OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using Assimp;

namespace Globe3DLight.Renderer.OpenTK.Core
{

    //internal interface IModel
    //{
    //    IEnumerable<IMesh> Meshes { get; }

    //    IEnumerable<IMaterial> Materials { get; }
    //}


    //internal class Model : IModel
    //{
    //    private IList<Texture> _texturesLoaded = new List<Texture>();
    //    private IList<IMesh> _meshes = new List<IMesh>();
    //    private IList<IMaterial> _materials= new List<IMaterial>();
    //    private string _directory;


    //    public IEnumerable<IMesh> Meshes => _meshes;

    //    public IEnumerable<IMaterial> Materials => _materials;

    //    public Model(IEnumerable<IMesh> meshes, IEnumerable<IMaterial> materials)
    //    {

    //    }


    //    public Model(string path)
    //    {

    //        AssimpContext importer = new AssimpContext();
    //        Assimp.Scene scene = importer.ImportFile(path, PostProcessSteps.PreTransformVertices | PostProcessSteps.GenerateSmoothNormals);

    //        _directory = Path.GetDirectoryName(path);

    //        processNode(scene.RootNode, scene);
    //    }

    //    public void draw(ShaderProgram program)
    //    {
    //        for (int i = 0; i < _meshes.Count; i++)
    //            _meshes[i].draw(program);
    //    }

    //    private void processNode(Node node, Assimp.Scene scene)
    //    {
    //        // Process each mesh located at the current node
    //        for (int i = 0; i < node.MeshCount; i++)
    //        {
    //            // The node object only contains indices to index the actual objects in the scene. 
    //            // The scene contains all the data, node is just to keep stuff organized (like relations between nodes).
    //            Assimp.Mesh mesh = scene.Meshes[node.MeshIndices[i]];

    //            _meshes.Add(processMesh(mesh, scene));
    //        }
    //        // After we've processed all of the meshes (if any) we then recursively process each of the children nodes
    //        for (int i = 0; i < node.ChildCount; i++)
    //        {
    //            processNode(node.Children[i], scene);
    //        }
    //    }

    //    private Mesh processMesh(Assimp.Mesh mesh, Assimp.Scene scene)
    //    {
    //        List<Vertex> vertices = new List<Vertex>();
    //        List<short> indices = new List<short>();
    //        //List<Texture> textures = new List<Texture>();
    //        Material material = new Material();

    //        // Walk through each of the mesh's vertices
    //        for (int i = 0; i < mesh.VertexCount; i++)
    //        {
    //            Vertex vertex = new Vertex();
    //            vec3 vector; // We declare a placeholder vector since assimp uses its own vector class that doesn't directly convert to glm's vec3 class so we transfer the data to this placeholder glm::vec3 first.

    //            // Positions
    //            vector.x = mesh.Vertices[i].X;
    //            vector.y = mesh.Vertices[i].Y;
    //            vector.z = mesh.Vertices[i].Z;
    //            vertex.position = vector;

    //            // Normals
    //            vector.x = mesh.Normals[i].X;
    //            vector.y = mesh.Normals[i].Y;
    //            vector.z = mesh.Normals[i].Z;
    //            vertex.normal = vector;

    //            // Texture Coordinates
    //            if (mesh.HasTextureCoords(0)) // Does the mesh contain texture coordinates?
    //            {
    //                vec2 vec;
    //                // A vertex can contain up to 8 different texture coordinates. We thus make the assumption that we won't 
    //                // use models where a vertex can have multiple texture coordinates so we always take the first set (0).
    //                vec.x = mesh.TextureCoordinateChannels[0][i].X;
    //                vec.y = mesh.TextureCoordinateChannels[0][i].Y;
    //                vertex.texCoords = vec;
    //            }
    //            else
    //                vertex.texCoords = new vec2(0.0f, 0.0f);
    //            vertices.Add(vertex);
    //        }

    //        // Now wak through each of the mesh's faces (a face is a mesh its triangle) and retrieve the corresponding vertex indices.
    //        for (int i = 0; i < mesh.FaceCount; i++)
    //        {
    //            Face face = mesh.Faces[i];
    //            // Retrieve all indices of the face and store them in the indices vector
    //            for (int j = 0; j < face.IndexCount; j++)
    //                indices.Add((short)face.Indices[j]);
    //        }

    //        // Process materials
    //        if (mesh.MaterialIndex >= 0)
    //        {
    //            Assimp.Material aMaterial = scene.Materials[mesh.MaterialIndex];
    //            // We assume a convention for sampler names in the shaders. Each diffuse texture should be named
    //            // as 'texture_diffuseN' where N is a sequential number ranging from 1 to MAX_SAMPLER_NUMBER. 
    //            // Same applies to other texture as the following list summarizes:
    //            // Diffuse: texture_diffuseN
    //            // Specular: texture_specularN
    //            // Normal: texture_normalN

    //            material.ambient = convertToVec4(aMaterial.ColorAmbient);
    //            material.diffuse = convertToVec4(aMaterial.ColorDiffuse);
    //            material.emission = convertToVec4(aMaterial.ColorEmissive);
    //            material.specular = convertToVec4(aMaterial.ColorSpecular);
    //            material.shininess = 20.0f;// aMaterial.Shininess;

    //            // 1. Diffuse maps
    //            List<Texture> diffuseMaps = loadMaterialTextures(aMaterial, TextureType.Diffuse, "u_textureDiffuse");
    //            //textures.InsertRange(textures.Count, diffuseMaps);
    //            if (diffuseMaps.Count != 0)
    //                material.mapDiffuse = diffuseMaps[0];
    //            // 2. Specular maps
    //            //                List<Texture> specularMaps = loadMaterialTextures(material, aiTextureType_SPECULAR, "texture_specular");
    //            //                curMesh.insert(textures.end(), specularMaps.begin(), specularMaps.end());
    //        }

    //        // Return a mesh object created from the extracted mesh data
    //        return new Mesh(vertices, indices, material);
    //    }

    //    private int load2DTexture(string path, A.TextureWrapMode mode, bool mipmap)
    //    {
    //        int id = A.GL.GenTexture();
    //        Bitmap bitmap = new Bitmap(path);

    //        BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
    //            ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

    //        A.GL.BindTexture(A.TextureTarget.Texture2D, id);

    //        A.GL.TexImage2D(A.TextureTarget.Texture2D, 0, A.PixelInternalFormat.Rgba, data.Width, data.Height, 0,
    //            A.PixelFormat.Bgra, A.PixelType.UnsignedByte, data.Scan0);

    //        if (mipmap)
    //            A.GL.GenerateMipmap(A.GenerateMipmapTarget.Texture2D);

    //        A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureWrapS, (int)mode);
    //        A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureWrapT, (int)mode);
    //        if (mipmap)
    //            A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
    //        else
    //            A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
    //        A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
    //        A.GL.BindTexture(A.TextureTarget.Texture2D, 0);

    //        bitmap.UnlockBits(data);

    //        return id;
    //    }


    //    // Checks all material textures of a given type and loads the textures if they're not loaded yet.
    //    // The required info is returned as a Texture struct.
    //    private List<Texture> loadMaterialTextures(Assimp.Material mat, TextureType type, string typeName)
    //    {
    //        List<Texture> textures = new List<Texture>();
    //        for (int i = 0; i < mat.GetMaterialTextureCount(type); i++)
    //        {           
    //            mat.GetMaterialTexture(type, i, out TextureSlot str);
    //            // Check if texture was loaded before and if so, continue to next iteration: skip loading a new texture
    //            bool skip = false;
    //            for (int j = 0; j < texturesLoaded.Count; j++)
    //            {
    //                if (texturesLoaded[j].path == str.FilePath)
    //                {
    //                    textures.Add(texturesLoaded[j]);
    //                    skip = true; // A texture with the same filepath has already been loaded, continue to next one. (optimization)
    //                    break;
    //                }
    //            }
    //            if (skip == false)
    //            {   
    //                // If texture hasn't been loaded already, load it
    //                var texture = new Texture();

    //                var temp = /*"resources/textures/satellite/" +*/ str.FilePath;
    //                texture.id = load2DTexture(temp/*str.FilePath*/, A.TextureWrapMode.Repeat, true);// textureFromFile(str.FilePath, directory);
    //                texture.type = typeName;
    //                texture.path = str.FilePath;
    //                textures.Add(texture);
    //                texturesLoaded.Add(texture);  // Store it as texture loaded for entire model, to ensure we won't unnecesery load duplicate textures.
    //            }
    //        }
    //        return textures;
    //    }

    //    private int textureFromFile(string path, string directory)
    //    {
    //        //Generate texture ID and load texture data 
    //        string filename = path;
    //        //filename = directory + '/' + filename;
    //        int textureID = A.GL.GenTexture();

    //        var bitmap = new Bitmap(filename);

    //        var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
    //                             System.Drawing.Imaging.ImageLockMode.ReadOnly,
    //                             System.Drawing.Imaging.PixelFormat.Format32bppRgb);

    //        // Assign texture to ID
    //        A.GL.BindTexture(A.TextureTarget.Texture2D, textureID);
    //        A.GL.TexImage2D(A.TextureTarget.Texture2D, 0, A.PixelInternalFormat.Rgb,
    //            bitmap.Width, bitmap.Height, 0, A.PixelFormat.Rgb, A.PixelType.UnsignedByte, bitmapData.Scan0);
    //        A.GL.GenerateMipmap(A.GenerateMipmapTarget.Texture2D);

    //        // Parameters
    //        A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureWrapS, (int)A.TextureWrapMode.Repeat);
    //        A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureWrapT, (int)A.TextureWrapMode.Repeat);
    //        A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
    //        A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
    //        A.GL.BindTexture(A.TextureTarget.Texture2D, 0);

    //        bitmap.UnlockBits(bitmapData);

    //        return textureID;
    //    }


    //    private vec4 convertToVec4(Color4D color)
    //    {
    //        return new vec4(color.R, color.G, color.B, color.A);
    //    }


    //}
}
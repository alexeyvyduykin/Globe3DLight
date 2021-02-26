using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using System.IO;
using Globe3DLight.Geometry;
using Globe3DLight.Geometry.Models;
using A = Assimp;
using System.Linq;

namespace Globe3DLight.ModelLoader.Assimp
{

    public class AssimpModelLoader : IModelLoader
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IFactory _factory;


        IList<IMesh> _meshes;


        public AssimpModelLoader(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _factory = _serviceProvider.GetService<IFactory>();

            _ameshes = new List<IAMesh>();
        }

        public IModel LoadModel(string path, bool withTextures)
        {
            //var importer = new A.AssimpContext();
            //var scene = importer.ImportFile(path, A.PostProcessSteps.PreTransformVertices | A.PostProcessSteps.GenerateSmoothNormals);
            
            A.Scene scene;
            
            try
            {
                //     Flips face winding order from CCW (default) to CW.
                // FlipWindingOrder = 16777216,

                using (var importer = new A.AssimpContext())
                {
                    scene = importer.ImportFile(
                        path,
                        A.PostProcessSteps.PreTransformVertices |
                        A.PostProcessSteps.Triangulate |
                        A.PostProcessSteps.GenerateSmoothNormals |
                        A.PostProcessSteps.CalculateTangentSpace | 
                        A.PostProcessSteps.FlipWindingOrder);
                }
            }
            catch (Exception)
            {
                return null;
            }


            _meshes = new List<IMesh>();

            processNode(scene.RootNode, scene);

            var materials = scene.Materials.Select(s => s.ToMaterial()).ToList();

            return new Model(_meshes, materials);
        }

        private void processNode(A.Node node, A.Scene scene)
        {
            // Process each mesh located at the current node
            for (int i = 0; i < node.MeshCount; i++)
            {
                // The node object only contains indices to index the actual objects in the scene. 
                // The scene contains all the data, node is just to keep stuff organized (like relations between nodes).
                var mesh = scene.Meshes[node.MeshIndices[i]];

                _meshes.Add(mesh.ToMesh());
            }
            // After we've processed all of the meshes (if any) we then recursively process each of the children nodes
            for (int i = 0; i < node.ChildCount; i++)
            {
                processNode(node.Children[i], scene);
            }
        }

        //private Mesh processMesh(Assimp.Mesh mesh, Assimp.Scene scene)
        //{
        //    List<Vertex> vertices = new List<Vertex>();
        //    List<short> indices = new List<short>();
        //    //List<Texture> textures = new List<Texture>();
        //    Material material = new Material();

        //    // Walk through each of the mesh's vertices
        //    for (int i = 0; i < mesh.VertexCount; i++)
        //    {
        //        Vertex vertex = new Vertex();
        //        vec3 vector; // We declare a placeholder vector since assimp uses its own vector class that doesn't directly convert to glm's vec3 class so we transfer the data to this placeholder glm::vec3 first.

        //        // Positions
        //        vector.x = mesh.Vertices[i].X;
        //        vector.y = mesh.Vertices[i].Y;
        //        vector.z = mesh.Vertices[i].Z;
        //        vertex.position = vector;

        //        // Normals
        //        vector.x = mesh.Normals[i].X;
        //        vector.y = mesh.Normals[i].Y;
        //        vector.z = mesh.Normals[i].Z;
        //        vertex.normal = vector;

        //        // Texture Coordinates
        //        if (mesh.HasTextureCoords(0)) // Does the mesh contain texture coordinates?
        //        {
        //            vec2 vec;
        //            // A vertex can contain up to 8 different texture coordinates. We thus make the assumption that we won't 
        //            // use models where a vertex can have multiple texture coordinates so we always take the first set (0).
        //            vec.x = mesh.TextureCoordinateChannels[0][i].X;
        //            vec.y = mesh.TextureCoordinateChannels[0][i].Y;
        //            vertex.texCoords = vec;
        //        }
        //        else
        //            vertex.texCoords = new vec2(0.0f, 0.0f);
        //        vertices.Add(vertex);
        //    }

        //    // Now wak through each of the mesh's faces (a face is a mesh its triangle) and retrieve the corresponding vertex indices.
        //    for (int i = 0; i < mesh.FaceCount; i++)
        //    {
        //        Face face = mesh.Faces[i];
        //        // Retrieve all indices of the face and store them in the indices vector
        //        for (int j = 0; j < face.IndexCount; j++)
        //            indices.Add((short)face.Indices[j]);
        //    }

        //    // Process materials
        //    if (mesh.MaterialIndex >= 0)
        //    {
        //        var aMaterial = scene.Materials[mesh.MaterialIndex];
        //        // We assume a convention for sampler names in the shaders. Each diffuse texture should be named
        //        // as 'texture_diffuseN' where N is a sequential number ranging from 1 to MAX_SAMPLER_NUMBER. 
        //        // Same applies to other texture as the following list summarizes:
        //        // Diffuse: texture_diffuseN
        //        // Specular: texture_specularN
        //        // Normal: texture_normalN

        //        material.ambient = convertToVec4(aMaterial.ColorAmbient);
        //        material.diffuse = convertToVec4(aMaterial.ColorDiffuse);
        //        material.emission = convertToVec4(aMaterial.ColorEmissive);
        //        material.specular = convertToVec4(aMaterial.ColorSpecular);
        //        material.shininess = 20.0f;// aMaterial.Shininess;

        //        // 1. Diffuse maps
        //        List<Texture> diffuseMaps = loadMaterialTextures(aMaterial, TextureType.Diffuse, "u_textureDiffuse");
        //        //textures.InsertRange(textures.Count, diffuseMaps);
        //        if (diffuseMaps.Count != 0)
        //            material.mapDiffuse = diffuseMaps[0];
        //        // 2. Specular maps
        //        //                List<Texture> specularMaps = loadMaterialTextures(material, aiTextureType_SPECULAR, "texture_specular");
        //        //                curMesh.insert(textures.end(), specularMaps.begin(), specularMaps.end());
        //    }

        //    // Return a mesh object created from the extracted mesh data
        //    return new Mesh(vertices, indices, material);
        //}



        //public void draw(ShaderProgram program)
        //{
        //    for (int i = 0; i < _meshes.Count; i++)
        //        _meshes[i].draw(program);
        //}

        //private int load2DTexture(string path, A.TextureWrapMode mode, bool mipmap)
        //{
        //    int id = A.GL.GenTexture();
        //    Bitmap bitmap = new Bitmap(path);

        //    BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
        //        ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        //    A.GL.BindTexture(A.TextureTarget.Texture2D, id);

        //    A.GL.TexImage2D(A.TextureTarget.Texture2D, 0, A.PixelInternalFormat.Rgba, data.Width, data.Height, 0,
        //        A.PixelFormat.Bgra, A.PixelType.UnsignedByte, data.Scan0);

        //    if (mipmap)
        //        A.GL.GenerateMipmap(A.GenerateMipmapTarget.Texture2D);

        //    A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureWrapS, (int)mode);
        //    A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureWrapT, (int)mode);
        //    if (mipmap)
        //        A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
        //    else
        //        A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        //    A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        //    A.GL.BindTexture(A.TextureTarget.Texture2D, 0);

        //    bitmap.UnlockBits(data);

        //    return id;
        //}


        //// Checks all material textures of a given type and loads the textures if they're not loaded yet.
        //// The required info is returned as a Texture struct.
        //private List<Texture> loadMaterialTextures(Assimp.Material mat, TextureType type, string typeName)
        //{
        //    List<Texture> textures = new List<Texture>();
        //    for (int i = 0; i < mat.GetMaterialTextureCount(type); i++)
        //    {
        //        mat.GetMaterialTexture(type, i, out TextureSlot str);
        //        // Check if texture was loaded before and if so, continue to next iteration: skip loading a new texture
        //        bool skip = false;
        //        for (int j = 0; j < texturesLoaded.Count; j++)
        //        {
        //            if (texturesLoaded[j].path == str.FilePath)
        //            {
        //                textures.Add(texturesLoaded[j]);
        //                skip = true; // A texture with the same filepath has already been loaded, continue to next one. (optimization)
        //                break;
        //            }
        //        }
        //        if (skip == false)
        //        {
        //            // If texture hasn't been loaded already, load it
        //            var texture = new Texture();

        //            var temp = /*"resources/textures/satellite/" +*/ str.FilePath;
        //            texture.id = load2DTexture(temp/*str.FilePath*/, A.TextureWrapMode.Repeat, true);// textureFromFile(str.FilePath, directory);
        //            texture.type = typeName;
        //            texture.path = str.FilePath;
        //            textures.Add(texture);
        //            texturesLoaded.Add(texture);  // Store it as texture loaded for entire model, to ensure we won't unnecesery load duplicate textures.
        //        }
        //    }
        //    return textures;
        //}

        //private int textureFromFile(string path, string directory)
        //{
        //    //Generate texture ID and load texture data 
        //    string filename = path;
        //    //filename = directory + '/' + filename;
        //    int textureID = A.GL.GenTexture();

        //    var bitmap = new Bitmap(filename);

        //    var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
        //                         System.Drawing.Imaging.ImageLockMode.ReadOnly,
        //                         System.Drawing.Imaging.PixelFormat.Format32bppRgb);

        //    // Assign texture to ID
        //    A.GL.BindTexture(A.TextureTarget.Texture2D, textureID);
        //    A.GL.TexImage2D(A.TextureTarget.Texture2D, 0, A.PixelInternalFormat.Rgb,
        //        bitmap.Width, bitmap.Height, 0, A.PixelFormat.Rgb, A.PixelType.UnsignedByte, bitmapData.Scan0);
        //    A.GL.GenerateMipmap(A.GenerateMipmapTarget.Texture2D);

        //    // Parameters
        //    A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureWrapS, (int)A.TextureWrapMode.Repeat);
        //    A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureWrapT, (int)A.TextureWrapMode.Repeat);
        //    A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
        //    A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        //    A.GL.BindTexture(A.TextureTarget.Texture2D, 0);

        //    bitmap.UnlockBits(bitmapData);

        //    return textureID;
        //}


        //private vec4 convertToVec4(Color4D color)
        //{
        //    return new vec4(color.R, color.G, color.B, color.A);
        //}
        int ggg;

        //-----------------------------------------------

        private string _directory;
        private readonly IList<IAMesh> _ameshes;

        private IList<Texture> texturesLoaded = new List<Texture>();

        public IEnumerable<IAMesh> AMeshes => _ameshes;


        private bool withTextureMode = false;

        public bool LoadFromFile(string path, bool withTexture)
        {
            this.withTextureMode = withTexture;

            A.Scene scene;

            try
            {
                using (var importer = new A.AssimpContext())
                {
                    scene = importer.ImportFile(
                        path,
                        A.PostProcessSteps.PreTransformVertices |
                        A.PostProcessSteps.Triangulate |
                        A.PostProcessSteps.GenerateSmoothNormals |
                        A.PostProcessSteps.CalculateTangentSpace);
                }
            }
            catch (Exception)
            {
                return false;
            }

            _directory = Path.GetDirectoryName(path);

            _ameshes.Clear();

            processNode__(scene.RootNode, scene);

            return true;
        }

        private void processNode__(A.Node node, A.Scene scene)
        {


            // Process each mesh located at the current node
            for (int i = 0; i < node.MeshCount; i++)
            {
                // The node object only contains indices to index the actual objects in the scene. 
                // The scene contains all the data, node is just to keep stuff organized (like relations between nodes).
                var mesh = scene.Meshes[node.MeshIndices[i]];

                if (withTextureMode == true)
                {
                    _ameshes.Add(processMesh1__(mesh, scene));
                }
                else
                {
                    _ameshes.Add(processMesh__(mesh, scene));
                }
            }
            // After we've processed all of the meshes (if any) we then recursively process each of the children nodes
            for (int i = 0; i < node.ChildCount; i++)
            {
                processNode__(node.Children[i], scene);
            }
        }

        private IAMesh processMesh__(A.Mesh aMesh, A.Scene aScene)
        {
            var positionsAttribute = _factory.CreateVertexAttributePosition<vec3>(VertexAttributeType.FloatVector3);
            var normalsAttribute = _factory.CreateVertexAttributeNormal<vec3>(VertexAttributeType.FloatVector3);
            var texCoordsAttribute = _factory.CreateVertexAttributeTextCoord<vec2>(VertexAttributeType.FloatVector2);
            var tangentsAttribute = _factory.CreateVertexAttributeTangent<vec3>(VertexAttributeType.FloatVector3);

            IList<vec3> positions = positionsAttribute.Values;
            IList<vec3> normals = normalsAttribute.Values;
            IList<vec2> texCoords = texCoordsAttribute.Values;
            IList<vec3> tangents = tangentsAttribute.Values;

            var indicesAttr = _factory.CreateIndicesUnsignedShort();// indicesBase.Values;

            IList<ushort> indices = indicesAttr.Values;

            // Walk through each of the mesh's vertices
            for (int i = 0; i < aMesh.VertexCount; i++)
            {
                positions.Add(new vec3(aMesh.Vertices[i].X, aMesh.Vertices[i].Y, aMesh.Vertices[i].Z));
                normals.Add(new vec3(aMesh.Normals[i].X, aMesh.Normals[i].Y, aMesh.Normals[i].Z));
                tangents.Add(new vec3(aMesh.Tangents[i].X, aMesh.Tangents[i].Y, aMesh.Tangents[i].Z));

                // Texture Coordinates
                if (aMesh.HasTextureCoords(0)) // Does the mesh contain texture coordinates?
                {
                    // A vertex can contain up to 8 different texture coordinates. We thus make the assumption that we won't 
                    // use models where a vertex can have multiple texture coordinates so we always take the first set (0).
                    texCoords.Add(new vec2(aMesh.TextureCoordinateChannels[0][i].X, aMesh.TextureCoordinateChannels[0][i].Y));
                }
                else
                    texCoords.Add(vec2.Zero);
            }

            // Now wak through each of the mesh's faces (a face is a mesh its triangle) and retrieve the corresponding vertex indices.
            for (int i = 0; i < aMesh.FaceCount; i++)
            {
                var face = aMesh.Faces[i];
                // Retrieve all indices of the face and store them in the indices vector
                for (int j = 0; j < face.IndexCount; j++)
                    indices.Add((ushort)face.Indices[j]);
            }

            // Return a mesh object created from the extracted mesh data         
            var mesh = _factory.CreateMesh();

            mesh.AddAttribute(positionsAttribute);
            mesh.AddAttribute(normalsAttribute);
            mesh.AddAttribute(texCoordsAttribute);
            mesh.AddAttribute(tangentsAttribute);

            mesh.Indices = indicesAttr;

            return mesh;
        }


        private IAMesh processMesh1__(A.Mesh aMesh, A.Scene scene)
        {
            List<Vertex> vertices = new List<Vertex>();
            List<short> indices = new List<short>();
            //List<Texture> textures = new List<Texture>();
            AMaterial material = new AMaterial();

            // Walk through each of the mesh's vertices
            for (int i = 0; i < aMesh.VertexCount; i++)
            {
                Vertex vertex = new Vertex();
                vec3 vector; // We declare a placeholder vector since assimp uses its own vector class that doesn't directly convert to glm's vec3 class so we transfer the data to this placeholder glm::vec3 first.

                // Positions
                vector.x = aMesh.Vertices[i].X;
                vector.y = aMesh.Vertices[i].Y;
                vector.z = aMesh.Vertices[i].Z;
                vertex.Position = vector;

                // Normals
                vector.x = aMesh.Normals[i].X;
                vector.y = aMesh.Normals[i].Y;
                vector.z = aMesh.Normals[i].Z;
                vertex.Normal = vector;

                // Texture Coordinates
                if (aMesh.HasTextureCoords(0)) // Does the mesh contain texture coordinates?
                {
                    vec2 vec;
                    // A vertex can contain up to 8 different texture coordinates. We thus make the assumption that we won't 
                    // use models where a vertex can have multiple texture coordinates so we always take the first set (0).
                    vec.x = aMesh.TextureCoordinateChannels[0][i].X;
                    vec.y = aMesh.TextureCoordinateChannels[0][i].Y;
                    vertex.TexCoords = vec;
                }
                else
                    vertex.TexCoords = new vec2(0.0f, 0.0f);
                vertices.Add(vertex);
            }

            // Now wak through each of the mesh's faces (a face is a mesh its triangle) and retrieve the corresponding vertex indices.
            for (int i = 0; i < aMesh.FaceCount; i++)
            {
                var face = aMesh.Faces[i];
                // Retrieve all indices of the face and store them in the indices vector
                for (int j = 0; j < face.IndexCount; j++)
                    indices.Add((short)face.Indices[j]);
            }

            // Process materials
            if (aMesh.MaterialIndex >= 0)
            {
                var aMaterial = scene.Materials[aMesh.MaterialIndex];
                // We assume a convention for sampler names in the shaders. Each diffuse texture should be named
                // as 'texture_diffuseN' where N is a sequential number ranging from 1 to MAX_SAMPLER_NUMBER. 
                // Same applies to other texture as the following list summarizes:
                // Diffuse: texture_diffuseN
                // Specular: texture_specularN
                // Normal: texture_normalN


                material.Ambient = aMaterial.ColorAmbient.ToVec4();
                material.Diffuse = aMaterial.ColorDiffuse.ToVec4();
                material.Emission = aMaterial.ColorEmissive.ToVec4();
                material.Specular = aMaterial.ColorSpecular.ToVec4();
                material.Shininess = 20.0f;// aMaterial.Shininess;

                // 1. Diffuse maps
                List<Texture> diffuseMaps = loadMaterialTextures(aMaterial, A.TextureType.Diffuse, "u_textureDiffuse");
                //textures.InsertRange(textures.Count, diffuseMaps);
                if (diffuseMaps.Count != 0)
                    material.MapDiffuse = diffuseMaps[0];
                // 2. Specular maps
                //                List<Texture> specularMaps = loadMaterialTextures(material, aiTextureType_SPECULAR, "texture_specular");
                //                curMesh.insert(textures.end(), specularMaps.begin(), specularMaps.end());
            }

            // Return a mesh object created from the extracted mesh data
            var mesh = _factory.CreateMesh();
            mesh.vertices = vertices;
            mesh.indices = indices;
            mesh.material = material;

            return mesh;
        }

        //private int load2DTexture(string path, OpenTK.Graphics.OpenGL.TextureWrapMode mode, bool mipmap)
        //{
        //    int id = GL.GenTexture();
        //    Bitmap bitmap = new Bitmap(path);

        //    BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
        //        ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        //    GL.BindTexture(TextureTarget.Texture2D, id);

        //    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
        //        OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

        //    if (mipmap)
        //        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        //    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)mode);
        //    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)mode);
        //    if (mipmap)
        //        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
        //    else
        //        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        //    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        //    GL.BindTexture(TextureTarget.Texture2D, 0);

        //    bitmap.UnlockBits(data);

        //    return id;
        //}


        // Checks all material textures of a given type and loads the textures if they're not loaded yet.
        // The required info is returned as a Texture struct.
        private List<Texture> loadMaterialTextures(A.Material mat, A.TextureType type, string typeName)
        {
            List<Texture> textures = new List<Texture>();
            for (int i = 0; i < mat.GetMaterialTextureCount(type); i++)
            {
                mat.GetMaterialTexture(type, i, out A.TextureSlot str);
                // Check if texture was loaded before and if so, continue to next iteration: skip loading a new texture
                bool skip = false;
                for (int j = 0; j < texturesLoaded.Count; j++)
                {
                    if (texturesLoaded[j].Path == str.FilePath)
                    {
                        textures.Add(texturesLoaded[j]);
                        skip = true; // A texture with the same filepath has already been loaded, continue to next one. (optimization)
                        break;
                    }
                }

                if (skip == false)
                {   // If texture hasn't been loaded already, load it
                    Texture texture = new Texture();

                    //var temp = "resources/textures/satellite/" + str.FilePath;
                    // !!!!!!!                   texture.Id = load2DTexture(temp/*str.FilePath*/, OpenTK.Graphics.OpenGL.TextureWrapMode.Repeat, true);// textureFromFile(str.FilePath, directory);
                    texture.Type = typeName;
                    texture.Path = str.FilePath;
                    textures.Add(texture);
                    texturesLoaded.Add(texture);  // Store it as texture loaded for entire model, to ensure we won't unnecesery load duplicate textures.
                }
            }
            return textures;
        }

        //private int textureFromFile(string path, string directory)
        //{
        //    //Generate texture ID and load texture data 
        //    string filename = path;
        //    //filename = directory + '/' + filename;
        //    int textureID = GL.GenTexture();

        //    var bitmap = new Bitmap(filename);

        //    var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
        //                         System.Drawing.Imaging.ImageLockMode.ReadOnly,
        //                         System.Drawing.Imaging.PixelFormat.Format32bppRgb);

        //    // Assign texture to ID
        //    GL.BindTexture(TextureTarget.Texture2D, textureID);
        //    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb,
        //        bitmap.Width, bitmap.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedByte, bitmapData.Scan0);
        //    GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        //    // Parameters
        //    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.Repeat);
        //    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.Repeat);
        //    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
        //    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        //    GL.BindTexture(TextureTarget.Texture2D, 0);

        //    bitmap.UnlockBits(bitmapData);

        //    return textureID;
        //}



    }
}

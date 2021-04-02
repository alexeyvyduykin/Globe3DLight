using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GlmSharp;
using Globe3DLight.Models;
using Globe3DLight.ViewModels.Geometry;
using A = Assimp;

namespace Globe3DLight.ModelLoader.Assimp
{
    public class AssimpModelLoader : IModelLoader
    {
        private readonly IServiceProvider _serviceProvider;
        private IList<Mesh> _meshes;

        public AssimpModelLoader(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Model LoadModel(string path, bool flipUVs)
        {
            A.Scene scene;

            try
            {
                using var importer = new A.AssimpContext();

                var flags =
                    A.PostProcessSteps.PreTransformVertices |
                    A.PostProcessSteps.Triangulate |
                    A.PostProcessSteps.GenerateSmoothNormals |
                    A.PostProcessSteps.CalculateTangentSpace |
                    A.PostProcessSteps.FlipWindingOrder;

                if (flipUVs == true)
                { 
                    flags |= A.PostProcessSteps.FlipUVs; 
                }

                scene = importer.ImportFile(path, flags);
            }
            catch (Exception)
            {
                return null;
            }

            _meshes = new List<Mesh>();

            processNode(scene.RootNode, scene);

            string currentDirectory = Path.GetDirectoryName(path);
            string modelPath = Path.GetFullPath(currentDirectory);

            var materials = CreateMaterials(modelPath, scene.Materials);

            return new Model()
            { 
                Meshes =_meshes,                
                Materials = materials
            };
        }

        private void processNode(A.Node node, A.Scene scene)
        {
            // Process each mesh located at the current node
            for (int i = 0; i < node.MeshCount; i++)
            {
                // The node object only contains indices to index the actual objects in the scene. 
                // The scene contains all the data, node is just to keep stuff organized (like relations between nodes).
                var mesh = scene.Meshes[node.MeshIndices[i]];

                _meshes.Add(CreateMesh(mesh));
            }
            // After we've processed all of the meshes (if any) we then recursively process each of the children nodes
            for (int i = 0; i < node.ChildCount; i++)
            {
                processNode(node.Children[i], scene);
            }
        }

        private IList<Material> CreateMaterials(string modelPath, IEnumerable<A.Material> materials)
        {
            var list = new List<Material>();

            foreach (var item in materials)
            {
                list.Add(new Material()
                {
                    Ambient = item.ColorAmbient.ToVec4(),
                    Diffuse = item.ColorDiffuse.ToVec4(),
                    Emission = item.ColorEmissive.ToVec4(),
                    Specular = item.ColorSpecular.ToVec4(),
                    Shininess = item.Shininess,
                    HasTextureDiffuse = item.HasTextureDiffuse,
                    TextureDiffusePath = (item.HasTextureDiffuse) ? Path.Combine(modelPath, item.TextureDiffuse.FilePath) : string.Empty,
                    TextureDiffuseKey = (item.HasTextureDiffuse) ? Path.GetFileNameWithoutExtension(item.TextureDiffuse.FilePath) : string.Empty,
                });
            }

            return list;
        }

        private Mesh CreateMesh(A.Mesh mesh)
        {
            var vertices = mesh.Vertices.Select(s => new vec3(s.X, s.Y, s.Z)).ToList();
            var normals = mesh.Normals.Select(s => new vec3(s.X, s.Y, s.Z)).ToList();
            var tangents = mesh.Tangents.Select(s => new vec3(s.X, s.Y, s.Z)).ToList();
            var isTextureCoords = mesh.HasTextureCoords(0);
            var texCoords = (isTextureCoords == true) ?
                mesh.TextureCoordinateChannels[0].Select(s => new vec2(s.X, s.Y)).ToList() :
                Enumerable.Repeat(vec2.Zero, mesh.VertexCount).ToList();

            var indices = new List<ushort>();

            foreach (var item in mesh.Faces.SelectMany(s => s.Indices))
            {
                indices.Add((ushort)item);
            }

            return new Mesh()
            {
                Vertices = vertices,
                Normals = normals,
                TexCoords = texCoords,
                Tangents = tangents,
                Indices = indices,
                MaterialIndex = mesh.MaterialIndex,
            };
        }
    }
}

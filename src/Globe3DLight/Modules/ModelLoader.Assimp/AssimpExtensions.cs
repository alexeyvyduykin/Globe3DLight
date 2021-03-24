using System;
using System.Collections.Generic;
using System.Text;
using A = Assimp;
using GlmSharp;
using Globe3DLight.Models.Geometry.Models;
using System.Linq;
using System.IO;
using Globe3DLight.ViewModels.Geometry.Models;

namespace Globe3DLight.ModelLoader.Assimp
{
    internal static class AssimpExtensions
    {
        public static vec4 ToVec4(this A.Color4D color)
        {
            return new vec4(color.R, color.G, color.B, color.A);
        }


        public static IMesh ToMesh(this A.Mesh mesh)
        {
            var vertices = mesh.Vertices.Select(s => new vec3(s.X, s.Y, s.Z)).ToList();
            var normals = mesh.Normals.Select(s => new vec3(s.X, s.Y, s.Z)).ToList();
            var tangents = mesh.Tangents.Select(s => new vec3(s.X, s.Y, s.Z)).ToList();

            var isTextureCoords = mesh.HasTextureCoords(0);
            var texCoords = (isTextureCoords == true) ?
                mesh.TextureCoordinateChannels[0].Select(s => new vec2(s.X, s.Y)).ToList() : Enumerable.Repeat(vec2.Zero, mesh.VertexCount).ToList();

            {
                //// Walk through each of the mesh's vertices
                //for (int i = 0; i < mesh.VertexCount; i++)
                //{
                //    // We declare a placeholder vector since assimp uses its own vector class that doesn't directly convert to glm's vec3 class so we transfer the data to this placeholder glm::vec3 first.

                //    // Positions
                //    vertices.Add(new vec3(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z));

                //    // Normals
                //    normals.Add(new vec3(mesh.Normals[i].X, mesh.Normals[i].Y, mesh.Normals[i].Z));

                //    // Texture Coordinates
                //    if (mesh.HasTextureCoords(0) == true) // Does the mesh contain texture coordinates?
                //    {                
                //        // A vertex can contain up to 8 different texture coordinates. We thus make the assumption that we won't 
                //        // use models where a vertex can have multiple texture coordinates so we always take the first set (0).
                //        texCoords.Add(new vec2(mesh.TextureCoordinateChannels[0][i].X, mesh.TextureCoordinateChannels[0][i].Y));
                //    }
                //    else
                //        texCoords.Add(new vec2(0.0f, 0.0f));            
                //}
            }
            {
                //// Now wak through each of the mesh's faces (a face is a mesh its triangle) and retrieve the corresponding vertex indices.
                //for (int i = 0; i < mesh.FaceCount; i++)
                //{
                //    var face = mesh.Faces[i];
                //    // Retrieve all indices of the face and store them in the indices vector
                //    for (int j = 0; j < face.IndexCount; j++)
                //        indices.Add((short)face.Indices[j]);
                //}
            }

            //var indices = mesh.Faces.SelectMany(s => s.Indices).Cast<short>().ToList();

            var indices = new List<ushort>();

            foreach (var item in mesh.Faces.SelectMany(s => s.Indices))
            {                   
                indices.Add((ushort)item);                
            }
        
            {
                //// Process materials
                //if (mesh.MaterialIndex >= 0)
                //{
                //    var aMaterial = scene.Materials[mesh.MaterialIndex];
                //    // We assume a convention for sampler names in the shaders. Each diffuse texture should be named
                //    // as 'texture_diffuseN' where N is a sequential number ranging from 1 to MAX_SAMPLER_NUMBER. 
                //    // Same applies to other texture as the following list summarizes:
                //    // Diffuse: texture_diffuseN
                //    // Specular: texture_specularN
                //    // Normal: texture_normalN

                //    material.ambient = convertToVec4(aMaterial.ColorAmbient);
                //    material.diffuse = convertToVec4(aMaterial.ColorDiffuse);
                //    material.emission = convertToVec4(aMaterial.ColorEmissive);
                //    material.specular = convertToVec4(aMaterial.ColorSpecular);
                //    material.shininess = 20.0f;// aMaterial.Shininess;

                //    // 1. Diffuse maps
                //    var diffuseMaps = loadMaterialTextures(aMaterial, TextureType.Diffuse, "u_textureDiffuse");
                //    //textures.InsertRange(textures.Count, diffuseMaps);
                //    if (diffuseMaps.Count != 0)
                //        material.mapDiffuse = diffuseMaps[0];
                //    // 2. Specular maps
                //    //                List<Texture> specularMaps = loadMaterialTextures(material, aiTextureType_SPECULAR, "texture_specular");
                //    //                curMesh.insert(textures.end(), specularMaps.begin(), specularMaps.end());
                //}
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

        public static IMaterial ToMaterial(this A.Material material)
        {
            vec4 ToColor(A.Color4D color)
            {
                return new vec4(color.R, color.G, color.B, color.A);
            }


            return new Material()
            {
                Ambient = ToColor(material.ColorAmbient),
                Diffuse = ToColor(material.ColorDiffuse),
                Emission = ToColor(material.ColorEmissive),
                Specular = ToColor(material.ColorSpecular),
                Shininess = material.Shininess,// 20.0f,
                HasTextureDiffuse = material.HasTextureDiffuse,
                TextureDiffusePath = (material.HasTextureDiffuse) ? 
                    material.TextureDiffuse.FilePath : string.Empty,
                TextureDiffuseKey = (material.HasTextureDiffuse) ? 
                    Path.GetFileNameWithoutExtension(material.TextureDiffuse.FilePath) : string.Empty,
            };
        }
    }
}

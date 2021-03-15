using Globe3DLight.Geometry;
using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Containers;
using System.Collections.Immutable;
using Globe3DLight.Scene;
using Globe3DLight.Renderer;
using Spatial;
using Globe3DLight.Style;
using System.Linq;
using Globe3DLight.ScenarioObjects;
using Globe3DLight.Data;
using System.IO;
using Globe3DLight.Editor;
using Globe3DLight.SceneTimer;
using Globe3DLight.Time;
using Globe3DLight.Timer;


namespace Globe3DLight
{
    public class Factory : IFactory
    {
        public ILibrary<T> CreateLibrary<T>(string name)
        {
            return new Library<T>()
            {
                Name = name,
                Items = ImmutableArray.Create<T>(),
                Selected = default
            };
        }
  
        public ILibrary<T> CreateLibrary<T>(string name, IEnumerable<T> items)
        {
            return new Library<T>()
            {
                Name = name,
                Items = ImmutableArray.CreateRange<T>(items),
                Selected = items.FirstOrDefault()
            };
        }

        public ICache<TKey, TValue> CreateCache<TKey, TValue>(Action<TValue> dispose = null)
        {
            return new Cache<TKey, TValue>(dispose);
        }

        //public IShapeRendererState CreateShapeRendererState()
        //{
        //    var state = new ShapeRendererState()
        //    {
        //        PanX = 0.0,
        //        PanY = 0.0,
        //        ZoomX = 1.0,
        //        ZoomY = 1.0,
        //        DrawShapeState = CreateShapeState(ShapeStateFlags.Visible),
        //        SelectedShapes = default
        //    };

        //    state.SelectionStyle =
        //        CreateShapeStyle(
        //            "Selection",
        //            0x7F, 0x33, 0x33, 0xFF,
        //            0x4F, 0x33, 0x33, 0xFF,
        //            1.0);

        //    state.HelperStyle =
        //        CreateShapeStyle(
        //            "Helper",
        //            0xFF, 0x00, 0xBF, 0xFF,
        //            0xFF, 0x00, 0xBF, 0xFF,
        //            1.0);

        //    state.DrawDecorators = true;
        //    state.DrawPoints = true;

        //    state.PointStyle =
        //        CreateShapeStyle(
        //            "Point",
        //            0xFF, 0x00, 0xBF, 0xFF,
        //            0xFF, 0xFF, 0xFF, 0xFF,
        //            2.0);
        //    state.SelectedPointStyle =
        //        CreateShapeStyle(
        //            "SelectionPoint",
        //            0xFF, 0x00, 0xBF, 0xFF,
        //            0xFF, 0x00, 0xBF, 0xFF,
        //            2.0);
        //    state.PointSize = 4.0;

        //    return state;
        //}


        public IVertexAttribute CreateVertexAttributePosition()
        {
            return new VertexAttribute<vec3>("POSITION", VertexAttributeType.FloatVector3);
        }

        public IVertexAttribute CreateVertexAttributeNormal()
        {
            return new VertexAttribute<vec3>("NORMAL", VertexAttributeType.FloatVector3);
        }

        public IVertexAttribute CreateVertexAttributeTextCoord()
        {
            return new VertexAttribute<vec2>("TEXCOORD", VertexAttributeType.FloatVector2);
        }

        public IVertexAttribute CreateVertexAttributeTangent()
        {
            return new VertexAttribute<vec3>("TANGENT", VertexAttributeType.FloatVector3);
        }

        public IVertexAttribute CreateVertexAttributeColor()
        {
            return new VertexAttribute<vec4>("COLOR", VertexAttributeType.FloatVector4);           
        }

        public IVertexAttribute<T> CreateVertexAttributePosition<T>(VertexAttributeType type)
        {
            return new VertexAttribute<T>("POSITION", type/*VertexAttributeType.FloatVector3*/);
        }

        public IVertexAttribute<T> CreateVertexAttributeNormal<T>(VertexAttributeType type)
        {
            return new VertexAttribute<T>("NORMAL", type/*VertexAttributeType.FloatVector3*/);
        }

        public IVertexAttribute<T> CreateVertexAttributeTextCoord<T>(VertexAttributeType type)
        {
            return new VertexAttribute<T>("TEXCOORD", type/*VertexAttributeType.FloatVector2*/);
        }
        public IVertexAttribute<T> CreateVertexAttributeTangent<T>(VertexAttributeType type)
        {
            return new VertexAttribute<T>("TANGENT", type/*VertexAttributeType.FloatVector3*/);
        }

        public IIndices<ushort> CreateIndicesUnsignedShort()
        {
            return new IndicesUnsignedShort();
        }

        public IIndices<uint> CreateIndicesUnsignedInt()
        {
            return new IndicesUnsignedInt();
        }

        public IAMesh CreateMesh()
        {
       //     IVertexAttribute positionsAttribute = CreateVertexAttributePosition();
       //     IVertexAttribute normalsAttribute = CreateVertexAttributeNormal();
       //     IVertexAttribute texCoordsAttribute = CreateVertexAttributeTextCoord();
       //     IVertexAttribute tangentsAttribute = CreateVertexAttributeTangent();

      //      IndicesUnsignedShort indicesBase = new IndicesUnsignedShort();

      //      mesh.Attributes.Add(positionsAttribute);
       //     mesh.Attributes.Add(normalsAttribute);
      //      mesh.Attributes.Add(texCoordsAttribute);
      //      mesh.Attributes.Add(tangentsAttribute);

      //      mesh.Indices = indicesBase;
    
            return new AMesh() 
            {
                PrimitiveType = PrimitiveType.Triangles,
                FrontFaceWindingOrder = FrontFaceDirection.Cw,
                Attributes = ImmutableArray.Create<IVertexAttribute>()       
            };
        }


        public IAMesh CreateBillboard()
        {
            var billboard = CreateMesh();

            var billboardVertices = new vec2[4] 
            {
                new vec2(-1.0f, -1.0f),
                new vec2(-1.0f, 1.0f),
                new vec2(1.0f, 1.0f),
                new vec2(1.0f, -1.0f)
            };

            var billboardIndices = new ushort[6] { 0, 1, 2, 0, 2, 3 };

            var positionsAttribute = CreateVertexAttributePosition<vec2>(VertexAttributeType.FloatVector2);
            billboard.AddAttribute(positionsAttribute);

            var indicesBase = new IndicesUnsignedShort();
            billboard.Indices = indicesBase;

            billboard.PrimitiveType = PrimitiveType.Triangles;
            billboard.FrontFaceWindingOrder = FrontFaceDirection.Cw;

            IList<vec2> positions = positionsAttribute.Values;
            IList<ushort> indices = indicesBase.Values;

            for (int i = 0; i < billboardVertices.Length; i++)
                positions.Add(billboardVertices[i]);

            for (int i = 0; i < billboardIndices.Length; i++)
                indices.Add(billboardIndices[i]);

            return billboard;
        }


        public IAMesh CreateCube(float width)
        {
            var cube = CreateMesh();

            vec3[] cubeVertices = {
                new vec3( 1.0f, -1.0f, -1.0f) * width,
                new vec3( 1.0f, -1.0f,  1.0f) * width,
                new vec3( 1.0f,  1.0f,  1.0f) * width,
                new vec3( 1.0f,  1.0f, -1.0f) * width,
                new vec3(-1.0f, -1.0f,  1.0f) * width,
                new vec3(-1.0f, -1.0f, -1.0f) * width,
                new vec3(-1.0f,  1.0f, -1.0f) * width,
                new vec3(-1.0f,  1.0f,  1.0f) * width
            };

            ushort[] cubeIndices = {
                    0, 1, 2,  // x_pos
                    2, 3, 0,  //
                    4, 5, 6,  // x_neg
                    6, 7, 4,  //
                    6, 3, 2,  // y_pos
                    2, 7, 6,  //
                    0, 5, 4,  // y_neg
                    4, 1, 0,  //
                    1, 4, 7,  // z_pos
                    7, 2, 1,  //
                    5, 0, 3,  // z_neg
                    3, 6, 5   //
                };

            var positionsAttribute = CreateVertexAttributePosition<vec3>(VertexAttributeType.FloatVector3);
            cube.AddAttribute(positionsAttribute);

            IndicesUnsignedShort indicesBase = new IndicesUnsignedShort();
            cube.Indices = indicesBase;

            cube.PrimitiveType = PrimitiveType.Triangles;
            cube.FrontFaceWindingOrder = FrontFaceDirection.Cw;

            IList<vec3> positions = positionsAttribute.Values;
            IList<ushort> indices = indicesBase.Values;

            //positions = cubeVertices;
            //indices = cubeIndices;

            for (int i = 0; i < cubeVertices.Length; i++)
                positions.Add(cubeVertices[i]);

            for (int i = 0; i < cubeIndices.Length; i++)
                indices.Add(cubeIndices[i]);


            return cube;
        }

        public IAMesh CreateSolidSphere(float radius, int rings, int sectors)
        {
            var sphere = CreateMesh();

            var positionsAttribute = CreateVertexAttributePosition<vec3>(VertexAttributeType.FloatVector3);
            sphere.AddAttribute(positionsAttribute);

            IndicesUnsignedShort indicesBase = new IndicesUnsignedShort();
            sphere.Indices = indicesBase;

            sphere.PrimitiveType = PrimitiveType.Triangles;
            sphere.FrontFaceWindingOrder = FrontFaceDirection.Ccw;

            IList<vec3> positions = positionsAttribute.Values;
            IList<ushort> indices = indicesBase.Values;

          //  List<vec3> positions = new List<vec3>();
            List<vec2> texcoords = new List<vec2>();
            List<vec3> normals = new List<vec3>();
          //  List<ushort> indices = new List<ushort>();

            double R = 1.0 / (double)(rings - 1);
            double S = 1.0 / (double)(sectors - 1);
            int r, s;

            for (r = 0; r < rings; r++)
            {
                for (s = 0; s < sectors; s++)
                {
                    double y = Math.Sin((-Math.PI / 2.0 + Math.PI * r * R));
                    double x = Math.Cos(2.0 * Math.PI * s * S) * Math.Sin(Math.PI * r * R);
                    double z = Math.Sin(2.0 * Math.PI * s * S) * Math.Sin(Math.PI * r * R);

                    positions.Add(new vec3((float)x * radius, (float)y * radius, (float)z * radius));
                    texcoords.Add(new vec2((float)(s * S), (float)(r * R)));
                    normals.Add(new vec3((float)x, (float)y, (float)z));
                }
            }

            for (r = 0; r < rings - 1; r++)
            {
                for (s = 0; s < sectors - 1; s++)
                {
                    //indices.Add((ushort)(r * sectors + s));
                    //indices.Add((ushort)(r * sectors + (s + 1)));
                    //indices.Add((ushort)((r + 1) * sectors + (s + 1)));
                    //indices.Add((ushort)((r + 1) * sectors + s));

                    indices.Add((ushort)(r * sectors + s));
                    indices.Add((ushort)(r * sectors + (s + 1)));
                    indices.Add((ushort)((r + 1) * sectors + (s + 1)));

                    indices.Add((ushort)((r + 1) * sectors + (s + 1)));
                    indices.Add((ushort)((r + 1) * sectors + s));
                    indices.Add((ushort)(r * sectors + s));
                }
            }

            //for (int i = 0; i < positions.Count; i++)
            //    positionsRef.Add(positions[i]);

            //for (int i = 0; i < indices.Count; i++)
            //    indicesRef.Add(indices[i]);

            return sphere;
        }



        public IProjectContainer CreateProjectContainer(string name = "Project")
        {
            return new ProjectContainer()
            {
                Name = name,                       
                Scenarios = ImmutableArray.Create<IScenarioContainer>()
            };
        }


        public IScenarioContainer CreateScenarioContainer(string name = "Scenario")
        {
            return new ScenarioContainer()
            {
                Name = name,   
                LogicalTreeNodeRoot = ImmutableArray.Create<ILogical>(),                   
                ScenarioObjects = ImmutableArray.Create<IScenarioObject>(),                
            };
        }



        public ILogical CreateLogical(string name, IState state)
        {
            return new Logical()
            {
                Name = name,
                Children = ImmutableArray.Create<IObservableObject>(),
                State = state,              
            };
        }

        public ILogicalCollection CreateLogicalCollection(string name)
        {          
            var builder = ImmutableArray.CreateBuilder<ILogical>();

            return new LogicalCollection()
            {
                Name = name,             
                //State = states,
                Values = builder.ToImmutable(),
            };
        }

        private ITimer CreateAcceleratedTimer()
        {
            return new AcceleratedTimer();
        }

        public ITimePresenter CreateTimePresenter(DateTime dateTime, TimeSpan timeSpan)
        {
            var timer = CreateAcceleratedTimer();

            return new TimePresenter(dateTime, timeSpan)
            {
                Timer = timer,            
            };
        }

        public IDataUpdater CreateDataUpdater()
        {     
            return new DataUpdater();
        }

        public void SaveProjectContainer(IProjectContainer project, string path, IFileSystem fileIO, IJsonSerializer serializer)
        {
            //if (project is IImageCache imageCache)
            {
                using var stream = fileIO.Create(path);
                SaveProjectContainer(project/*, imageCache*/, stream, fileIO, serializer);
            }
        }

        public IProjectContainer OpenProjectContainer(string path, IFileSystem fileIO, IJsonSerializer serializer)
        {
            using var stream = fileIO.Open(path);
            return OpenProjectContainer(stream, fileIO, serializer);
        }

        public IProjectContainer OpenProjectContainer(Stream stream, IFileSystem fileIO, IJsonSerializer serializer)
        {
            //using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
            //var projectEntry = archive.Entries.FirstOrDefault(e => e.FullName == "Project.json");
            var project = ReadProjectContainer(stream, fileIO, serializer);
            return project;
        }
        //public IProjectContainer OpenProjectContainer(Stream stream, IFileSystem fileIO, IJsonSerializer serializer)
        //{
        //    using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
        //    var projectEntry = archive.Entries.FirstOrDefault(e => e.FullName == "Project.json");
        //    var project = ReadProjectContainer(projectEntry, fileIO, serializer);
        //    return project;
        //}

        public void SaveProjectContainer(IProjectContainer project, Stream stream, IFileSystem fileIO, IJsonSerializer serializer)
        {
            //using var archive = new ZipArchive(stream, ZipArchiveMode.Create);
            //var projectEntry = archive.CreateEntry("Project.json");
            WriteProjectContainer(project, stream, fileIO, serializer);
        }
        //public void SaveProjectContainer(IProjectContainer project/*, IImageCache imageCache*/, Stream stream, IFileSystem fileIO, IJsonSerializer serializer)
        //{
        //    using var archive = new ZipArchive(stream, ZipArchiveMode.Create);
        //    var projectEntry = archive.CreateEntry("Project.json");
        //    WriteProjectContainer(project, projectEntry, fileIO, serializer);
        //}
        private IProjectContainer ReadProjectContainer(Stream stream, IFileSystem fileIO, IJsonSerializer serializer)
        {    
            return serializer.Deserialize<ProjectContainer>(fileIO.ReadUtf8Text(stream));
        }
        //private IProjectContainer ReadProjectContainer(ZipArchiveEntry projectEntry, IFileSystem fileIO, IJsonSerializer serializer)
        //{
        //    using var entryStream = projectEntry.Open();
        //    return serializer.Deserialize<ProjectContainer>(fileIO.ReadUtf8Text(entryStream));
        //}

        private void WriteProjectContainer(IProjectContainer project, Stream stream, IFileSystem fileIO, IJsonSerializer serializer)
        {
            //using var jsonStream = projectEntry.Open();
            fileIO.WriteUtf8Text(stream, serializer.Serialize(project));
        }
        //private void WriteProjectContainer(IProjectContainer project, ZipArchiveEntry projectEntry, IFileSystem fileIO, IJsonSerializer serializer)
        //{
        //    using var jsonStream = projectEntry.Open();
        //    fileIO.WriteUtf8Text(jsonStream, serializer.Serialize(project));
        //}
    }
}

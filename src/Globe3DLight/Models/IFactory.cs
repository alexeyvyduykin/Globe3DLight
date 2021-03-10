using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Geometry;
using Globe3DLight.Containers;
using Globe3DLight.Scene;
using Globe3DLight.Renderer;
using Globe3DLight.Data;
using System.IO;
using Globe3DLight.Time;

namespace Globe3DLight
{
    public interface IFactory
    {
        ILibrary<T> CreateLibrary<T>(string name);

        ILibrary<T> CreateLibrary<T>(string name, IEnumerable<T> items);

        ICache<TKey, TValue> CreateCache<TKey, TValue>(Action<TValue> dispose = null);
   //     IShapeRendererState CreateShapeRendererState();

        IVertexAttribute CreateVertexAttributePosition();

        IVertexAttribute CreateVertexAttributeNormal();

        IVertexAttribute CreateVertexAttributeTextCoord();

        IVertexAttribute CreateVertexAttributeTangent();

        IVertexAttribute CreateVertexAttributeColor();

        IVertexAttribute<T> CreateVertexAttributePosition<T>(VertexAttributeType type);

        IVertexAttribute<T> CreateVertexAttributeNormal<T>(VertexAttributeType type);

        IVertexAttribute<T> CreateVertexAttributeTextCoord<T>(VertexAttributeType type);
        IVertexAttribute<T> CreateVertexAttributeTangent<T>(VertexAttributeType type);

        IIndices<ushort> CreateIndicesUnsignedShort();
        IIndices<uint> CreateIndicesUnsignedInt();



        IAMesh CreateMesh();

        IAMesh CreateBillboard();

        IAMesh CreateCube(float width);
        IAMesh CreateSolidSphere(float radius, int rings, int sectors);

        IProjectContainer CreateProjectContainer(string name = "Project");

        IScenarioContainer CreateScenarioContainer(string name = "Scenario");

        ILogical CreateLogical(string name, IState state);
       
        ILogicalCollection CreateLogicalCollection(string name);

        ITimePresenter CreateTimePresenter(DateTime dateTime, TimeSpan timeSpan);
        //IRenderState CreateRenderState();

        // ISceneState CreateSceneState();


        //   ICamera CreateArcBallCamera();

        void SaveProjectContainer(IProjectContainer project, string path, IFileSystem fileIO, IJsonSerializer serializer);
     
        IProjectContainer OpenProjectContainer(string path, IFileSystem fileIO, IJsonSerializer serializer);


        IProjectContainer OpenProjectContainer(Stream stream, IFileSystem fileIO, IJsonSerializer serializer);

        void SaveProjectContainer(IProjectContainer project/*, IImageCache imageCache*/, Stream stream, IFileSystem fileIO, IJsonSerializer serializer);

    }
}

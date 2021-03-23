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
        Library<T> CreateLibrary<T>(string name);

        Library<T> CreateLibrary<T>(string name, IEnumerable<T> items);

        ICache<TKey, TValue> CreateCache<TKey, TValue>(Action<TValue> dispose = null);

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

        ProjectContainer CreateProjectContainer(string name = "Project");

        ScenarioContainer CreateScenarioContainer(string name = "Scenario");

        Logical CreateLogical(string name, IState state);
       
        LogicalCollection CreateLogicalCollection(string name);

        SliderTimePresenter CreateSliderTimePresenter(DateTime dateTime, TimeSpan timeSpan);

        IDataUpdater CreateDataUpdater();

        void SaveProjectContainer(ProjectContainer project, string path, IFileSystem fileIO, IJsonSerializer serializer);
     
        ProjectContainer OpenProjectContainer(string path, IFileSystem fileIO, IJsonSerializer serializer);

        ProjectContainer OpenProjectContainer(Stream stream, IFileSystem fileIO, IJsonSerializer serializer);

        void SaveProjectContainer(ProjectContainer project/*, IImageCache imageCache*/, Stream stream, IFileSystem fileIO, IJsonSerializer serializer);
    }
}

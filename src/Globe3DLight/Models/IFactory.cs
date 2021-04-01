﻿using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Models.Geometry;
using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels.Scene;
using Globe3DLight.Models.Renderer;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.Models.Data;
using System.IO;
using Globe3DLight.ViewModels.Time;
using Globe3DLight.ViewModels.Entities;
using Globe3DLight.Models.Scene;
using System.Linq;
using System.Collections.Immutable;
using GlmSharp;
using System.Threading;
using Globe3DLight.ViewModels.Geometry.Models;

namespace Globe3DLight.Models
{
    public interface IFactory
    {
        FrameState CreateFrameState(string name);

        Spacebox CreateSpacebox(BaseState parent);

        Sun CreateSun(SunData data, BaseState parent);

        Earth CreateEarth(EarthData data, BaseState parent);

        EntityList CreateGroundObjects(ScenarioData data, BaseState parent);

        EntityList CreateGroundStations(ScenarioData data, BaseState parent);

        EntityList CreateRetranslators(ScenarioData data, BaseState parent);

        IList<Satellite> CreateSatellites(ScenarioData data, BaseState parent, EntityList gss, EntityList rtrs);
        
        IList<SatelliteTask> CreateSatelliteTasks(IList<Satellite> satellites, ScenarioData data);
        
        LibraryViewModel<T> CreateLibrary<T>(string name);

        LibraryViewModel<T> CreateLibrary<T>(string name, IEnumerable<T> items);

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

        Mesh CreateBillboard();

        Mesh CreateCube(double scale);

        Mesh CreateSolidSphere(double radius, int rings, int sectors);

        ISceneState CreateSceneState();

        ICamera CreateArcballCamera(dvec3 eye);

        ProjectContainerViewModel CreateProjectContainer(string name = "Project");

        ScenarioContainerViewModel CreateScenarioContainer(string name = "Scenario");

        //LogicalViewModel CreateLogical(string name, BaseState state);
        //LogicalViewModel CreateBaseState(string name, BaseState state);

        LogicalCollectionViewModel CreateLogicalCollection(string name);

        SliderTimePresenter CreateSliderTimePresenter(DateTime dateTime, TimeSpan timeSpan);

        IDataUpdater CreateDataUpdater();

        void SaveProjectContainer(ProjectContainerViewModel project, string path, IFileSystem fileIO, IJsonSerializer serializer);

        ProjectContainerViewModel OpenProjectContainer(string path, IFileSystem fileIO, IJsonSerializer serializer);

        ProjectContainerViewModel OpenProjectContainer(Stream stream, IFileSystem fileIO, IJsonSerializer serializer);

        void SaveProjectContainer(ProjectContainerViewModel project/*, IImageCache imageCache*/, Stream stream, IFileSystem fileIO, IJsonSerializer serializer);
    }
}

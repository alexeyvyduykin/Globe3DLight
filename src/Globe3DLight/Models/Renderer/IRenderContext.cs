using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Geometry;
using Globe3DLight.Scene;
using GlmSharp;
using Globe3DLight.Style;
using Globe3DLight.Containers;


namespace Globe3DLight.Renderer
{
    public interface IRenderContext
    {
        void DrawScenario(object dc, ScenarioContainer container);

        void DrawSun(object dc, SunRenderModel sun, dmat4 modelMatrix, ISceneState scene);
        
        void DrawEarth(object dc, EarthRenderModel earth, dmat4 modelMatrix, ISceneState scene);
        
        void DrawFrame(object dc, FrameRenderModel frame, dmat4 modelMatrix, ISceneState scene);

        void DrawFrameList(object dc, FrameRenderModel frame, IEnumerable<dmat4> modelMatrices, ISceneState scene);

        void DrawOrbit(object dc, OrbitRenderModel orbit, dmat4 modelMatrix, ISceneState scene);
        
        void DrawGroundStation(object dc, GroundStationRenderModel groundStation, dmat4 modelMatrix, ISceneState scene);
       
        void DrawGroundStationList(object dc, GroundStationRenderModel groundStation, IEnumerable<dmat4> modelMatrices, ISceneState scene);

        void DrawGroundObject(object dc, GroundObjectRenderModel groundobject, dmat4 modelMatrix, ISceneState scene);
      
        void DrawGroundObjectList(object dc, GroundObjectRenderModel groundobject, IEnumerable<dmat4> modelMatrices, ISceneState scene);

        void DrawRetranslator(object dc, RetranslatorRenderModel retranslator, dmat4 modelMatrix, ISceneState scene);
        
        void DrawSatellite(object dc, SatelliteRenderModel satellite, dmat4 modelMatrix, ISceneState scene);
        
        void DrawSensor(object dc, SensorRenderModel sensor, dmat4 modelMatrix, ISceneState scene);

        void DrawAntenna(object dc, AntennaRenderModel antenna, dmat4 modelMatrix, ISceneState scene);

        void DrawSpacebox(object dc, SpaceboxRenderModel spacebox, dmat4 modelMatrix, ISceneState scene);
    }
}

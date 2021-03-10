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
        void DrawScenario(object dc, IScenarioContainer container);

        void DrawSun(object dc, ISunRenderModel sun, dmat4 modelMatrix, ISceneState scene);
        
        void DrawEarth(object dc, IEarthRenderModel earth, dmat4 modelMatrix, ISceneState scene);
        
        void DrawFrame(object dc, IFrameRenderModel frame, dmat4 modelMatrix, ISceneState scene);
        
        void DrawOrbit(object dc, IOrbitRenderModel orbit, dmat4 modelMatrix, ISceneState scene);
        
        void DrawGroundStations(object dc, IGroundStationRenderModel groundStation, IEnumerable<dmat4> modelMatrices, ISceneState scene);
    
        void DrawGroundObjects(object dc, IGroundObjectRenderModel groundobject, IEnumerable<dmat4> modelMatrices, ISceneState scene);

        void DrawRetranslator(object dc, IRetranslatorRenderModel retranslator, dmat4 modelMatrix, ISceneState scene);
        
        void DrawSatellite(object dc, ISatelliteRenderModel satellite, dmat4 modelMatrix, ISceneState scene);
        
        void DrawSensor(object dc, ISensorRenderModel sensor, dmat4 modelMatrix, ISceneState scene);

        void DrawAntenna(object dc, IAntennaRenderModel antenna, dmat4 modelMatrix, ISceneState scene);

        void DrawSpacebox(object dc, ISpaceboxRenderModel spacebox, dmat4 modelMatrix, ISceneState scene);
    }
}

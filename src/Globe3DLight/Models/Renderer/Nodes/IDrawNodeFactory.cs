using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Scene;


namespace Globe3DLight.Renderer
{
    public interface IDrawNodeFactory
    { 
        ISunDrawNode CreateSunDrawNode(SunRenderModel sun);

        IEarthDrawNode CreateEarthDrawNode(EarthRenderModel earth);
    
        IFrameDrawNode CreateFrameDrawNode(FrameRenderModel frame);
        IOrbitDrawNode CreateOrbitDrawNode(OrbitRenderModel orbit);
        IGroundStationDrawNode CreateGroundStationDrawNode(GroundStationRenderModel groundStation);
        IGroundObjectDrawNode CreateGroundObjectListDrawNode(GroundObjectRenderModel groundStation);
        IRetranslatorDrawNode CreateRetranslatorDrawNode(RetranslatorRenderModel retranslator);
        ISatelliteDrawNode CreateSatelliteDrawNode(SatelliteRenderModel satellite, ICache<string, int> textureCache);
        ISensorDrawNode CreateSensorDrawNode(SensorRenderModel sensor);

        ISpaceboxDrawNode CreateSpaceboxDrawNode(SpaceboxRenderModel spacebox);


        IAntennaDrawNode CreateAntennaDrawNode(AntennaRenderModel antenna);
    }
}

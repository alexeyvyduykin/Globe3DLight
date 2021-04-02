using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.ViewModels.Scene;

namespace Globe3DLight.Models.Renderer
{
    public interface IDrawNodeFactory
    { 
        ISunDrawNode CreateSunDrawNode(SunRenderModel sun);

        IEarthDrawNode CreateEarthDrawNode(EarthRenderModel earth);
    
        IFrameDrawNode CreateFrameDrawNode(FrameRenderModel frame);
       
        IOrbitDrawNode CreateOrbitDrawNode(OrbitRenderModel orbit);
       
        IGroundStationDrawNode CreateGroundStationDrawNode(RenderModel groundStation);
       
        IGroundObjectDrawNode CreateGroundObjectListDrawNode(GroundObjectRenderModel groundStation);
        
        IRetranslatorDrawNode CreateRetranslatorDrawNode(RenderModel retranslator, ICache<string, int> textureCache);
        
        ISatelliteDrawNode CreateSatelliteDrawNode(RenderModel satellite, ICache<string, int> textureCache);
        
        ISensorDrawNode CreateSensorDrawNode(SensorRenderModel sensor);

        ISpaceboxDrawNode CreateSpaceboxDrawNode(SpaceboxRenderModel spacebox);

        IAntennaDrawNode CreateAntennaDrawNode(AntennaRenderModel antenna);
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Scene;


namespace Globe3DLight.Renderer
{
    public interface IDrawNodeFactory
    { 
        ISunDrawNode CreateSunDrawNode(ISunRenderModel sun);

        IEarthDrawNode CreateEarthDrawNode(IEarthRenderModel earth);
    
        IFrameDrawNode CreateFrameDrawNode(IFrameRenderModel frame);
        IOrbitDrawNode CreateOrbitDrawNode(IOrbitRenderModel orbit);
        IGroundStationDrawNode CreateGroundStationDrawNode(IGroundStationRenderModel groundStation);
        IGroundObjectDrawNode CreateGroundObjectListDrawNode(IGroundObjectRenderModel groundStation);
        IRetranslatorDrawNode CreateRetranslatorDrawNode(IRetranslatorRenderModel retranslator);
        ISatelliteDrawNode CreateSatelliteDrawNode(ISatelliteRenderModel satellite, ICache<string, int> textureCache);
        ISensorDrawNode CreateSensorDrawNode(ISensorRenderModel sensor);

        ISpaceboxDrawNode CreateSpaceboxDrawNode(ISpaceboxRenderModel spacebox);


        IAntennaDrawNode CreateAntennaDrawNode(IAntennaRenderModel antenna);
    }
}

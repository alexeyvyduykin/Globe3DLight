using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Scene;
using Globe3DLight.Renderer;

namespace Globe3DLight.Renderer.OpenTK
{
    internal class OpenTKDrawNodeFactory : IDrawNodeFactory
    {
        public ISunDrawNode CreateSunDrawNode(ISunRenderModel sun)
        {
            return new SunDrawNode(sun);
        }

        public IEarthDrawNode CreateEarthDrawNode(IEarthRenderModel earth)
        {
            return new EarthDrawNode(earth);
        }

        public IFrameDrawNode CreateFrameDrawNode(IFrameRenderModel frame)
        {
            return new FrameDrawNode(frame);
        }

        public IGroundStationDrawNode CreateGroundStationDrawNode(IGroundStationRenderModel groundStation)
        {
            return new GroundStationDrawNode(groundStation);
        }
        public IGroundObjectListDrawNode CreateGroundObjectListDrawNode(IGroundObjectListRenderModel groundObjectList)
        {
            return new GroundObjectListDrawNode(groundObjectList);
        }
        public IRetranslatorDrawNode CreateRetranslatorDrawNode(IRetranslatorRenderModel retranslator)
        {
            return new RetranslatorDrawNode(retranslator);
        }

        public ISatelliteDrawNode CreateSatelliteDrawNode(ISatelliteRenderModel satellite, ICache<string, int> textureCache)
        {
            return new SatelliteDrawNode(satellite, textureCache);
        }

        public ISensorDrawNode CreateSensorDrawNode(ISensorRenderModel sensor)
        {
            return new SensorDrawNode(sensor);
        }
        public IAntennaDrawNode CreateAntennaDrawNode(IAntennaRenderModel antenna)
        {
            return new AntennaDrawNode(antenna);
        }

        public IOrbitDrawNode CreateOrbitDrawNode(IOrbitRenderModel orbit)
        {
            return new OrbitDrawNode(orbit);
        }

        public ISpaceboxDrawNode CreateSpaceboxDrawNode(ISpaceboxRenderModel spacebox)
        {
            return new SpaceboxDrawNode(spacebox);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.ViewModels.Scene;
using Globe3DLight.Models.Renderer;

namespace Globe3DLight.Renderer.OpenTK
{
    internal class OpenTKDrawNodeFactory : IDrawNodeFactory
    {
        public ISunDrawNode CreateSunDrawNode(SunRenderModel sun)
        {
            return new SunDrawNode(sun);
        }

        public IEarthDrawNode CreateEarthDrawNode(EarthRenderModel earth)
        {
            return new EarthDrawNode(earth);
        }

        public IFrameDrawNode CreateFrameDrawNode(FrameRenderModel frame)
        {
            return new FrameDrawNode(frame);
        }

        public IGroundStationDrawNode CreateGroundStationDrawNode(RenderModel groundStation)
        {
            return new GroundStationDrawNode(groundStation);
        }

        public IGroundObjectDrawNode CreateGroundObjectListDrawNode(GroundObjectRenderModel groundObject)
        {
            return new GroundObjectDrawNode(groundObject);
        }

        public IRetranslatorDrawNode CreateRetranslatorDrawNode(RenderModel retranslator, ICache<string, int> textureCache)
        {
            return new RetranslatorDrawNode(retranslator, textureCache);
        }

        public ISatelliteDrawNode CreateSatelliteDrawNode(RenderModel satellite, ICache<string, int> textureCache)
        {
            return new SatelliteDrawNode(satellite, textureCache);
        }

        public ISensorDrawNode CreateSensorDrawNode(SensorRenderModel sensor)
        {
            return new SensorDrawNode(sensor);
        }
        public IAntennaDrawNode CreateAntennaDrawNode(AntennaRenderModel antenna)
        {
            return new AntennaDrawNode(antenna);
        }

        public IOrbitDrawNode CreateOrbitDrawNode(OrbitRenderModel orbit)
        {
            return new OrbitDrawNode(orbit);
        }

        public ISpaceboxDrawNode CreateSpaceboxDrawNode(SpaceboxRenderModel spacebox)
        {
            return new SpaceboxDrawNode(spacebox);
        }
    }
}

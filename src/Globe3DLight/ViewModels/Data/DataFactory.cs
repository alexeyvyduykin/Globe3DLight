using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Data.Animators;
using System.Collections.Immutable;
using Globe3DLight.Data.Database;
using GlmSharp;


namespace Globe3DLight.Data
{
    public interface IDataFactory
    {
        IFrameData CreateFrameData(/*string name*/);

        ISunData CreateSunAnimator(ISunDatabase sunDatabase);

        IJ2000Data CreateJ2000Animator(IJ2000Database j2000Database);

        IOrbitData CreateOrbitAnimator(IOrbitDatabase orbitDatabase);

        IRotationData CreateRotationAnimator(IRotationDatabase rotationDatabase);

        ISensorData CreateSensorAnimator(ISensorDatabase sensorDatabase);

        IAntennaData CreateAntennaAnimator(IAntennaDatabase antennaDatabase);

        IGroundStationData CreateGroundStationData(IGroundStationDatabase groundStationDatabase);

        IRetranslatorData CreateRetranslatorAnimator(IRetranslatorDatabase retranslatorDatabase);

    }


    public class DataFactory : IDataFactory
    {
        public IFrameData CreateFrameData(/*string name*/)
        {
            return new FrameData();// { Name = name };
        }

        public ISunData CreateSunAnimator(ISunDatabase sunDatabase)
        {
            return new SunAnimator(sunDatabase);// { Name = "fr_sun" };
        }

        public IJ2000Data CreateJ2000Animator(IJ2000Database j2000Database)
        {
            return new J2000Animator(j2000Database);// { Name = "fr_j2000" };
        }

        public IOrbitData CreateOrbitAnimator(IOrbitDatabase orbitDatabase)
        {
            return new OrbitAnimator(orbitDatabase);// { Name = "fr_orbital" };
        }

        public IRotationData CreateRotationAnimator(IRotationDatabase rotationDatabase)
        {
            return new RotationAnimator(rotationDatabase);// { Name = "fr_rotation" };
        }

        public ISensorData CreateSensorAnimator(ISensorDatabase sensorDatabase)
        {
            return new SensorAnimator(sensorDatabase);// { Name = "fr_sensor" };
        }

        public IAntennaData CreateAntennaAnimator(IAntennaDatabase antennaDatabase)
        {
            return new AntennaAnimator(antennaDatabase);
        }

        public IGroundStationData CreateGroundStationData(IGroundStationDatabase groundStationDatabase)
        {
            return new GroundStationData(groundStationDatabase);// { Name ="fr_groundstation" };
        }

        public IRetranslatorData CreateRetranslatorAnimator(IRetranslatorDatabase retranslatorDatabase)
        {
            return new RetranslatorAnimator(retranslatorDatabase);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Immutable;
using GlmSharp;


namespace Globe3DLight.Data
{
    public interface IDataFactory
    {
        IFrameState CreateFrameState();

        ISunState CreateSunAnimator(SunData data);

        IJ2000State CreateJ2000Animator(J2000Data data);

        IOrbitState CreateOrbitAnimator(OrbitData data);

        IRotationState CreateRotationAnimator(RotationData data);

        ISensorState CreateSensorAnimator(SensorData data);

        IAntennaState CreateAntennaAnimator(AntennaData data);

        IGroundStationState CreateGroundStationData(GroundStationData data);

        IGroundObjectListState CreateGroundObjectListData(GroundObjectListData data);

        IRetranslatorState CreateRetranslatorAnimator(RetranslatorData data);
    }


    public class DataFactory : IDataFactory
    {
        public IFrameState CreateFrameState()
        {
            return new FrameState();
        }

        public ISunState CreateSunAnimator(SunData data)
        {
            return new SunAnimator(data);
        }

        public IJ2000State CreateJ2000Animator(J2000Data data)
        {
            return new J2000Animator(data);
        }

        public IOrbitState CreateOrbitAnimator(OrbitData data)
        {
            return new OrbitAnimator(data);
        }

        public IRotationState CreateRotationAnimator(RotationData data)
        {
            return new RotationAnimator(data);
        }

        public ISensorState CreateSensorAnimator(SensorData data)
        {
            return new SensorAnimator(data);
        }

        public IAntennaState CreateAntennaAnimator(AntennaData data)
        {
            return new AntennaAnimator(data);
        }

        public IGroundStationState CreateGroundStationData(GroundStationData data)
        {
            return new GroundStationState(data);
        }
        public IGroundObjectListState CreateGroundObjectListData(GroundObjectListData data)
        {
            return new GroundObjectListState(data);
        }
        public IRetranslatorState CreateRetranslatorAnimator(RetranslatorData data)
        {
            return new RetranslatorAnimator(data);
        }
    }
}

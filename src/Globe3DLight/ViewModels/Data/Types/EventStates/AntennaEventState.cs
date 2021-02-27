using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Data
{
    //public enum TranslationType
    //{
    //    GroundStation = 0,
    //    Retranslator = 1
    //}


    public class AntennaEventState : EventState
    {
        internal override EventState FromHit(EventState state0, EventState state1, double t)
        {
            return new AntennaEventState()
            {
                Target = this.Target,
       //         IndexTarget = this.IndexTarget,
       //         Type = this.Type,
            };
        }

        public string Target { get; internal set; }

     //   public TranslationType Type { get; internal set; }  // 0 - PPI, 1 - Retranslator

     //   public int IndexTarget { get; internal set; }
    }
}

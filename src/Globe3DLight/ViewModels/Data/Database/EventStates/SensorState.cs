using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Data.Animators;

namespace Globe3DLight.Data.Database
{
    public class SensorState : EventState
    {
        internal double Range1 { get; set; }
        internal double Range2 { get; set; }
        internal double Gam1RAD { get; set; }
        internal double Gam2RAD { get; set; }

        internal int Direction { get; set; }

        public IShoot Shoot { get; private set; }

        private bool first = true;

        internal override EventState FromHit(EventState state0, EventState state1, double t)
        {
            if (first == true)
            {
                {
                    //var gam = GlmSharp.glm.Radians(40.0);
                    //int pls11 = Gam1RAD >= 0.0 ? 1 : -1;
                    //int pls22 = Gam2RAD >= 0.0 ? 1 : -1;
                    //int pls = (pls11 == 1 & pls22 == 1) ? 1 : -1;


                    //if (Gam1RAD >= 0.0)
                    //{
                    //    Gam1RAD = Gam1RAD - gam;
                    //}
                    //else
                    //{
                    //    Gam1RAD = Gam1RAD + gam;
                    //}

                    //if (Gam2RAD >= 0.0)
                    //{
                    //    Gam2RAD = Gam2RAD - gam;
                    //}
                    //else
                    //{
                    //    Gam2RAD = Gam2RAD + gam;
                    //}


                    //int pls1 = Gam1RAD >= GlmSharp.glm.Radians(40.0) ? 1 : -1;
                    //int pls2 = Gam2RAD >= GlmSharp.glm.Radians(40.0) ? 1 : -1;

                    //double yScale1;
                    //double yScale2;
                    //if (pls == 1)
                    //{
                    //    yScale1 = -Range1;
                    //    yScale2 = -Range2;
                    //}
                    //else
                    //{
                    //    yScale1 = -Range2;
                    //    yScale2 = -Range1;
                    //}

                    //double angRot1 = -Math.Abs(Gam1RAD) * (pls1);
                    //double angRot2 = -Math.Abs(Gam2RAD) * (pls2);
                    //double dx = 0.02;

                    //Shoot = (                
                    //    new dvec3(-dx * pls1, Math.Cos(angRot1) * yScale1, Math.Sin(angRot1) * yScale1),    // * pls - для обхода TRIANGLE_STRIP по часов стр.               
                    //    new dvec3(-dx * pls2, Math.Cos(angRot2) * yScale2, Math.Sin(angRot2) * yScale2),                
                    //    new dvec3(dx * pls2, Math.Cos(angRot2) * yScale2, Math.Sin(angRot2) * yScale2),               
                    //    new dvec3(dx * pls1, Math.Cos(angRot1) * yScale1, Math.Sin(angRot1) * yScale1)
                    //);
                }

                Direction = (Gam1RAD >= 0.0 && Gam2RAD >= 0.0) ? 1 : -1;
                
                double yScale1 = -Range1;
                double yScale2 = -Range2;
                double angRot1 = -Math.Abs(Gam1RAD) * Direction;
                double angRot2 = -Math.Abs(Gam2RAD) * Direction;
                double dx = 0.02;


                Shoot = new Shoot() 
                {
                    p0 = new dvec3(-dx * Direction, Math.Cos(angRot1) * yScale1, Math.Sin(angRot1) * yScale1),    // * pls - для обхода TRIANGLE_STRIP по часов стр.                
                    p1 = new dvec3(-dx * Direction, Math.Cos(angRot2) * yScale2, Math.Sin(angRot2) * yScale2),
                    p2 = new dvec3(dx * Direction, Math.Cos(angRot2) * yScale2, Math.Sin(angRot2) * yScale2),
                    p3 = new dvec3(dx * Direction, Math.Cos(angRot1) * yScale1, Math.Sin(angRot1) * yScale1),        
                };
                
                first = false;
            }

            return new SensorState()
            {
                Shoot = Shoot,
                Direction = Direction,

            };
        }
    }

}

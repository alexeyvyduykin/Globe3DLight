using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Globe3DLight.Data
{
    public enum MissMode
    {
        NotActive,
        LastActive,
    }

    public class ContinuousEvents<T> : Events<T> where T : EventState
    {
        public ContinuousEvents()
        {
            MissMode = MissMode.NotActive;

            Count = 0;
        }

        private int ActiveOrLastIndex { get; set; }

        private int Count { get; set; }

        public MissMode MissMode { get; set; }

        private void Miss(int index)
        {
            switch (MissMode)
            {
                case MissMode.NotActive:

                    ActiveState = null;

                    break;
                case MissMode.LastActive:

                    int last = index - 1;

                    if (last >= 0)
                    {
                        ActiveState = Behaviours[last].StateEnd;
                    }
                    else // last < 0
                    {
                        ActiveState = Behaviours[0].StateBegin;
                    }


                    Debug.WriteLine(string.Format("Events miss, LastActiveState: t = {0}", ActiveState.t));

                    break;
                default:
                    break;
            }
        }

        private void Hit(int index, double t)
        {
            ActiveOrLastIndex = index;

            ActiveState = Behaviours[index].InRange(t);          
        }

        private T From { get; set; }

        public override void AddFrom(T state)
        {
            this.From = state;
        }

        public override void AddTo(T state)
        {
            if (Count == 0)
            {
                Behaviours = new List<EventInterval<T>>();

                ActiveState = null;
            
                ActiveOrLastIndex = 0;
            }

            Behaviours.Add(new EventInterval<T>(From, state));

            From = state;

            Count = Behaviours.Count;
        }

        public override void Update(double t)
        {
            if (Count != 0)
            {
                if (Behaviours[ActiveOrLastIndex].IsRange(t) == true)
                {
                    Hit(ActiveOrLastIndex, t);
                    return;
                }
                else
                {
                    if (Behaviours[ActiveOrLastIndex].IsForward(t) == true) // forward
                    {
                        bool isActive;
                        int index = ActiveOrLastIndex;
                        do
                        {
                            index = index + 1;

                            if (index >= Count)
                            {
                                Miss(index);
                                return;
                            }

                            isActive = Behaviours[index].IsRange(t);

                            if (isActive == false)
                            {
                                if (Behaviours[index].IsForward(t) == false)
                                {
                                    Miss(index);
                                    return;
                                }
                            }

                        } while (isActive == false);

                        Hit(index, t);
                    }
                    else // backward
                    {
                        bool isActive;
                        int index = ActiveOrLastIndex;
                        do
                        {
                            index = index - 1;

                            if (index < 0)
                            {
                                Miss(index + 1);
                                return;
                            }

                            isActive = Behaviours[index].IsRange(t);

                            if (isActive == false)
                            {
                                if (Behaviours[index].IsBackward(t) == false)
                                {
                                    Miss(index + 1);
                                    return;
                                }
                            }

                        } while (isActive != true);

                        Hit(index, t);
                    }
                }
            }
        }
    }


}

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Globe3DLight.Data
{
    public class EventList<T> : IEventList<T> where T : IEventState
    {
        private int _activeOrLastIndex; 
        private readonly IList<EventInterval<T>> _behaviours; 
        private readonly EventMissMode _missMode;
        private T _activeState;

        public EventList(EventMissMode eventMissMode = EventMissMode.NotActive)
        {
            _missMode = eventMissMode;

            _behaviours = new List<EventInterval<T>>();

            _activeState = default;

            _activeOrLastIndex = 0;
        }

        public T ActiveState => _activeState;

        public bool HasActiveState => _activeState != null;

        public EventMissMode MissMode => _missMode;

        private void Miss(int index)
        {
            switch (MissMode)
            {
                case EventMissMode.NotActive:
                    _activeState = default; //null;                    
                    break;
                case EventMissMode.LastActive:                        
                    _activeState = ((index - 1) >= 0) ? _behaviours[index - 1].EndState : _behaviours[0].BeginState;
                    break;
                default:
                    break;
            }
        }

        private void Hit(int index, double t)
        {
            _activeOrLastIndex = index;

            _activeState = _behaviours[index].InRange(t);          
        }

        public void Add(T from, T to)
        {  
            _behaviours.Add(new EventInterval<T>(from, to));
        }

        public void Update(double t)
        {
            if (_behaviours.Count != 0)
            {
                if (_behaviours[_activeOrLastIndex].IsRange(t) == true)
                {
                    Hit(_activeOrLastIndex, t);
                    return;
                }
                else
                {
                    if (_behaviours[_activeOrLastIndex].IsForward(t) == true) // forward
                    {
                        bool isActive;
                        int index = _activeOrLastIndex;
                        do
                        {
                            index++;

                            if (index >= _behaviours.Count)
                            {
                                Miss(index);
                                return;
                            }

                            isActive = _behaviours[index].IsRange(t);

                            if (isActive == false)
                            {
                                if (_behaviours[index].IsForward(t) == false)
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
                        int index = _activeOrLastIndex;
                        do
                        {
                            index--;

                            if (index < 0)
                            {
                                Miss(index + 1);
                                return;
                            }

                            isActive = _behaviours[index].IsRange(t);

                            if (isActive == false)
                            {
                                if (_behaviours[index].IsBackward(t) == false)
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

        public void Clear() => _behaviours.Clear();
    }


}

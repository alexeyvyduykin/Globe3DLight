using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;

namespace Globe3DLight.Data
{
    public class EventList<T> : IEventList<T> where T : IEventInterval
    {
        private int _activeOrLastIndex;
        private readonly IList<T> _list;
        private readonly EventMissMode _missMode;

        public EventList(EventMissMode eventMissMode = EventMissMode.NotActive)
        {
            _missMode = eventMissMode;

            _list = new List<T>();

            _activeOrLastIndex = 0;
        }

        public EventMissMode MissMode => _missMode;

        public void Add(T interval) => _list.Add(interval);

        public void Clear() => _list.Clear();

        public T ActiveInterval(double t)
        {
            if (_list.Count != 0)
            {
                if (_list[_activeOrLastIndex].IsRange(t) == true)
                {
                    return _list[_activeOrLastIndex];
                }
                else
                {
                    if (_list[_activeOrLastIndex].IsForward(t) == true) // forward
                    {
                        for (int i = _activeOrLastIndex + 1; i < _list.Count; i++)
                        {
                            var isActive = _list[i].IsRange(t);

                            if (isActive == false)
                            {
                                if (_list[i].IsForward(t) == false)
                                {
                                    return Miss(i);
                                }
                            }
                            else
                            {
                                _activeOrLastIndex = i;
                                return _list[i];
                            }

                        }

                        return Miss(_list.Count);
                    }
                    else // backward
                    {

                        for (int i = _activeOrLastIndex - 1; i >= 0; i--)
                        {
                            var isActive = _list[i].IsRange(t);

                            if (isActive == false)
                            {
                                if (_list[i].IsBackward(t) == false)
                                {
                                    return Miss(i + 1);
                                }
                            }
                            else
                            {
                                _activeOrLastIndex = i;
                                return _list[i];
                            }

                        }

                        return Miss(0);
                    }
                }
            }

            return default;
        }

        private T Miss(int index)
        {
            return MissMode switch
            {
                EventMissMode.NotActive => default,
                EventMissMode.LastActive => ((index - 1) >= 0) ? _list[index - 1] : _list[0],
                _ => default,
            };
        }
    }
}

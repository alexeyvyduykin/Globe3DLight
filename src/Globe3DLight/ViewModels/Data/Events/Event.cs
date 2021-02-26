using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Data
{
    public abstract class Events<T> where T : EventState
    {
        public abstract void AddFrom(T state);
        public abstract void AddTo(T state);

        public virtual void Clear()
        {
            if (Behaviours != null)
            {
                Behaviours.Clear();
            }
        }

        public abstract void Update(double t);

        public T ActiveState { get; protected set; }

        public bool IsActiveState
        {
            get
            {
                return ActiveState != null;
            }
        }

        //protected EventInterval<T> ActiveBehaviour { get; set; }

        internal IList<EventInterval<T>> Behaviours { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using System.Linq;

namespace Globe3DLight.Data
{
    public interface IAntennaState : IState, IAnimator
    {  
        bool Enable { get; }

        string Target { get; }      
    }


    public class AntennaAnimator : ObservableObject, IAntennaState
    {      
        private readonly IEventList<AntennaInterval> _translationEvents;
        private bool _enable;
        private string _target;

        public AntennaAnimator(AntennaData data)
        {
            _translationEvents = data.Translations.Select(s => new AntennaInterval(s.BeginTime, s.EndTime, s.Target)).ToEventList();
        }

        public bool Enable
        {
            get => _enable;
            protected set => Update(ref _enable, value);
        }

        public string Target
        {
            get => _target;
            protected set => Update(ref _target, value);
        }

        public void Animate(double t)
        {
            var activeInterval = _translationEvents.ActiveInterval(t);

            Enable = activeInterval != default;

            if (Enable == true)
            {       
                Target = activeInterval.Animate(t).Target;
            }
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}

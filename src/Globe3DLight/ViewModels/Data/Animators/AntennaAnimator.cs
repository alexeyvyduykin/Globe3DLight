using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using System.Linq;
using Globe3DLight.Models.Data;

namespace Globe3DLight.ViewModels.Data
{
    public class AntennaAnimator : BaseState, IAnimator
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
            protected set => RaiseAndSetIfChanged(ref _enable, value);
        }

        public string Target
        {
            get => _target;
            protected set => RaiseAndSetIfChanged(ref _target, value);
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
    }
}

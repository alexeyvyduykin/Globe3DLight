using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;

namespace Globe3DLight.Data
{
    public interface IAntennaState : IState, IAnimator
    {  
        bool Enable { get; }

        string Target { get; }      
    }


    public class AntennaAnimator : ObservableObject, IAntennaState
    {      
        private readonly ContinuousEvents<AntennaEventState> _translationEvents;
        private bool _enable;
        private string _target;

        public AntennaAnimator(AntennaData data)
        {
            _translationEvents = create(data.Translations);
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

        private ContinuousEvents<AntennaEventState> create(IList<TranslationRecord> translations)
        {
            var translationEvents = new ContinuousEvents<AntennaEventState>();

            foreach (var item in translations)
            {
                translationEvents.AddFrom(new AntennaEventState()
                {
                    t = item.BeginTime,
                    Target = item.Target,                    
                });

                translationEvents.AddTo(new AntennaEventState()
                {
                    t = item.EndTime,
                    Target = item.Target,
                });
            }

            return translationEvents;
        }

        public void Animate(double t)
        {
            _translationEvents.Update(t);

            this.Enable = _translationEvents.IsActiveState;

            if (Enable == true)
            {
                var activeState = _translationEvents.ActiveState;

                this.Target = activeState.Target;
            }
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}

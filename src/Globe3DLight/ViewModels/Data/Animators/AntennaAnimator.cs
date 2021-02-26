using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Data.Database;

namespace Globe3DLight.Data.Animators
{
    public interface IAntennaData : IData, IAnimator
    {
        bool Enable { get; }

        string Target { get; }

      
    }


    public class AntennaAnimator : ObservableObject, IAntennaData
    {
        private readonly IAntennaDatabase _antennaDatabase;
        private readonly ContinuousEvents<AntennaState> _translationEvents;
        private bool _enable;
        private string _target;

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

        public AntennaAnimator(IAntennaDatabase antennaDatabase)
        {
            this._antennaDatabase = antennaDatabase;
            this._translationEvents = create(antennaDatabase.Translations);      
        }

        private ContinuousEvents<AntennaState> create(IList<TranslationRecord> translations)
        {
            var translationEvents = new ContinuousEvents<AntennaState>();

            foreach (var item in translations)
            {
                translationEvents.AddFrom(new AntennaState()
                {
                    t = item.BeginTime,
                    Target = item.Target,                    
                });

                translationEvents.AddTo(new AntennaState()
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

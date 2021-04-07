#nullable enable
using System.Collections.Immutable;
using GlmSharp;
using Globe3DLight.Models.Data;
using Globe3DLight.Models.Entities;
using Globe3DLight.ViewModels.Entities;

namespace Globe3DLight.ViewModels.Data
{
    public class AntennaAnimator : BaseState, IAnimator, IAssetable<BaseEntity>
    {
        private readonly AntennaData _data;
        private readonly IEventList<AntennaInterval> _translationEvents;
        private ImmutableArray<BaseEntity> _assets;
        private bool _enable;
        private string _target;
        private bool _first = true;
        private dvec3 _targetPosition;
        private dvec3 _attachPosition;

        public AntennaAnimator(AntennaData data)
        {
            _data = data;
            _translationEvents = new EventList<AntennaInterval>();
            _target = string.Empty;
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

        public dvec3 TargetPosition
        {
            get => _targetPosition;
            protected set => RaiseAndSetIfChanged(ref _targetPosition, value);
        }

        public ImmutableArray<BaseEntity> Assets
        {
            get => _assets;
            set => RaiseAndSetIfChanged(ref _assets, value);
        }

        public dvec3 AttachPosition
        {
            get => _attachPosition;
            set => RaiseAndSetIfChanged(ref _attachPosition, value);
        }
        
        private void Init()
        {          
            foreach (var rec in _data.Translations)
            {
                var begin = rec.BeginTime;
                var end = rec.EndTime;
                var target = rec.Target;

                foreach (var item in Assets)
                {
                    if (item is GroundStation gs)
                    {
                        if (gs.Name.Equals(target) == true)
                        {
                            _translationEvents.Add(new AntennaInterval(begin, end, target, gs.Frame.State));
                            break;
                        }
                    }
                    else if (item is Retranslator retranslator)
                    {
                        if (retranslator.Name.Equals(target) == true)
                        {
                            _translationEvents.Add(new AntennaInterval(begin, end, target, retranslator.Frame.State));
                        }
                    }
                }
            }


            ModelMatrix = dmat4.Translate(AttachPosition);

            _first = false;
        }

        public void Animate(double t)
        {
            if (_first == true)
            {
                Init();
            }

            var activeInterval = _translationEvents.ActiveInterval(t);

            Enable = activeInterval != default;

            if (activeInterval != default)
            {
                var value = activeInterval.Animate(t);

                Target = value.Target;
                TargetPosition = value.Position;
            }
        }
    }
}

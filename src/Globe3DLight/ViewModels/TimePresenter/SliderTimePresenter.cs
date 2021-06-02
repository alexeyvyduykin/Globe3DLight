using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Globe3DLight.Timer;

namespace Globe3DLight.ViewModels.Time
{
    public class SliderTimePresenter : TimePresenter
    {
        private int _sliderMin;
        private int _sliderMax;
        private int _sliderValue;

        public SliderTimePresenter(ITimer timer, DateTime begin, TimeSpan duration, int min, int max) : base(timer, begin, duration)
        {
            _sliderMin = min;
            _sliderMax = max;
        }

        public int SliderMin 
        {
            get => _sliderMin;
            protected set => RaiseAndSetIfChanged(ref _sliderMin, value); 
        }

        public int SliderMax 
        {
            get => _sliderMax;
            protected set => RaiseAndSetIfChanged(ref _sliderMax, value);
        }

        public int SliderValue
        {
            get => _sliderValue;
            set 
            {
                FromSlider(value);
                RaiseAndSetIfChanged(ref _sliderValue, value); 
            }
        }
        
        protected override void TimerThreadElapsed(object sender, ElapsedEventArgs e)
        {
            base.TimerThreadElapsed(sender, e);

            var sliderValue = (int)(CurrentTime * _sliderMax / Duration.TotalSeconds);

            RaiseAndSetIfChanged(ref _sliderValue, sliderValue, nameof(SliderTimePresenter.SliderValue));
        }

        private void FromSlider(int value)
        {
            var t = value * Duration.TotalSeconds / _sliderMax;

            Update(t);
        }
    }
}

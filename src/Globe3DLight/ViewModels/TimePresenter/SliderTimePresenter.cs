using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Globe3DLight.Time
{
    public class SliderTimePresenter : TimePresenter, ISliderTimePresenter
    {
        private int _sliderMin;
        private int _sliderMax;
        private int _sliderValue;

        public SliderTimePresenter(DateTime begin, TimeSpan duration, int min, int max) : base(begin, duration)
        {
            _sliderMin = min;
            _sliderMax = max;
        }

        public int SliderMin 
        {
            get => _sliderMin;
            protected set => Update(ref _sliderMin, value); 
        }

        public int SliderMax 
        {
            get => _sliderMax;
            protected set => Update(ref _sliderMax, value);
        }

        public int SliderValue
        {
            get => _sliderValue;
            set 
            {
                FromSlider(value);
                Update(ref _sliderValue, value); 
            }
        }
        
        protected override void TimerThreadElapsed(object sender, ElapsedEventArgs e)
        {
            base.TimerThreadElapsed(sender, e);

            var sliderValue = (int)(CurrentTime * _sliderMax / Duration.TotalSeconds);

            Update(ref _sliderValue, sliderValue, nameof(SliderValue));
        }

        private void FromSlider(int value)
        {
            var t = value * Duration.TotalSeconds / _sliderMax;

            Update(t);
        }
    }
}

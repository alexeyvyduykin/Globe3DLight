using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Time
{
    public interface ISliderTimePresenter : ITimePresenter
    {
        int SliderMin { get; }

        int SliderMax { get; }

        int SliderValue { get; set; }
    }
}

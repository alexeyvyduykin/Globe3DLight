using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeDataViewer.ViewModels;
using Avalonia.Controls;

namespace TimeDataViewer.Models
{
    public interface ISeriesControl
    {
        SchedulerControl? Scheduler { get; }
       
        Control Tooltip { get; set; }

        IShape CreateIntervalShape(IInterval interval);

        IShape CreateSeriesShape(); 
          
        IntervalTooltipViewModel CreateTooltip(IInterval marker);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeDataViewer.Core
{

    public interface ICoreFactory
    {
        ITimeAxis CreateTimeAxis();

        ICategoryAxis CreateCategoryAxis(); 

        Area CreateArea();
    }

    public class CoreFactory : ICoreFactory
    {
        public ITimeAxis CreateTimeAxis()
        {
            return new TimeAxis()
            {
                Header = "X",
                Type = AxisType.X,
                HasInversion = false,
                IsDynamicLabelEnable = true,
                TimePeriodMode = TimePeriod.Month,
                LabelFormatPool = new Dictionary<TimePeriod, string>()
                {
                    { TimePeriod.Hour, @"{0:HH:mm}" },
                    { TimePeriod.Day, @"{0:HH:mm}" },
                    { TimePeriod.Week, @"{0:dd/MMM}" },
                    { TimePeriod.Month, @"{0:dd}" },
                    { TimePeriod.Year, @"{0:dd/MMM}" },
                },
                LabelDeltaPool = new Dictionary<TimePeriod, double>()
                {
                    { TimePeriod.Hour, 60.0 * 5 },
                    { TimePeriod.Day, 3600.0 * 2 },
                    { TimePeriod.Week, 86400.0 },
                    { TimePeriod.Month, 86400.0 },
                    { TimePeriod.Year, 86400.0 * 12 },
                }
            };
        }

        public ICategoryAxis CreateCategoryAxis()
        {
            return new CategoryAxis()
            {
                Header = "Y",
                Type = AxisType.Y,
                HasInversion = false,
                IsDynamicLabelEnable = true,
            };
        }

        public Area CreateArea()
        {
            return new Area()
            {
                MinZoom = 0,
                MaxZoom = 100,
                ZoomScaleX = 1.0, // 30 %        
                ZoomScaleY = 0.0,
                CanDragMap = true,
                MouseWheelZoomEnabled = true,       
            };
        }
    }
}

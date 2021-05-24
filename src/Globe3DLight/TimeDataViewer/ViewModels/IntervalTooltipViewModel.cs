using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeDataViewer.Models;

namespace TimeDataViewer.ViewModels
{
    public class IntervalTooltipViewModel : ViewModelBase
    {
        private readonly IInterval _marker;
        private string _category;
        private string _date;
        private string _begin;
        private string _end;

        public IntervalTooltipViewModel(IInterval marker)
        {
            _marker = marker;

            var left = marker.Left;
            var right = marker.Right;

            var series = marker.Series;
            // var epoch = DateTime.Now.Date;

            var epoch = marker.SeriesControl.Scheduler.Epoch;

            _category = series.Name;
            _date = epoch.AddSeconds(left).ToShortDateString();
            _begin = epoch.AddSeconds(left).ToLongTimeString();
            _end = epoch.AddSeconds(right).ToLongTimeString();
        }

        public string Category
        {
            get => _category;
            set => RaiseAndSetIfChanged(ref _category, value);
        }

        public string Date
        {
            get => _date;
            set => RaiseAndSetIfChanged(ref _date, value);
        }

        public string Begin
        {
            get => _begin;
            set => RaiseAndSetIfChanged(ref _begin, value);
        }

        public string End
        {
            get => _end;
            set => RaiseAndSetIfChanged(ref _end, value);
        }
    }
}

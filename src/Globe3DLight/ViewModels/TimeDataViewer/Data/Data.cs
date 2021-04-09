using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Globe3DLight.ViewModels.TimeDataViewer.Data
{
    public class BaseModelView : INotifyPropertyChanged
    {
        DataPool data;

        RealDataPool realData;

        public enum EDataType { Old, New };

        EDataType DataType;

        public BaseModelView(EDataType type, RealDataPool.ETimePeriod timePeriod = RealDataPool.ETimePeriod.Day)
        {
            this.DataType = type;

            switch (type)
            {
                case EDataType.Old:
                    {
                        data = new DataPool();

                        DataPool.MaxLength = 5000;

                        StringCount = 3;

                        CreateBacks();
                    }
                    break;
                case EDataType.New:
                    {
                        realData = new RealDataPool() { NumSatellites = 4, TimePeriod = timePeriod };

                        StringCount = 4;
                    }
                    break;
                default:
                    break;
            }
        }
        ObservableCollection<IntervalGroup> _strings = new ObservableCollection<IntervalGroup>();
        public ObservableCollection<IntervalGroup> Strings
        {
            get
            {
                return _strings;
            }
            set
            {
                _strings = value;
                OnPropertyChanged("Strings");
            }
        }

        ObservableCollection<IntervalCustom> Intervals
        {
            get
            {
                return new ObservableCollection<IntervalCustom>(_strings.SelectMany(s => s.Intervals));
            }
        }

        ObservableCollection<IntervalCustom> _backgroundIntervals = new ObservableCollection<IntervalCustom>();
        public ObservableCollection<IntervalCustom> BackgroundIntervals
        {
            get
            {
                return _backgroundIntervals;
            }
        }

        public DateTime Origin = new DateTime(2000, 1, 1, 0, 0, 0);

        public double MinValue
        {
            get
            {
                double min = double.MaxValue;
                foreach (var item in Strings)
                {
                    min = Math.Min(item.Intervals.Min(s => s.Left), min);
                }

                return min;
            }
        }

        public double MaxValue
        {
            get
            {
                double max = double.MinValue;
                foreach (var item in Strings)
                {
                    max = Math.Max(item.Intervals.Max(s => s.Left), max);
                }

                return max;
            }
        }

        void CreateBacks()
        {

            if (DataType == EDataType.Old)
            {
                data.GenerateBacks();

                foreach (var item in data.Backs)
                {
                    _backgroundIntervals.Add(new IntervalCustom(item.Left, item.Right)
                    {
                        Name = string.Format("BackgroundInterval_{0}_{1}", item.Left, item.Right)
                    });
                }
            }
            else if (DataType == EDataType.New)
            {
                foreach (var item in realData.BackIntervals)
                {
                    _backgroundIntervals.Add(new IntervalCustom(item.Left, item.Right)
                    {
                        Name = string.Format("BackgroundInterval_{0}_{1}", item.Left, item.Right)
                    });
                }
            }

        }

        private int _stringCount;
        public int StringCount
        {
            get
            {
                return _stringCount;
            }
            set
            {
                _stringCount = value;


                Strings.Clear();

                if (DataType == EDataType.Old)
                {

                    data.Generate(_stringCount);

                    //  double h = scheduler.SchedulerRect.SizeY;

                    //   double step = h / (_stringCount + 1);
                    int i = 1;
                    foreach (var key in data.Pool.Keys)
                    {
                        var group = new IntervalGroup("String_" + key.ToString());
                        foreach (var item in data.Pool[key].Select(s => new IntervalCustom(s.Left, s.Right)))
                        {
                            //   str.Map = scheduler;
                            //  str.Position = new SCTimeScheduler.NET.SCSchedulerPoint(0.0, (i * step));
                            //var arr = data.Pool[key].Select(s => new Interval(s.Left, s.Right) /*{ String = str }*/);
                            group.AddInterval(item);

                        }
                        Strings.Add(group);

                        i++;
                    }
                }
                else if (DataType == EDataType.New)
                {
                    realData.NumSatellites = _stringCount;

                    realData.GenerateIntervals();

                    int i = 1;
                    foreach (var key in realData.Intervals.Keys)
                    {
                        var group = new IntervalGroup("String_" + key.ToString());
                        foreach (var item in realData.Intervals[key].Select(s => new IntervalCustom(s.Left, s.Right)))
                        {
                            group.AddInterval(item);
                        }
                        Strings.Add(group);

                        i++;
                    }

                    CreateBacks();
                }

                OnPropertyChanged("StringCount");
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
    public class IntervalCustom
    {
        public IntervalCustom(double left, double right)
        {
            this.Left = left;
            this.Right = right;

            this.Name = string.Format("Interval_{0:HH:mm:ss}_{1:HH:mm:ss}", left, right);
            this.Id = Guid.NewGuid().ToString();
        }

        public IntervalGroup Group { get; set; }

        public double Left { get; private set; }
        public double Right { get; private set; }

        public string Name { get; set; }
        public string Id { get; }
    }

    public class IntervalGroup : IDisposable
    {
        static Random r = new Random();

        public IntervalGroup(string name)
        {
            this.Name = name;

            Intervals = new ObservableCollection<IntervalCustom>();

            string[] descr = new string[]
            {
                "Satellites times vision",
                "Sunlight satellite subpoint",
                "Satellites angle rotation",
                "Satellite received",
                "Sensor daylight",
                "GroundStation work",
                "Satellite orbit correction"
            };

            var index = r.Next(0, descr.Length - 1);

            Description = descr[index];
        }

        public void AddInterval(IntervalCustom ival)
        {
            ival.Group = this;

            Intervals.Add(ival);
        }

        public void Dispose()
        {
            Intervals.Clear();
        }

        public ObservableCollection<IntervalCustom> Intervals { get; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Globe3DLight.ViewModels.Editors
{
    public abstract class BaseTask
    {
        public string? Name { get; set; }
        public string? Begin { get; set; }
        public TimeSpan Duration { get; set; }
    }

    public class Task1 : BaseTask { }

    public class Task2 : BaseTask { }

    public class Task3 : BaseTask { }

    public class Filter : ViewModelBase
    {
public bool IsRotate { get; set; }
        public bool IsTransmit { get; set; }
        public bool IsShoot { get; set; }
    }

    public class TaskListEditorViewModel : ViewModelBase
    {
        ObservableCollection<BaseTask> _tasks;

        int[] randoms = new int[] { 1, 0, 1, 0, 1, 2, 0, 2, 2, 2, 1, 2, 2, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 2, 2, 2, 1, 1, 0, 2, 0, 1, 1, 1, 2, 1, 0, 2, 1, 0, 2, 1, 1, 0, 0, 2, 1, 1, 0, 0 };

        public TaskListEditorViewModel()
        {
            Filter = new Filter() { IsRotate = true, IsShoot = true };

            var list = new List<BaseTask>();

            DateTime dt = DateTime.UtcNow;

            string format = "dd-MMM-yyyy hh:mm:ss";

            for (int i = 0; i < randoms.Length; i++)
            {
                switch (randoms[i])
                {
                    case 0:
                        list.Add(new Task1()
                        {
                            Name = $"Task#{i:00}",
                            Begin = dt.AddHours(i).ToString(format, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")),
                            Duration = TimeSpan.FromSeconds((randoms[i] + 1) * 10.0),
                        });
                        break;
                    case 1:
                        list.Add(new Task2()
                        {
                            Name = $"Task#{i:00}",
                            Begin = dt.AddHours(i).ToString(format, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")),
                            Duration = TimeSpan.FromSeconds((randoms[i] + 1) * 10.0),
                        });
                        break;
                    case 2:
                        list.Add(new Task3()
                        {
                            Name = $"Task#{i:00}",
                            Begin = dt.AddHours(i).ToString(format, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")),
                            Duration = TimeSpan.FromSeconds((randoms[i] + 1) * 10.0),
                        });
                        break;
                    default:
                        throw new Exception();
                }
            }

            _tasks = new ObservableCollection<BaseTask>(list);
        }

        public Filter Filter { get; set; }

        public ObservableCollection<BaseTask> Tasks
        {
            get => _tasks;
            set => this.RaiseAndSetIfChanged(ref _tasks, value);
        }
    }
}

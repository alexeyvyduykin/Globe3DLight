using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;

namespace Globe3DLight.ViewModels.Editors
{
    public enum EntityMode { Mode1, Mode2, Mode3 };

    public class Entity : ViewModelBase
    {
        double _value1;
        double _value2;
        string _name1;
        string _name2;
        bool _is1;
        bool _is2;
        EntityMode _mode;
        uint _color;
        Color _avaloniaColor;

        public double Value1
        {
            get => _value1;
            set => this.RaiseAndSetIfChanged(ref _value1, value);
        }
        public double Value2
        {
            get => _value2;
            set => this.RaiseAndSetIfChanged(ref _value2, value);
        }
        public string Name1
        {
            get => _name1;
            set => this.RaiseAndSetIfChanged(ref _name1, value);
        }
        public string Name2
        {
            get => _name2;
            set => this.RaiseAndSetIfChanged(ref _name2, value);
        }
        public bool Is1
        {
            get => _is1;
            set => this.RaiseAndSetIfChanged(ref _is1, value);
        }
        public bool Is2
        {
            get => _is2;
            set => this.RaiseAndSetIfChanged(ref _is2, value);
        }
        public EntityMode Mode
        {
            get => _mode;
            set => this.RaiseAndSetIfChanged(ref _mode, value);
        }
        public uint Color
        {
            get => _color;
            set => this.RaiseAndSetIfChanged(ref _color, value);
        }
        public Color AvaloniaColor
        {
            get => _avaloniaColor;
            set => this.RaiseAndSetIfChanged(ref _avaloniaColor, value);
        }
    }

    public class Scene : ViewModelBase
    {

    }

    public class Scenario : ViewModelBase
    {

    }

    public class PropertiesEditorViewModel : ViewModelBase
    {
        Entity _entity;

        public PropertiesEditorViewModel()
        {
            _entity = new Entity()
            {
                Value1 = 0.434343436565,
                Value2 = 100,
                Name1 = "ShortName",
                Name2 = "LongNameLongNameLongName",
                Is1 = false,
                Is2 = true,
                Mode = EntityMode.Mode2,
                Color = 0xff0000ff,// Brushes.Blue
                AvaloniaColor = Colors.Azure,
            };


        }

        public Entity Entity
        {
            get => _entity;
            set => this.RaiseAndSetIfChanged(ref _entity, value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System.Reflection;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Reactive.Disposables;
using GlmSharp;
using Globe3DLight.Models;
using Globe3DLight.Models.Data;
using Globe3DLight.Models.Scene;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.ViewModels.Entities;
using Globe3DLight.ViewModels.Editors;
using Globe3DLight.ViewModels.Containers;

namespace Globe3DLight.ViewModels.Editors
{
    public class PropertiesEditorViewModel : ViewModelBase
    {
        private readonly ScenarioContainerViewModel _scenario;

        public PropertiesEditorViewModel(ScenarioContainerViewModel scenario)
        {
            _scenario = scenario;
        }

    }
}

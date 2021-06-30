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
    public enum DisplayMode
    {
        Visual, 
        Logical
    }

    public class OutlinerEditorViewModel : ViewModelBase
    {
        private ScenarioContainerViewModel _scenario;

        private ImmutableArray<FrameViewModel> _frameRoot;
        private FrameViewModel _currentFrame;

        private ImmutableArray<BaseEntity> _entities;
        private BaseEntity _currentEntity;

        private DisplayMode _selectedMode;
        private ObservableCollection<ViewModelBase> _items;
        private ViewModelBase _selectedItem;

        public OutlinerEditorViewModel(ScenarioContainerViewModel scenario)
        {
            _scenario = scenario;
            PropertyChanged += OutlinerEditorViewModel_PropertyChanged;
        }

        private void OutlinerEditorViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OutlinerEditorViewModel.SelectedMode))
            {
                switch (SelectedMode)
                {
                    case DisplayMode.Visual:
                        InvalidateVisual();
                        break;
                    case DisplayMode.Logical:
                        InvalidateLogical();
                        break;
                    default:
                        break;
                }
            }
            else if(e.PropertyName == nameof(OutlinerEditorViewModel.SelectedItem))
            {
                switch (SelectedMode)
                {
                    case DisplayMode.Visual:
                        if (SelectedItem is BaseEntity entity)
                        {
                            CurrentEntity = entity;
                        }
                        break;
                    case DisplayMode.Logical:
                        if (SelectedItem is FrameViewModel frame)
                        {
                            CurrentFrame = frame;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void InvalidateVisual()
        {                    
            Items = new ObservableCollection<ViewModelBase>(_entities);                    
            SelectedItem = _entities.FirstOrDefault();
        }

        private void InvalidateLogical()
        {                    
            Items = new ObservableCollection<ViewModelBase>(_frameRoot);                    
            SelectedItem = _frameRoot.FirstOrDefault();
        }

        public ImmutableArray<FrameViewModel> FrameRoot
        {
            get => _frameRoot;
            set                                 
            {                 
                RaiseAndSetIfChanged(ref _frameRoot, value);
                InvalidateLogical();
            }
        }

        public FrameViewModel CurrentFrame
        {
            get => _currentFrame;
            set => RaiseAndSetIfChanged(ref _currentFrame, value);
        }

        public ImmutableArray<BaseEntity> Entities
        {
            get => _entities;
            set 
            { 
                RaiseAndSetIfChanged(ref _entities, value);
                InvalidateVisual();
            }
        }

        public BaseEntity CurrentEntity
        {
            get => _currentEntity;
            set => RaiseAndSetIfChanged(ref _currentEntity, value);
        }

        public DisplayMode SelectedMode 
        {
            get => _selectedMode; 
            set => this.RaiseAndSetIfChanged(ref _selectedMode, value); 
        }

        public ObservableCollection<ViewModelBase> Items 
        {
            get => _items;
            set => this.RaiseAndSetIfChanged(ref _items, value);
        }

        public ViewModelBase SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        public override IDisposable Subscribe(IObserver<(object sender, PropertyChangedEventArgs e)> observer)
        {
            var mainDisposable = new CompositeDisposable();
            var disposablePropertyChanged = default(IDisposable);          
            var disposableShapes = default(CompositeDisposable);

            ObserveSelf(Handler, ref disposablePropertyChanged, mainDisposable);   
            ObserveList(_entities, ref disposableShapes, mainDisposable, observer);

            void Handler(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == nameof(OutlinerEditorViewModel.Entities))
                {
                    ObserveList(_entities, ref disposableShapes, mainDisposable, observer);
                }

                observer.OnNext((sender, e));
            }

            return mainDisposable;
        }

        public void OnSetCameraTo(ViewModelBase item)
        {
            if (item is ITargetable target)
            {
                _scenario.SetCameraTo(target);

                SelectedItem = item;
            }
        }
    }
}

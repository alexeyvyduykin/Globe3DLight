#nullable disable
using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Reactive.Disposables;
using Globe3DLight.ViewModels.Editors;

namespace Globe3DLight.ViewModels.Containers
{
    public partial class ProjectContainerViewModel : BaseContainerViewModel
    {
        private ImmutableArray<ScenarioContainerViewModel> _scenarios;
        private ScenarioContainerViewModel _currentScenario;
        private ViewModelBase _selected;
        private TopBarViewModel _topBar;

        public ProjectContainerViewModel()
        {
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Selected))
                {
                    SetSelected(Selected);
                }
            };
        }

        public ImmutableArray<ScenarioContainerViewModel> Scenarios
        {
            get => _scenarios;
            set => RaiseAndSetIfChanged(ref _scenarios, value);
        }

        public ScenarioContainerViewModel CurrentScenario
        {
            get => _currentScenario;
            set => RaiseAndSetIfChanged(ref _currentScenario, value);
        }

        public ViewModelBase Selected
        {
            get => _selected;
            set => RaiseAndSetIfChanged(ref _selected, value);
        }

        public TopBarViewModel TopBar
        {
            get => _topBar;
            set => RaiseAndSetIfChanged(ref _topBar, value);
        }

        public void SetCurrentScenario(ScenarioContainerViewModel scenario)
        {
            CurrentScenario = scenario;
            Selected = scenario;
        }

        public void SetSelected(ViewModelBase value)
        {
            if (value is ScenarioContainerViewModel scenario)
            {
                CurrentScenario = scenario;
            }
        }

        public override IDisposable Subscribe(IObserver<(object sender, PropertyChangedEventArgs e)> observer)
        {
            var mainDisposable = new CompositeDisposable();
            var disposablePropertyChanged = default(IDisposable);
            var disposableDocuments = default(CompositeDisposable);

            ObserveSelf(Handler, ref disposablePropertyChanged, mainDisposable);
            ObserveList(_scenarios, ref disposableDocuments, mainDisposable, observer);

            void Handler(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == nameof(Scenarios))
                {
                    ObserveList(_scenarios, ref disposableDocuments, mainDisposable, observer);
                }

                observer.OnNext((sender, e));
            }

            return mainDisposable;
        }
    }
}

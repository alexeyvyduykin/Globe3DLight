using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels.Scene;
using System.Linq;
using Globe3DLight.Models.Renderer;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.ViewModels.Entities;
using Globe3DLight.Models;
using Globe3DLight.Models.Editor;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.ComponentModel;

namespace Globe3DLight.ViewModels.Editor
{
    public class ProjectEditorViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Lazy<IFactory> _factory;
        private string _projectPath;
        private readonly Lazy<IContainerFactory> _containerFactory;    
        private readonly Lazy<IJsonSerializer> _jsonSerializer;
        private readonly Lazy<IFileSystem> _fileIO;
        private readonly Lazy<IRenderContext> _renderer;
        private readonly Lazy<IPresenterContract> _presenter;    
        private ProjectContainerViewModel _project;
        private readonly Lazy<IEditorTool> _currentTool;
        private readonly Lazy<IEditorCanvasPlatform> _canvasPlatform;
        private IDisposable _observer;
        private readonly Lazy<IProjectEditorPlatform> _platform;

        public ProjectContainerViewModel Project
        {
            get => _project;
            set => RaiseAndSetIfChanged(ref _project, value);
        }

        public string ProjectPath
        {
            get => _projectPath;
            set => RaiseAndSetIfChanged(ref _projectPath, value);
        }

        public IDisposable Observer
        {
            get => _observer;
            set => RaiseAndSetIfChanged(ref _observer, value);
        }

        public IEditorTool CurrentTool => _currentTool.Value;

        public IRenderContext Renderer => _renderer.Value;

        public IPresenterContract Presenter => _presenter.Value;

        public IContainerFactory ContainerFactory => _containerFactory.Value;

        public IFactory Factory => _factory.Value;

        public IEditorCanvasPlatform CanvasPlatform => _canvasPlatform.Value;

        public IJsonSerializer JsonSerializer => _jsonSerializer.Value;

        public IFileSystem FileIO => _fileIO.Value;

        public IProjectEditorPlatform Platform => _platform.Value;

        public ProjectEditorViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _factory = _serviceProvider.GetServiceLazily<IFactory>();       
            _containerFactory = _serviceProvider.GetServiceLazily<IContainerFactory>();   
            _renderer = _serviceProvider.GetServiceLazily<IRenderContext>();
            _presenter = _serviceProvider.GetServiceLazily<IPresenterContract>();   
            _currentTool = _serviceProvider.GetServiceLazily<IEditorTool>();
            _jsonSerializer = _serviceProvider.GetServiceLazily<IJsonSerializer>();
            _fileIO = _serviceProvider.GetServiceLazily<IFileSystem>();       
            _platform = _serviceProvider.GetServiceLazily<IProjectEditorPlatform>();         
            _canvasPlatform = _serviceProvider.GetServiceLazily<IEditorCanvasPlatform>();
        }

        public void OnUpdate()
        {
            if (Project != null)
            {
                Project.CurrentScenario.LogicalUpdate();            
            }
        }

        public void OnNewProject()
        {
            //Project = Factory.CreateProjectContainer();

            OnUnload();
            OnLoad(ContainerFactory?.GetProject() ?? Factory.CreateProjectContainer(), string.Empty);

            CanvasPlatform?.InvalidateControl?.Invoke();
        }

        public async void OnFromDatabaseProject()
        {
            var project = await ContainerFactory.GetFromDatabase();

            OnOpenProject(project, "");
        }

        public async void OnFromJsonProject()
        {
            var project = await ContainerFactory.GetFromJson();

            OnOpenProject(project, "");
        }

        public async void OnFromDatabaseToJson()
        {
            await ContainerFactory.SaveFromDatabaseToJson();
        }

        public void OnOpenProject(ProjectContainerViewModel project, string path)
        {
            try
            {
                if (project != null)
                {
                    OnUnload();
                    OnLoad(project, path);
                    //    OnAddRecent(path, project.Name);
                    //    CanvasPlatform?.ResetZoom?.Invoke();
                    OnUpdate();
                    CanvasPlatform?.InvalidateControl?.Invoke();
                }
            }
            catch //(Exception ex)
            {
               // Log?.LogException(ex);
            }
        }

        public void OnOpenProject(string path)
        {
            try
            {
                if (FileIO != null && JsonSerializer != null)
                {
                    if (string.IsNullOrEmpty(path) == false && FileIO.Exists(path) == true)
                    {
                        var project = Factory.OpenProjectContainer(path, FileIO, JsonSerializer);
                        if (project != null)
                        {
                            OnOpenProject(project, path);
                        }
                    }
                }
            }
            catch //(Exception ex)
            {
                //Log?.LogException(ex);
            }
        }

        public void OnSaveProject(string path)
        {
            try
            {
                if (Project != null && FileIO != null && JsonSerializer != null)
                {
                    Factory.SaveProjectContainer(Project, path, FileIO, JsonSerializer);

                    if (string.IsNullOrEmpty(ProjectPath))
                    {
                        ProjectPath = path;
                    }
                }
            }
            catch //(Exception ex)
            {
                //Log?.LogException(ex);
            }
        }

        public void OnImportJson(string path)
        {
            try
            {
                var json = FileIO?.ReadUtf8Text(path);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    var item = JsonSerializer.Deserialize<object>(json);
                    if (item != null)
                    {
                        OnImportObject(item, true);
                    }
                }
            }
            catch //(Exception ex)
            {
                //Log?.LogException(ex);
            }
        }

        public void OnExportJson(string path, object item)
        {
            try
            {
                var json = JsonSerializer?.Serialize(item);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    FileIO?.WriteUtf8Text(path, json);
                }
            }
            catch //(Exception ex)
            {
                //Log?.LogException(ex);
            }
        }
        //public void OnExport(string path, object item, IFileWriter writer)
        //{
        //    try
        //    {
        //        using var stream = FileIO.Create(path);
        //        writer?.Save(stream, item, Project);
        //    }
        //    catch //(Exception ex)
        //    {
        //        //Log?.LogException(ex);
        //    }
        //}

        public void OnImportObject(object item, bool restore)
        {
            if (item is BaseEntity entity)
            {
                Project.AddEntity(entity);
            }
            else if (item is ProjectContainerViewModel project)
            {
                OnUnload();
                OnLoad(project, string.Empty);
            }
            else
            {
                throw new NotSupportedException("Not supported import object.");
            }
        }

        public void OnAddScenario()
        {
            if (Project != null)
            {
                var scenario = ContainerFactory.GetScenario("Scenario1", DateTime.Now, TimeSpan.FromDays(1));

                Project.AddScenario(scenario);
                Project.SetCurrentScenario(scenario);
            }
        }

        //void OnRemoveScenario(IScenarioContainer scenario);
        public void OnRemove(object item)
        {
            if (item is ScenarioContainerViewModel scenario)
            {
                Project?.RemoveScenario(scenario);               
                var selected = Project?.Scenarios.FirstOrDefault();
                Project?.SetCurrentScenario(selected);
            }
            if (item is LogicalViewModel node)
            {
                Project?.RemoveLogicalNode(node);
              //  var selected = Project?.CurrentScenario?.LogicalTreeNodeRoot.SingleOrDefault();
               // Project?.CurrentScenario?.CurrentLogicalTreeNode = selected;


               // Project.SetSelected(selected);
                //var selected = Project?.CurrentDocument?.Pages.FirstOrDefault();
                //Project?.SetCurrentContainer(selected);
            }
            else if (item is ProjectEditorViewModel || item == null)
            {
              //  OnDeleteSelected();
            }
        }

        //void OnAddChildFrame(ITreeNode<IFrame> node);
        public void OnAddFrame(object item)
        {
            if (item is LogicalViewModel node)
            {
                if (Project?.CurrentScenario != null)
                {
                    var child = Factory.CreateFrameState("Frame");

                    Project.AddChildFrame(node, child);
                }
            }
        }

        public void OnSetCameraTo()
        {
            if (Project?.CurrentScenario != null && Project?.Selected != null)
            {
                if (Project.Selected is ITargetable target)
                {
                    Project.CurrentScenario.SetCameraTo(target);
                }       
            }
        }

        //public void OnImportJson__(string path)
        //{
        //    try
        //    {
        //        var json = FileIO?.ReadUtf8Text(path);
        //        if (!string.IsNullOrWhiteSpace(json))
        //        {
        //            var item = JsonSerializer.Deserialize<object>(json);
        //            if (item != null)
        //            {
        //                var name = System.IO.Path.GetFileNameWithoutExtension(path);
        //                OnImportObject__(name, item);
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
               
        //    }
        //}

        //public void OnImportObject__(string name, object item)
        //{
        //    if (item is IDatabase db)
        //    {
        //        var data = DataFactory.CreateDataFromDatabase(db);            
        //        OnImportObject__(name, data);
        //    }
        //    else if(item is IData data)
        //    {
        //        var node = Factory.CreateLogicalTreeNode(name, data);
        //        OnImportObject__(name, node);
        //    }
        //    else if (item is ILogicalTreeNode child)
        //    {
        //        Project?.AddChildFrame(Project.CurrentScenario.CurrentLogicalTreeNode, child);
        //    }
        //    else
        //    {
        //        throw new NotSupportedException("Not supported import object.");
        //    }
        //}

        public void OnLoad(ProjectContainerViewModel project, string path = null)
        {
            if (project != null)
            {
                Project = project;    
                ProjectPath = path;
        //        IsProjectDirty = false;
               
                var propertyChangedSubject = new Subject<(object sender, PropertyChangedEventArgs e)>();
                var propertyChangedDisposable = Project.Subscribe(propertyChangedSubject);
                var observable = propertyChangedSubject.Subscribe(ProjectChanged);

                Observer = new CompositeDisposable(propertyChangedDisposable, observable, propertyChangedSubject);

                void ProjectChanged((object sender, PropertyChangedEventArgs e) arg)
                {             
                    // _project?.CurrentContainer?.InvalidateLayer();
                    CanvasPlatform?.InvalidateControl?.Invoke();
                    //IsProjectDirty = true;
                }
            }
        }

        public void OnUnload()
        {
            if (Observer is { })
            {
                Observer?.Dispose();
                Observer = null;
            }

            if (Project != null)
            {
                Project = null;
                //     ProjectPath = string.Empty;
                //     IsProjectDirty = false;
                GC.Collect();
            }
        }

        public string GetName(object item)
        {
            if (item != null)
            {
                if (item is ViewModelBase observable)
                {
                    return observable.Name;
                }
            }
            return string.Empty;
        }

        public void OnNew(object item)
        {
            if (item is ProjectEditorViewModel)
            {
                OnNewProject();
            }
            else if (item == null)
            {
                if (Project == null)
                {
                    OnNewProject();
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        public void OnCloseProject()
        {
            //Project?.History?.Reset();
            OnUnload();
        }
    }
}

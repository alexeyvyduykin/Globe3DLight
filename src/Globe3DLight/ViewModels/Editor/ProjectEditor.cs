using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Containers;
using Globe3DLight.Scene;
using System.Linq;
using Globe3DLight.Renderer;
using Globe3DLight.Data;
using Globe3DLight.ScenarioObjects;

namespace Globe3DLight.Editor
{
    public class ProjectEditor : ObservableObject, IProjectEditor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Lazy<IFactory> _factory;
        private string _projectPath;
        private readonly Lazy<IDataFactory> _dataFactory;
        private readonly Lazy<IContainerFactory> _containerFactory;
        private readonly Lazy<IScenarioObjectFactory> _sceneFactory;
        private readonly Lazy<IJsonSerializer> _jsonSerializer;
        private readonly Lazy<IFileSystem> _fileIO;
        private readonly Lazy<IRenderContext> _renderer;
        private readonly Lazy<IDataUpdater> _updater;
        private IProjectContainer _project;
        private readonly Lazy<IEditorTool> _currentTool;
        private readonly Lazy<IEditorCanvasPlatform> _canvasPlatform;
        private ProjectObserver _observer;
        private readonly Lazy<IProjectEditorPlatform> _platform;

        public IProjectContainer Project
        {
            get => _project;
            set => Update(ref _project, value);
        }
        public string ProjectPath
        {
            get => _projectPath;
            set => Update(ref _projectPath, value);
        }
        public ProjectObserver Observer
        {
            get => _observer;
            set => Update(ref _observer, value);
        }
        public IEditorTool CurrentTool => _currentTool.Value;

        public IRenderContext Renderer => _renderer.Value;

        public IDataUpdater Updater => _updater.Value;

        public IContainerFactory ContainerFactory => _containerFactory.Value;
        public IFactory Factory => _factory.Value;
        public IDataFactory DataFactory => _dataFactory.Value;
        public IScenarioObjectFactory SceneFactory => _sceneFactory.Value;
        public IEditorCanvasPlatform CanvasPlatform => _canvasPlatform.Value;

        public IJsonSerializer JsonSerializer => _jsonSerializer.Value;

        public IFileSystem FileIO => _fileIO.Value;
        public IProjectEditorPlatform Platform => _platform.Value;

        public ProjectEditor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _factory = _serviceProvider.GetServiceLazily<IFactory>();
            _dataFactory = _serviceProvider.GetServiceLazily<IDataFactory>();
            _containerFactory = _serviceProvider.GetServiceLazily<IContainerFactory>();
            _sceneFactory = _serviceProvider.GetServiceLazily<IScenarioObjectFactory>();
            _renderer = _serviceProvider.GetServiceLazily<IRenderContext>();
            _updater = _serviceProvider.GetServiceLazily<IDataUpdater>();
            _currentTool = _serviceProvider.GetServiceLazily<IEditorTool>();
            _jsonSerializer = _serviceProvider.GetServiceLazily<IJsonSerializer>();
            _fileIO = _serviceProvider.GetServiceLazily<IFileSystem>();
            //      _shapeFactory = _serviceProvider.GetServiceLazily<IShapeFactory>();
            
            _platform = _serviceProvider.GetServiceLazily<IProjectEditorPlatform>();         
            _canvasPlatform = _serviceProvider.GetServiceLazily<IEditorCanvasPlatform>();

        }

        public void OnUpdate(double t)
        {
            if (Project != null)
            {
                Updater.Update(t, Project.CurrentScenario.LogicalTreeNodeRoot.SingleOrDefault());                
            }
        }

        public void OnNewProject()
        {
            //Project = Factory.CreateProjectContainer();

            OnUnload();
            OnLoad(ContainerFactory?.GetProject() ?? Factory.CreateProjectContainer(), string.Empty);

            CanvasPlatform?.InvalidateControl?.Invoke();
        }

        public void OnDemoProject()
        {
            OnOpenProject(ContainerFactory.GetDemo(), "");
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

        public void OnOpenProject(IProjectContainer project, string path)
        {
            try
            {
                if (project != null)
                {
                    OnUnload();
                    OnLoad(project, path);
                    //    OnAddRecent(path, project.Name);
                    //    CanvasPlatform?.ResetZoom?.Invoke();
                    OnUpdate(0.0);
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
            if (item is IScenarioObject scenarioObject)
            {
                Project.AddScenarioObject(scenarioObject);
            }
            else if (item is IProjectContainer project)
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
            if (item is IScenarioContainer scenario)
            {
                Project?.RemoveScenario(scenario);               
                var selected = Project?.Scenarios.FirstOrDefault();
                Project?.SetCurrentScenario(selected);
            }
            if (item is ILogicalTreeNode node)
            {
                Project?.RemoveLogicalNode(node);
              //  var selected = Project?.CurrentScenario?.LogicalTreeNodeRoot.SingleOrDefault();
               // Project?.CurrentScenario?.CurrentLogicalTreeNode = selected;


               // Project.SetSelected(selected);
                //var selected = Project?.CurrentDocument?.Pages.FirstOrDefault();
                //Project?.SetCurrentContainer(selected);
            }
            else if (item is IProjectEditor || item == null)
            {
              //  OnDeleteSelected();
            }
        }

        //void OnAddChildFrame(ITreeNode<IFrame> node);
        public void OnAddFrame(object item)
        {
            if (item is ILogicalTreeNode node)
            {
                if (Project?.CurrentScenario != null)
                {
                    var child = Factory.CreateLogicalTreeNode("Frame", DataFactory.CreateFrameState());

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
                    var camera = SceneFactory.CreateArcballCamera(target);
                    Project.CurrentScenario.SceneState.Camera = camera;
                    Project.CurrentScenario.SceneState.Target = target;
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

        public void OnLoad(IProjectContainer project, string path = null)
        {
            if (project != null)
            {
                //Deselect();
                //if (project is IImageCache imageCache)
                //{
                //    SetRenderersImageCache(imageCache);
                //}
                Project = project;
        //        Project.History = new StackHistory();
                ProjectPath = path;
        //        IsProjectDirty = false;
                Observer = new ProjectObserver(this);
            }
        }

        /// <inheritdoc/>
        public void OnUnload()
        {
            if (Observer != null)
            {
                Observer?.Dispose();
                Observer = null;
            }

            //if (Project?.History != null)
            //{
            //    Project.History.Reset();
            //    Project.History = null;
            //}

            if (Project != null)
            {
           //     if (Project is IImageCache imageCache)
           //     {
           //         imageCache.PurgeUnusedImages(new HashSet<string>());
           //     }
           //     Deselect();
           //     SetRenderersImageCache(null);
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
                if (item is IObservableObject observable)
                {
                    return observable.Name;
                }
            }
            return string.Empty;
        }


        public void OnNew(object item)
        {
            if (item is IProjectEditor)
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


        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}

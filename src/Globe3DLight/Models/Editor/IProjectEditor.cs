using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Containers;
using Globe3DLight.Data;
using Globe3DLight.Renderer;

namespace Globe3DLight.Editor
{
    public interface IProjectEditor : IObservableObject
    {
        IProjectContainer Project { get; set; }
        string ProjectPath { get; set; }
        IContainerFactory ContainerFactory { get; }
        IEditorCanvasPlatform CanvasPlatform { get; }
        ProjectObserver Observer { get; set; }
        IRenderContext Renderer { get; }
        IPresenterContract Presenter { get; }
        IDataUpdater Updater { get; }

        IEditorTool CurrentTool { get; }
        IProjectEditorPlatform Platform { get; }
        void OnUpdate(double t);
        
        void OnNewProject();
        void OnDemoProject();
        void OnFromDatabaseProject();
        void OnFromJsonProject();
        void OnOpenProject(IProjectContainer project, string path);
        void OnOpenProject(string path); 
        void OnSaveProject(string path);
        void OnAddScenario();

        void OnRemove(object item);
   
        void OnAddFrame(object item);

        void OnSetCameraTo();
        void OnExportJson(string path, object item);
        void OnImportJson(string path);
        //void OnImportJson__(string path);
        //void OnImportObject__(string name, object item);
        void OnImportObject(object item, bool restore);

        string GetName(object item);

        void OnNew(object item);

        void OnCloseProject();
    }
}

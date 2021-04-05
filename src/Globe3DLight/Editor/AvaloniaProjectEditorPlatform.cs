#nullable enable
using System;
using System.Linq;
using Avalonia.Controls;
using Globe3DLight.Models;
using Globe3DLight.Models.Editor;
using Globe3DLight.ViewModels.Editor;
using Globe3DLight.Views;

namespace Globe3DLight.Editor
{
    public class AvaloniaProjectEditorPlatform : IProjectEditorPlatform
    {
        private readonly IServiceProvider _serviceProvider;

        public AvaloniaProjectEditorPlatform(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        private MainWindow GetWindow()
        {
            return _serviceProvider.GetService<MainWindow>();
        }

        public void OnExit()
        {
            GetWindow().Close();
        }

        public async void OnOpen(string path)
        {
            if (path == null)
            {
                var dlg = new OpenFileDialog() { Title = "Open" };
                dlg.Filters.Add(new FileDialogFilter() { Name = "Project", Extensions = { "globe3d.json" } });
                dlg.Filters.Add(new FileDialogFilter() { Name = "All", Extensions = { "*" } });
                var result = await dlg.ShowAsync(GetWindow());
                if (result != null)
                {
                    var item = result.FirstOrDefault();
                    if (item != null)
                    {
                        var editor = _serviceProvider.GetService<ProjectEditorViewModel>();
                        editor.OnOpenProject(item);
                        editor.CanvasPlatform?.InvalidateControl?.Invoke();
                    }
                }
            }
            else
            {
                if (_serviceProvider.GetService<IFileSystem>().Exists(path))
                {
                    _serviceProvider.GetService<ProjectEditorViewModel>().OnOpenProject(path);
                }
            }
        }

        public void OnSave()
        {
            var editor = _serviceProvider.GetService<ProjectEditorViewModel>();
            if (!string.IsNullOrEmpty(editor.ProjectPath))
            {
                editor.OnSaveProject(editor.ProjectPath);
            }
            else
            {
                OnSaveAs();
            }
        }

        public async void OnSaveAs()
        {
            var editor = _serviceProvider.GetService<ProjectEditorViewModel>();
            var dlg = new SaveFileDialog() { Title = "Save" };
            dlg.Filters.Add(new FileDialogFilter() { Name = "Project", Extensions = { "globe3d.json" } });
            dlg.Filters.Add(new FileDialogFilter() { Name = "All", Extensions = { "*" } });
            dlg.InitialFileName = editor.Project?.Name;
            dlg.DefaultExtension = "globe3d.json";
            var result = await dlg.ShowAsync(GetWindow());
            if (result != null)
            {
                editor.OnSaveProject(result);
            }
        }

        public async void OnImportJson(string path)
        {
            if (path == null)
            {
                var dlg = new OpenFileDialog() { Title = "Open" };
                dlg.AllowMultiple = true;
                dlg.Filters.Add(new FileDialogFilter() { Name = "Json", Extensions = { "json" } });
                dlg.Filters.Add(new FileDialogFilter() { Name = "All", Extensions = { "*" } });

                var result = await dlg.ShowAsync(GetWindow());
                if (result != null)
                {
                    foreach (var item in result)
                    {
                        if (item != null)
                        {
                            _serviceProvider.GetService<ProjectEditorViewModel>().OnImportJson(item);
                        }
                    }
                }
            }
            else
            {
                if (_serviceProvider.GetService<IFileSystem>().Exists(path))
                {
                    _serviceProvider.GetService<ProjectEditorViewModel>().OnImportJson(path);
                }
            }
        }

        public async void OnImportObject(string path)
        {
            if (path == null)
            {
                var dlg = new OpenFileDialog() { Title = "Open" };
                dlg.AllowMultiple = true;
                dlg.Filters.Add(new FileDialogFilter() { Name = "Json", Extensions = { "json" } });
                var result = await dlg.ShowAsync(GetWindow());
                if (result != null)
                {
                    foreach (var item in result)
                    {
                        if (item != null)
                        {
                            string resultExtension = System.IO.Path.GetExtension(item);
                            if (string.Compare(resultExtension, ".json", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                _serviceProvider.GetService<ProjectEditorViewModel>().OnImportJson(item);
                            }
                        }
                    }
                }
            }
            else
            {
                if (_serviceProvider.GetService<IFileSystem>().Exists(path))
                {
                    string resultExtension = System.IO.Path.GetExtension(path);
                    if (string.Compare(resultExtension, ".json", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        _serviceProvider.GetService<ProjectEditorViewModel>().OnImportJson(path);
                    }
                }
            }
        }

        public async void OnExportJson(object item)
        {
            var editor = _serviceProvider.GetService<ProjectEditorViewModel>();
            var dlg = new SaveFileDialog() { Title = "Save" };
            dlg.Filters.Add(new FileDialogFilter() { Name = "Json", Extensions = { "json" } });
            dlg.Filters.Add(new FileDialogFilter() { Name = "All", Extensions = { "*" } });
            dlg.InitialFileName = editor?.GetName(item);
            dlg.DefaultExtension = "json";
            var result = await dlg.ShowAsync(GetWindow());
            if (result != null)
            {
                editor?.OnExportJson(result, item);
            }
        }

        public async void OnExportObject(object item)
        {
            var editor = _serviceProvider.GetService<ProjectEditorViewModel>();
            if (item != null)
            {
                var dlg = new SaveFileDialog() { Title = "Save" };
                dlg.Filters.Add(new FileDialogFilter() { Name = "Json", Extensions = { "json" } });
                dlg.InitialFileName = editor?.GetName(item);
                dlg.DefaultExtension = "json";
                var result = await dlg.ShowAsync(GetWindow());
                if (result != null)
                {
                    string resultExtension = System.IO.Path.GetExtension(result);
                    if (string.Compare(resultExtension, ".json", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        editor?.OnExportJson(result, item);
                    }
                }
            }
        }
    }
}

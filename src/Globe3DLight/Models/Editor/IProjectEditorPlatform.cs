using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Models.Editor
{
    public interface IProjectEditorPlatform
    {
        void OnExit();

        void OnOpen(string path);

        void OnSave();

        void OnSaveAs();

        void OnImportJson(string path);

        void OnImportObject(string path);

        void OnExportJson(object item);

        void OnExportObject(object item);
    }
}

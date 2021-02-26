using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
//using Globe3D.AvaloniaUI.ViewModels;

namespace Globe3DLight.AvaloniaUI
{
    public class ViewLocator : IDataTemplate
    {
        public bool SupportsRecycling => false;

        //public IControl Build(object data)
        //{
        //    var name = data.GetType().FullName.Replace("ViewModel", "View");
        //    var type = Type.GetType(name);

        //    if (type != null)
        //    {
        //        return (Control)Activator.CreateInstance(type);
        //    }
        //    else
        //    {
        //        return new TextBlock { Text = "Not Found: " + name };
        //    }
        //}

        public IControl Build(object data)
        {
            var name = data.GetType()?.FullName?.Replace("Globe3DLight", "Globe3DLight.AvaloniaUI.Views") + "Control";
            if (name == null)
            {
                return new TextBlock { Text = "Invalid Data Type" };
            }
            var type = Type.GetType(name);
            if (type != null)
            {
                var instance = Activator.CreateInstance(type);
                if (instance != null)
                {
                    return (Control)instance;
                }
                else
                {
                    return new TextBlock { Text = "Create Instance Failed: " + type.FullName };
                }
            }
            else
            {
                return new TextBlock { Text = "Not Found: " + name };
            }
        }
        public bool Match(object data)
        {
            return data is ObservableObject;
        }
    }
}
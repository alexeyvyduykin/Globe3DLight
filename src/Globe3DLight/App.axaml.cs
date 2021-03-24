using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Globe3DLight.Views;
using Autofac;
using System;
using Globe3DLight.ViewModels.Designer;
using Avalonia.Controls;
using System.Text;
using Globe3DLight;
using Globe3DLight.ViewModels.Editor;
using Globe3DLight.Modules;
using Globe3DLight.Models.Editor;

namespace Globe3DLight
{
    public class App : Application
    {
        static App()
        {
            InitializeDesigner();
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static void InitializeDesigner()
        {
            if (Design.IsDesignMode)
            {
                var builder = new ContainerBuilder();

                builder.RegisterModule<AppModule>();

                var container = builder.Build();

                DesignerContext.InitializeContext(container.Resolve<IServiceProvider>());
            }
        }

        //public override void OnFrameworkInitializationCompleted()
        //{
        //    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        //    {
        //        desktop.MainWindow = new MainWindow
        //        {
        //            DataContext = new MainWindowViewModel(),
        //        };
        //    }

        //    base.OnFrameworkInitializationCompleted();
        //}
        public override void OnFrameworkInitializationCompleted()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                InitializationClassicDesktopStyle(desktopLifetime);
            }

            base.OnFrameworkInitializationCompleted();
        }

        void Test()
        {
            Console.WriteLine("Again");
        }

        private void InitializationClassicDesktopStyle(IClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<AppModule>();

            var container = builder.Build();

            var serviceProvider = container.Resolve<IServiceProvider>();
            var containerFactory = serviceProvider.GetService<IContainerFactory>();
            var editor = serviceProvider.GetService<ProjectEditorViewModel>();


            //  editor.OnOpenProject(containerFactory.GetDemo(), "");

            //  editor.OnOpenProject(containerFactory.GetEmptyProject(), "");


            //  editor.IsToolIdle = true;

            var mainWindow = serviceProvider.GetService<MainWindow>();
            // var mainControl = mainWindow.FindControl<MainControl>("MainControl");

            mainWindow.DataContext = editor;

            mainWindow.Closing += (sender, e) => { };

            desktopLifetime.MainWindow = mainWindow;

            desktopLifetime.Exit += (sennder, e) =>
            {
                container.Dispose();
            };
        }

    }
}

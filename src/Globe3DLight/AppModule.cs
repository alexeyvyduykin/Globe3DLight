using System;
using Autofac;
using Globe3DLight.Editor;
using Globe3DLight.Views;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.DatabaseProvider.PostgreSQL;
using Globe3DLight.DataProvider.Json;
using Globe3DLight.ViewModels.Editor.Tools;
using Globe3DLight.FileSystem.DotNet;
using Globe3DLight.ImageLoader.SOIL;
using Globe3DLight.ModelLoader.Assimp;
using Globe3DLight.Renderer;
using Globe3DLight.Renderer.OpenTK;
using Globe3DLight.Serializer.Newtonsoft;
using Globe3DLight.ServiceProvider.Autofac;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;
using Globe3DLight.Models;
using Globe3DLight.ViewModels;
using Globe3DLight.ViewModels.Containers;
using Globe3DLight.Models.Renderer;
using Globe3DLight.ViewModels.Renderer;
using Globe3DLight.Models.Editor;
using Globe3DLight.ViewModels.Editor;
using Globe3DLight.Models.Data;

namespace Globe3DLight
{
    public class AppModule : Autofac.Module
    {
        /// <inheritdoc/>
        protected override void Load(ContainerBuilder builder)
        {
            // Locator

            builder.RegisterType<AutofacServiceProvider>().As<IServiceProvider>().InstancePerLifetimeScope();


            // Build configuration

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            builder.RegisterInstance(configuration).As<IConfigurationRoot>();

            // Core

            builder.RegisterType<ProjectEditorViewModel>().As<ProjectEditorViewModel>().InstancePerLifetimeScope();
            //    builder.RegisterType<StyleEditor>().As<IStyleEditor>().InstancePerLifetimeScope();
            builder.RegisterType<Factory>().As<IFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ContainerFactory>().As<IContainerFactory>().InstancePerLifetimeScope();
            builder.RegisterType<DataFactory>().As<IDataFactory>().InstancePerLifetimeScope();
            builder.RegisterType<RenderModelFactory>().As<IRenderModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ScenarioObjectFactory>().As<IScenarioObjectFactory>().InstancePerLifetimeScope();
          //  builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerLifetimeScope();

            //builder.RegisterType<OpenTKShaderProgram>().As<IShaderProgram>().InstancePerDependency();

            // Dependencies

            //builder.RegisterType<AvaloniaTextClipboard>().As<ITextClipboard>().InstancePerLifetimeScope();  
            builder.RegisterType<DotNetFileSystem>().As<IFileSystem>().InstancePerLifetimeScope();
            //builder.RegisterType<RoslynScriptRunner>().As<IScriptRunner>().InstancePerLifetimeScope();
            builder.RegisterType<NewtonsoftJsonSerializer>().As<IJsonSerializer>().InstancePerLifetimeScope();            
            //builder.RegisterType<DataUpdater>().As<IDataUpdater>().InstancePerLifetimeScope();



            // Data

            builder.RegisterType<PostgreSQLDatabaseProvider>().As<IDatabaseProvider>().InstancePerDependency();// InstancePerLifetimeScope();
            builder.RegisterType<JsonDataProvider>().As<IJsonDataProvider>().InstancePerLifetimeScope();

            // Renderer

            builder.RegisterType<OpenTKRenderer>().As<IRenderContext>().InstancePerLifetimeScope();
            builder.RegisterType<OpenTKPresenter>().As<IPresenterContract>().InstancePerLifetimeScope();



            //   builder.RegisterType</*OpenTK*/Device>().As<IDevice>().InstancePerLifetimeScope();
            //   builder.RegisterType<SOILDDSLoader>().As<IDDSLoader>().InstancePerLifetimeScope();
            builder.RegisterType<AssimpModelLoader>().As<IModelLoader>().InstancePerLifetimeScope();


            builder.RegisterType<ImageLibrary>().As<IImageLibrary>().InstancePerLifetimeScope();
            //  builder.RegisterType<PfimImageLoader>().As<IImageLoader>().InstancePerLifetimeScope();
            builder.RegisterType<SOILImageLoader>().As<IImageLoader>().InstancePerLifetimeScope();



            // Editor

            //  builder.RegisterType<AvaloniaImageImporter>().As<IImageImporter>().InstancePerLifetimeScope();
            builder.RegisterType<AvaloniaProjectEditorPlatform>().As<IProjectEditorPlatform>().InstancePerLifetimeScope();
            builder.RegisterType<AvaloniaEditorCanvasPlatform>().As<IEditorCanvasPlatform>().InstancePerLifetimeScope();
            builder.RegisterType<ToolDefault>().As<IEditorTool>().InstancePerLifetimeScope();

            //builder.RegisterType<AdvancedSceneTimer>().As<ISceneTimer>().InstancePerDependency();// InstancePerLifetimeScope();

            // View

            builder.RegisterType<MainWindow>().As<MainWindow>().InstancePerLifetimeScope();
        }
    }
}

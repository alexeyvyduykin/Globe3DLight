#nullable enable
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Globe3DLight.Models;
using Globe3DLight.Models.Data;
using Globe3DLight.Models.Editor;
using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.ViewModels.Entities;

namespace Globe3DLight.ViewModels.Editor
{
    public class ContainerFactory : IContainerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ContainerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ProjectContainerViewModel GetProject()
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var containerFactory = this as IContainerFactory;
            var project = factory.CreateProjectContainer("Project1");

            // Templates
            //   var templateBuilder = project.Templates.ToBuilder();
            //   templateBuilder.Add(CreateDefaultTemplate(this, project, "Default"));
            //   project.Templates = templateBuilder.ToImmutable();

            //   project.SetCurrentTemplate(project.Templates.FirstOrDefault(t => t.Name == "Default"));

            // Documents and Pages      
            var scenario = containerFactory.GetScenario("Scenario1", DateTime.Now, TimeSpan.FromDays(1));

            var scenarioBuilder = project.Scenarios.ToBuilder();
            scenarioBuilder.Add(scenario);
            project.Scenarios = scenarioBuilder.ToImmutable();

            // project.Selected = scenario.Pages.FirstOrDefault();

            return project;
        }

        public ProjectContainerViewModel? GetProject(ScenarioData data)
        {
            var factory = _serviceProvider.GetService<IFactory>();

            var epoch = FromJulianDate(data.JulianDateOnTheDay);
            var begin = epoch.AddSeconds(data.ModelingTimeBegin);
            var duration = TimeSpan.FromSeconds(data.ModelingTimeDuration);

            var project = factory.CreateProjectContainer("Project1");
            var scenario = GetScenario(data.Name, begin, duration);

            project.AddScenario(scenario);
            project.SetCurrentScenario(scenario);

            var root = scenario.LogicalRoot.First();

            project.AddEntity(factory.CreateSpacebox((BaseState)root));
            project.AddEntity(factory.CreateSun(data.Sun, (BaseState)root));
            var earth = project.AddEntity(factory.CreateEarth(data.Earth, (BaseState)root));
            project.AddEntity(factory.CreateGroundObjects(data, ((Earth)earth).Logical));
            EntityList gss = (EntityList)project.AddEntity(factory.CreateGroundStations(data, ((Earth)earth).Logical));
            EntityList rtrs = (EntityList)project.AddEntity(factory.CreateRetranslators(data, (BaseState)root));
            var satellites = project.AddEntities(factory.CreateSatellites(data, (BaseState)root, gss, rtrs));

            scenario.AddSatelliteTasks(factory.CreateSatelliteTasks(satellites, data));

            return project;
        }

        public ScenarioContainerViewModel GetScenario(string name, DateTime begin, TimeSpan duration)
        {
            var factory = _serviceProvider.GetService<IFactory>();

            var scenario = factory.CreateScenarioContainer(name);
            var root = factory.CreateFrameState("Root");

            //        root.Owner = scenario; ????????????????????????????????

            scenario.LogicalRoot = ImmutableArray.Create<LogicalViewModel>(root);
            scenario.CurrentLogical = scenario.LogicalRoot.FirstOrDefault();
            scenario.SceneState = factory.CreateSceneState();
            scenario.TimePresenter = factory.CreateSliderTimePresenter(begin, duration);
            //scenario.Tasks = ImmutableArray.Create<ISatelliteTask>();
            scenario.Updater = factory.CreateDataUpdater();

            return scenario;
        }

        public async Task<ProjectContainerViewModel?> GetFromDatabase()
        {
            try
            {
                return await _serviceProvider.GetService<IDatabaseProvider>().LoadProject();
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<ProjectContainerViewModel?> GetFromJson()
        {
            try
            {
                return await _serviceProvider.GetService<IJsonDataProvider>().LoadProject();
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task SaveFromDatabaseToJson()
        {
            try
            {
                await _serviceProvider.GetService<IDatabaseProvider>().Save();
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        private DateTime FromJulianDate(double jd) => DateTime.FromOADate(jd - 2415018.5);

        public ProjectContainerViewModel GetEmptyProject()
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var containerFactory = this as IContainerFactory;

            var project = factory.CreateProjectContainer("Project1");

            var scenario1 = containerFactory.GetScenario("Scenario1", DateTime.Now, TimeSpan.FromDays(1));

            project.AddScenario(scenario1);
            project.SetCurrentScenario(scenario1);

            return project;
        }
    }
}

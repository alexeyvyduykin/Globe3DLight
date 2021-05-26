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
            var project = factory.CreateProjectContainer("Project1");

            // Templates
            //   var templateBuilder = project.Templates.ToBuilder();
            //   templateBuilder.Add(CreateDefaultTemplate(this, project, "Default"));
            //   project.Templates = templateBuilder.ToImmutable();

            //   project.SetCurrentTemplate(project.Templates.FirstOrDefault(t => t.Name == "Default"));

            // Documents and Pages      
            var scenario = factory.CreateScenarioContainer("Scenario1", DateTime.Now, TimeSpan.FromDays(1));

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
            var scenario = factory.CreateScenarioContainer(data.Name, begin, duration);

            project.AddScenario(scenario);
            project.SetCurrentScenario(scenario);

            var root = scenario.FrameRoot.First();

            project.AddEntity(factory.CreateSpacebox(root));
            project.AddEntity(factory.CreateSun(data.Sun, root));
            var earth = project.AddEntity(factory.CreateEarth(data.Earth, root));
            var gos = project.AddEntity(factory.CreateGroundObjects(data, earth.Frame));
            var gss = project.AddEntity(factory.CreateGroundStations(data, earth.Frame));
            var rtrs = project.AddEntity(factory.CreateRetranslators(data, root));
            var satellites = project.AddEntities(factory.CreateSatellites(data, root, gss, rtrs));

            scenario.AddSatelliteTasks(factory.CreateSatelliteTasks(satellites, data));
            scenario.AddGroundObjectList(factory.CreateGroundObjectList(gos));            

            return project;
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
       
            var project = factory.CreateProjectContainer("Project1");
            var scenario1 = factory.CreateScenarioContainer("Scenario1", DateTime.Now, TimeSpan.FromDays(1));

            project.AddScenario(scenario1);
            project.SetCurrentScenario(scenario1);

            return project;
        }
    }
}

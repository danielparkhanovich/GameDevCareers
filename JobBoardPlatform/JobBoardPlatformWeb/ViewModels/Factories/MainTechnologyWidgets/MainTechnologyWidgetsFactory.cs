using JobBoardPlatform.DAL.Models.Enums;
using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.MainTechnologyWidgets;

namespace JobBoardPlatform.PL.ViewModels.Factories.MainTechnologyWidgets
{
    public class MainTechnologyWidgetsFactory : IViewModelAsyncFactory<List<MainTechnologyWidget>>
    {
        public Task<List<MainTechnologyWidget>> CreateAsync()
        {
            var enumValues = Enum.GetValues(typeof(MainTechnologyTypeEnum))
                .Cast<MainTechnologyTypeEnum>()
                .ToArray();
            var mainTechnologies = enumValues.Select(x => x.ToString()).ToList();
            mainTechnologies.Insert(0, "All");

            var technologyWidgets = new List<MainTechnologyWidget>();
            var widgetFactory = new MainTechnologyWidgetFactory();
            foreach (var mainTechnology in mainTechnologies)
            {
                var widget = widgetFactory.Create(mainTechnology);
                technologyWidgets.Add(widget);
            }

            return Task.FromResult(technologyWidgets);
        }
    }
}

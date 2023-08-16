using JobBoardPlatform.DAL.Models.Enums;
using JobBoardPlatform.PL.Controllers.Utils;
using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.MainTechnologyWidgets;
using System.Drawing;

namespace JobBoardPlatform.PL.ViewModels.Factories.MainTechnologyWidgets
{
    public class MainTechnologyWidgetsFactory : IViewModelAsyncFactory<List<MainTechnologyWidget>>
    {
        public Task<List<MainTechnologyWidget>> CreateAsync()
        {
            var labels = GetLabels();
            var backgroundColorsRange = GetBackgroundColorRanges();
            var icons = GetIconsUri();

            var enumValues = Enum.GetValues(typeof(MainTechnologyTypeEnum))
                .Cast<MainTechnologyTypeEnum>()
                .ToArray();
            var mainTechnologies = enumValues.Select(x => x.ToString()).ToList();
            mainTechnologies.Insert(0, "All");

            var technologyWidgets = new List<MainTechnologyWidget>();
            foreach (var mainTechnology in mainTechnologies)
            {
                var widget = new MainTechnologyWidget();
                widget.Label = labels[mainTechnology];
                widget.BackgroundColorFrom = backgroundColorsRange[mainTechnology].Item1;
                widget.BackgroundColorTo = backgroundColorsRange[mainTechnology].Item2;
                widget.IconUri = StaticFilesUtils.GetDefaultTechnologyWidgetUriIfEmpty(icons[mainTechnology]);
                widget.Value = mainTechnology;

                technologyWidgets.Add(widget);
            }

            return Task.FromResult(technologyWidgets);
        }

        private Dictionary<string, string> GetLabels()
        {
            return new Dictionary<string, string>()
            {
                { "All", "All" },
                { MainTechnologyTypeEnum.Programming.ToString(), "Programming" },
                { MainTechnologyTypeEnum.Audio.ToString(), "Audio" },
                { MainTechnologyTypeEnum.Graphics3D.ToString(), "Graphics 3D" },
                { MainTechnologyTypeEnum.LevelDesign.ToString(), "Level Design" },
                { MainTechnologyTypeEnum.Testing.ToString(), "Testing" },
                { MainTechnologyTypeEnum.Management.ToString(), "Management" },
                { MainTechnologyTypeEnum.Other.ToString(), "Other" },
            };
        }

        private Dictionary<string, (Color, Color)> GetBackgroundColorRanges()
        {
            var primaryBlue = Color.FromArgb(52, 89, 230);
            var test = GetColorWithOffset(primaryBlue);

            return new Dictionary<string, (Color, Color)>()
            {
                { "All", (primaryBlue, GetColorWithOffset(primaryBlue)) },
                { MainTechnologyTypeEnum.Programming.ToString(), (Color.DodgerBlue, GetColorWithOffset(Color.DodgerBlue)) },
                { MainTechnologyTypeEnum.Audio.ToString(), (Color.MediumSeaGreen, GetColorWithOffset(Color.MediumSeaGreen)) },
                { MainTechnologyTypeEnum.Graphics3D.ToString(), (Color.OrangeRed, GetColorWithOffset(Color.OrangeRed)) },
                { MainTechnologyTypeEnum.LevelDesign.ToString(), (Color.Crimson, GetColorWithOffset(Color.Crimson)) },
                { MainTechnologyTypeEnum.Testing.ToString(), (Color.Firebrick, GetColorWithOffset(Color.Firebrick)) },
                { MainTechnologyTypeEnum.Management.ToString(), (Color.MediumOrchid, GetColorWithOffset(Color.MediumOrchid)) },
                { MainTechnologyTypeEnum.Other.ToString(), (Color.BlueViolet, GetColorWithOffset(Color.BlueViolet)) },
            };
        }

        private Dictionary<string, string> GetIconsUri()
        {
            return new Dictionary<string, string>()
            {
                { "All", StaticFilesUtils.PathToTechnologyAllWidgetIcon },
                { MainTechnologyTypeEnum.Programming.ToString(), StaticFilesUtils.PathToTechnologyProgrammingWidgetIcon },
                { MainTechnologyTypeEnum.Audio.ToString(), StaticFilesUtils.PathToTechnologyAudioWidgetIcon },
                { MainTechnologyTypeEnum.Graphics3D.ToString(), StaticFilesUtils.PathToTechnologyGraphicsWidgetIcon },
                { MainTechnologyTypeEnum.LevelDesign.ToString(), StaticFilesUtils.PathToTechnologyLevelDesignWidgetIcon },
                { MainTechnologyTypeEnum.Testing.ToString(), StaticFilesUtils.PathToTechnologyTestingWidgetIcon },
                { MainTechnologyTypeEnum.Management.ToString(), StaticFilesUtils.PathToTechnologyManagementWidgetIcon },
                { MainTechnologyTypeEnum.Other.ToString(), StaticFilesUtils.PathToTechnologyOtherWidgetIcon },
            };
        }

        private Color GetColorWithOffset(Color color)
        {
            int r = Math.Clamp(color.R + 50, 0, 255);
            int g = Math.Clamp(color.G + 45, 0, 255);
            int b = Math.Clamp(color.B + 25, 0, 255);
            return Color.FromArgb(r, g, b);
        }
    }
}

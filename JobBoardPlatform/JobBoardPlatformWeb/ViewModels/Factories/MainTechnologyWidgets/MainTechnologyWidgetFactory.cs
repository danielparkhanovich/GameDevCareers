using JobBoardPlatform.DAL.Models.Enums;
using JobBoardPlatform.PL.Controllers.Utils;
using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.MainTechnologyWidgets;
using System.Drawing;

namespace JobBoardPlatform.PL.ViewModels.Factories.MainTechnologyWidgets
{
    public class MainTechnologyWidgetFactory : IViewModelFactory<string, MainTechnologyWidget>
    {
        private readonly Dictionary<string, string> labels = new Dictionary<string, string>()
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

        private readonly static Color primaryBlue = Color.FromArgb(52, 89, 230);

        private readonly Dictionary<string, (Color, Color)> backgroundColorRanges = new Dictionary<string, (Color, Color)>()
        {
            { "All", GetColorGradientRange(primaryBlue) },
            { MainTechnologyTypeEnum.Programming.ToString(), GetColorGradientRange(Color.DodgerBlue) },
            { MainTechnologyTypeEnum.Audio.ToString(), GetColorGradientRange(Color.MediumSeaGreen) },
            { MainTechnologyTypeEnum.Graphics3D.ToString(), GetColorGradientRange(Color.OrangeRed) },
            { MainTechnologyTypeEnum.LevelDesign.ToString(), GetColorGradientRange(Color.Crimson) },
            { MainTechnologyTypeEnum.Testing.ToString(), GetColorGradientRange(Color.Firebrick) },
            { MainTechnologyTypeEnum.Management.ToString(), GetColorGradientRange(Color.MediumOrchid) },
            { MainTechnologyTypeEnum.Other.ToString(), GetColorGradientRange(Color.BlueViolet) },
        };

        private readonly Dictionary<string, string> iconsUri = new Dictionary<string, string>()
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

        private static (Color, Color) GetColorGradientRange(Color color)
        {
            return (color, GetColorWithGradientOffset(color));
        }

        private static Color GetColorWithGradientOffset(Color color)
        {
            int r = Math.Clamp(color.R + 50, 0, 255);
            int g = Math.Clamp(color.G + 45, 0, 255);
            int b = Math.Clamp(color.B + 25, 0, 255);
            return Color.FromArgb(r, g, b);
        }

        public MainTechnologyWidget Create(string mainTechnology)
        {
            var widget = new MainTechnologyWidget();
            widget.Label = labels[mainTechnology];
            widget.BackgroundColorFrom = backgroundColorRanges[mainTechnology].Item1;
            widget.BackgroundColorTo = backgroundColorRanges[mainTechnology].Item2;
            widget.IconUri = StaticFilesUtils.GetDefaultTechnologyWidgetUriIfEmpty(iconsUri[mainTechnology]);
            widget.Value = mainTechnology;
            return widget;
        }
    }
}

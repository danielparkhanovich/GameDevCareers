namespace JobBoardPlatform.PL.ViewModels.Models.Templates
{
    public class ContainerHeaderViewModel
    {
        public string[] SortLabels { get; set; }
        public string[] SortValues { get; set; }
        public string[] FilterLabels { get; set; }
        public string[] FilterValues { get; set; }
        public bool IsInvertFilters { get; set; }
    }
}

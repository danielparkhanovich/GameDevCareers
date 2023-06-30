using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.PL.Controllers.Profile;
using JobBoardPlatform.PL.Controllers.RestorePassword;
using JobBoardPlatform.PL.Controllers.Security;
using JobBoardPlatform.PL.Controllers.Utils;
using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Profile;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Profile
{
    public class ProfileTabsViewModelFactory : IViewModelAsyncFactory<ProfileTabsViewModel>
    {
        private readonly string userRole;


        public ProfileTabsViewModelFactory(string userRole) 
        { 
            this.userRole = userRole;
        }

        public Task<ProfileTabsViewModel> CreateAsync()
        {
            if (userRole == UserRoles.Employee)
            {
                return Task.FromResult(GetEmployeeTabs());
            }
            else if (userRole == UserRoles.Company) 
            {
                return Task.FromResult(GetCompanyTabs());
            }
            else if (userRole == UserRoles.Admin)
            {
                return Task.FromResult(GetAdminTabs());
            }

            throw new Exception("Unexpected user role!");
        }

        private ProfileTabsViewModel GetEmployeeTabs()
        {
            string[] tabsLabels = new string[] { "Profile", "Settings" };
            string[] tabsActions = new string[] { "Profile", "Settings" };
            string[] tabsControllers = new string[] { "EmployeeProfile", "UserSettings" };

            var viewModel = new ProfileTabsViewModel()
            {
                MainTabsLabels = tabsLabels,
                MainTabsActions = tabsActions,
                MainTabsControllers = tabsControllers
            };

            return viewModel;
        }

        private ProfileTabsViewModel GetCompanyTabs()
        {
            string[] tabsLabels = new string[] { "Company profile", "Manage Ads", "Settings" };
            string[] tabsActions = new string[] { "Profile", "Offers", "Settings" };
            string[] tabsControllers = new string[] { "CompanyProfile", "CompanyOffersPanel", "UserSettings" };

            var viewModel = new ProfileTabsViewModel()
            {
                MainTabsLabels = tabsLabels,
                MainTabsActions = tabsActions,
                MainTabsControllers = tabsControllers
            };

            return viewModel;
        }

        private ProfileTabsViewModel GetAdminTabs()
        {
            string[] tabsLabels = new string[] { "Offers Panel", "Companies Panel", "Users Panel", "Settings" };
            string[] tabsActions = new string[] { "Panel", "Panel", "Panel", "Settings" };
            string[] tabsControllers = new string[] 
            { 
                ControllerUtils.GetControllerName(typeof(AdminPanelOffersController)),
                ControllerUtils.GetControllerName(typeof(AdminPanelCompaniesController)),
                ControllerUtils.GetControllerName(typeof(AdminPanelEmployeesController)),
                ControllerUtils.GetControllerName(typeof(UserSettingsController))
            };

            var viewModel = new ProfileTabsViewModel()
            {
                MainTabsLabels = tabsLabels,
                MainTabsActions = tabsActions,
                MainTabsControllers = tabsControllers
            };

            return viewModel;
        }
    }
}

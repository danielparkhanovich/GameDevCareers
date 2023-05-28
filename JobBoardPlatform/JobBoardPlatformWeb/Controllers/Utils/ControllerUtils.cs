using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Utils
{
    public static class ControllerUtils
    {
        public static string GetControllerName(Type controllerType)
        {
            string? name = TryGetNameFromRouteAttribute(controllerType);
            if (!string.IsNullOrEmpty(name))
            {
                return name;
            }

            return GetNameFromControllerType(controllerType);
        }

        private static string? TryGetNameFromRouteAttribute(Type controllerType)
        {
            var controllerAttribute = controllerType.GetCustomAttributes(typeof(RouteAttribute), inherit: true)
                .OfType<RouteAttribute>()
                .FirstOrDefault();
            return controllerAttribute?.Name;
        }

        private static string GetNameFromControllerType(Type controllerType)
        {
            string typeName = controllerType.Name;
            const string controllerSuffix = "Controller";
            if (typeName.EndsWith(controllerSuffix))
            {
                return typeName.Substring(0, typeName.Length - controllerSuffix.Length);
            }
            return typeName;
        }
    }
}

using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace JobBoardPlatform.PL.Interactors.Notifications
{
    public class NotificationsManager : INotificationsManager
    {
        public const string RegisterSection = "registerSection";
        public const string ResetPasswordSection = "restorePassword";
        public const string LoginSection = "loginSection";

        public static INotificationsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NotificationsManager();
                }
                return instance;
            }
        }

        private static INotificationsManager instance;

        private NotificationsManager()
        {
        }

        public void SetActionDoneNotification(string key, string value, ITempDataDictionary tempData)
        {
            tempData[key] = GetNotification(value, NotificationType.ActionDone);
        }

        public void SetErrorNotification(string key, string value, ITempDataDictionary tempData)
        {
            tempData[key] = GetNotification(value, NotificationType.Error);
        }

        public NotificationData? TryGetNotification(string key, ITempDataDictionary tempData)
        {
            if (!tempData.ContainsKey(key))
            {
                return null;
            }
            return tempData[key] as NotificationData;
        }

        private NotificationData GetNotification(string value, NotificationType type)
        {
            return new NotificationData()
            {
                Text = value,
                Type = type
            };
        }
    }
}

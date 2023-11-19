using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace JobBoardPlatform.PL.Interactors.Notifications
{
    public class NotificationsManager : INotificationsManager
    {
        public const string RegisterSection = "registerSection";
        public const string ResetPasswordSection = "restorePassword";
        public const string LoginSection = "loginSection";
        public const string PaymentSection = "paymentSection";
        public const string PostApplicationSection = "postApplicationSection";


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

        public void SetActionDoneEmailNotification(string key, string value, ITempDataDictionary tempData)
        {
            var notification = GetNotification(value, NotificationType.ActionDoneEmail);
            PutData(key, notification, tempData);
        }

        public void SetSuccessNotification(string key, string value, ITempDataDictionary tempData)
        {
            var notification = GetNotification(value, NotificationType.Success);
            PutData(key, notification, tempData);
        }

        public void SetErrorNotification(string key, string value, ITempDataDictionary tempData)
        {
            var notification = GetNotification(value, NotificationType.Error);
            PutData(key, notification, tempData);
        }

        public NotificationData? TryGetNotification(string key, ITempDataDictionary tempData)
        {
            return GetData<NotificationData>(key, tempData);
        }

        private NotificationData GetNotification(string value, NotificationType type)
        {
            return new NotificationData()
            {
                Text = value,
                Type = type
            };
        }

        private void PutData<T>(string key, T data, ITempDataDictionary tempData) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(data);
        }

        private T? GetData<T>(string key, ITempDataDictionary tempData) where T : class
        {
            object? data;
            tempData.TryGetValue(key, out data);
            return data == null ? null : JsonConvert.DeserializeObject<T>((string)data);
        }
    }
}

using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace JobBoardPlatform.PL.Interactors.Notifications
{
    public interface INotificationsManager
    {
        void SetActionDoneEmailNotification(string key, string value, ITempDataDictionary tempData);
        void SetSuccessNotification(string key, string value, ITempDataDictionary tempData);
        void SetErrorNotification(string key, string value, ITempDataDictionary tempData);
        NotificationData? TryGetNotification(string key, ITempDataDictionary tempData);
    }
}

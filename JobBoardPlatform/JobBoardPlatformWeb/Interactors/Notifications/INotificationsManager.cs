using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace JobBoardPlatform.PL.Interactors.Notifications
{
    public interface INotificationsManager
    {
        void SetActionDoneNotification(string key, string value, ITempDataDictionary tempData);
        void SetErrorNotification(string key, string value, ITempDataDictionary tempData);
        NotificationData? TryGetNotification(string key, ITempDataDictionary tempData);
    }
}

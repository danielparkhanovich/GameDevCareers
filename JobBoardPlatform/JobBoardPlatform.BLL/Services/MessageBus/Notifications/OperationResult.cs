namespace JobBoardPlatform.BLL.Services.MessageBus.Notifications
{
    public class OperationResult
    {
        public string Message = string.Empty;
        public OperationResultType ResultType { get; set; }
    }
}

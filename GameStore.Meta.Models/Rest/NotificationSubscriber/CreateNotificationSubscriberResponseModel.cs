namespace GameStore.Meta.Models.Rest.NotificationSubscriber
{
    public class CreateNotificationSubscriberResponseModel
    {
        public string Client { get; set; }
        public string ApiKey { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}

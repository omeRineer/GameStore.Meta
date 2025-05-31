namespace GameStore.Meta.Models.Message
{
    public class PushNotificationModel
    {
        public Guid Sender { get; set; }
        public string Type { get; set; }
        public string? ContentType { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public List<Guid>? TargetUsers { get; set; }
    }
}

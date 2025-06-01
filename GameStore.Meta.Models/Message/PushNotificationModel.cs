namespace GameStore.Meta.Models.Message
{
    public class PushNotificationModel
    {
        public string Type { get; set; }
        public string Sender { get; set; }
        public string Level { get; set; }
        public string Title { get; set; }
        public string ContentType { get; set; } = "text";
        public string? Content { get; set; }
        public List<string>? Targets { get; set; }

        public Dictionary<string, object>? Custom { get; set; }
    }
}

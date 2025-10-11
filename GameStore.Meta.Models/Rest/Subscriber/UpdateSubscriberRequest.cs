namespace GameStore.Meta.Models.Rest.Subscriber
{
    public class UpdateSubscriberRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string>? Topics { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}

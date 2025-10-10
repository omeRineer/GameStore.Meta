namespace GameStore.Meta.Models.Rest.Channel
{
    public class UpdateChannelRequest
    {
        public string Name { get; set; }
        public List<string>? Topics { get; set; }
    }
}

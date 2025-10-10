namespace GameStore.Meta.Models.Rest.Channel
{
    public class CreateChannelRequest
    {
        public string Name { get; set; }
        public List<string>? Topics { get; set; }
    }
}

namespace GameStore.Meta.Models.Rest.Client
{
    public class UpdateClientRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}

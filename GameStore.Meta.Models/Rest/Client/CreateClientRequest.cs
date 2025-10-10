namespace GameStore.Meta.Models.Rest.Client
{
    public class CreateClientRequest
    {
        public string Signature { get; set; }
        public string Name { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}

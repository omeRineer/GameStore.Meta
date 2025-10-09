using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Models.Rest
{
    public record SubscriberItem(Guid Id, Guid Client, string Key, string ApiKey, List<string>? Topics, DateTime? ExpireDate);

    public record CreateSubscriberRequest(Guid Client, string Key, DateTime? ExpireDate);

    public record CreateSubscriberResponse(Guid Client, string ApiKey, DateTime? ExpireDate);
    public record GetSubscriberResponse(Guid Id, Guid Client, string Key, string ApiKey, List<string>? Topics, DateTime? ExpireDate, DateTime CreateDate);
}

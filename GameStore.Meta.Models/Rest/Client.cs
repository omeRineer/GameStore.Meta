using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Models.Rest
{
    public record ClientItem(Guid Id, string Name, DateTime? ExpireDate);

    public record CreateClientRequest(string Signature, string Name, DateTime? ExpireDate);


    public record CreateClientResponse(string Signature, string Name, DateTime? ExpireDate, DateTime CreateDate);
    public record GetClientDetailResponse(Guid Id,
                                               string Signature,
                                               string Name,
                                               DateTime? ExpireDate,
                                               DateTime CreateDate);
}

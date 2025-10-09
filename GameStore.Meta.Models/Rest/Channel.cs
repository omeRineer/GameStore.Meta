using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Models.Rest
{
    public record CreateChannelRequest(string Name, List<string>? Topics);


    public record CreateChannelResponse(Guid id, string Name, List<string>? Topics);
}

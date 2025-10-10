using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Models.Rest.Channel
{
    public class ChannelResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<string>? Topics { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
    }
}

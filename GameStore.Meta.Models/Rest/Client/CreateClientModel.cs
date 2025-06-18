using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Models.Rest.Client
{
    public class CreateClientModel
    {
        public string Signature { get; set; }
        public string Name { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}

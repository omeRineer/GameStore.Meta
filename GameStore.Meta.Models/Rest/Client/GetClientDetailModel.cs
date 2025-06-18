using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Models.Rest.Client
{
    public class GetClientDetailModel
    {
        public Guid Id { get; set; }
        public string Signature { get; set; }
        public string Name { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

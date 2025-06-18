using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Models.Client
{
    public class CurrentUser
    {
        public Guid Client { get; set; }
        public string Key { get; set; }
        public string ApiKey { get; set; }
    }
}

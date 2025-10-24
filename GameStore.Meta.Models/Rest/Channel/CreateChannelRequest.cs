using System.Globalization;

namespace GameStore.Meta.Models.Rest.Channel
{
    public class CreateChannelRequest
    {
        public string Prefix 
        { 
            get => _Prefix; 
            set => _Prefix = Prefix.ToUpper(new CultureInfo("en-EN")); 
        }
        public string Name { get; set; }
        public List<string>? Topics { get; set; }



        private string _Prefix;
    }
}

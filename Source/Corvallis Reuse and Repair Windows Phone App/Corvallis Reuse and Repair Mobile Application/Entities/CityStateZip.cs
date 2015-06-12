using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corvallis_Reuse_and_Recycle_Mobile_Application.Entities
{
    public class CityStateZip
    {
        public CityStateZip(string json)
        {
            if (json != null)
            {
                JObject jObject = JObject.Parse(json);
                country = (string)jObject["country"];
                state = (string)jObject["state"];
                city = (string)jObject["city"];
            }
        }

        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
    }
}

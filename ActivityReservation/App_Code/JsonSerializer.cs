using Microsoft.Web.Redis;
using Newtonsoft.Json;
using System.Text;

namespace ActivityReservation
{
    public class JsonSerializer : ISerializer
    {
        private static JsonSerializerSettings _settings =
            new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };

        public byte[] Serialize(object data)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data, _settings));
        }

        public object Deserialize(byte[] data)
        {
            if (data == null)
            {
                return null;
            }
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), _settings);
        }
    }
}
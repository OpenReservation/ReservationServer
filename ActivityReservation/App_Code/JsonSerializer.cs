using System.Text;
using Microsoft.Web.Redis;
using Newtonsoft.Json;
using WeihanLi.Extensions;

namespace ActivityReservation
{
    public class JsonSerializer : ISerializer
    {
        public byte[] Serialize(object data)
        {
            return Encoding.UTF8.GetBytes(data.ToJson());
        }

        public object Deserialize(byte[] data)
        {
            if (data == null)
            {
                return null;
            }
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data));
        }
    }
}

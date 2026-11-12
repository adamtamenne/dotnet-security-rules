using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;

public class DataLoader
{
    public object FromJson(string json)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };
        return JsonConvert.DeserializeObject(json, settings);
    }

    public object FromBinary(Stream stream)
    {
        var formatter = new BinaryFormatter();
        return formatter.Deserialize(stream);
    }
}

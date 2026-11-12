using Newtonsoft.Json;
using System.Text.Json;

public class DataLoader
{
    public object FromJson(string json)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None
        };
        return JsonConvert.DeserializeObject(json, settings);
    }

    public MyData FromSystemTextJson(string json)
    {
        return System.Text.Json.JsonSerializer.Deserialize<MyData>(json);
    }
}

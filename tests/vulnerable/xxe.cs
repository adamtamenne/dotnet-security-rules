using System.Xml;

public class XmlParser
{
    public void Parse(string input)
    {
        var settings = new XmlReaderSettings
        {
            XmlResolver = new XmlUrlResolver(),
            DtdProcessing = DtdProcessing.Parse
        };
        using var reader = XmlReader.Create(input, settings);
    }
}

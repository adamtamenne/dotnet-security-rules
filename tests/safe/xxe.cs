using System.Xml;

public class XmlParser
{
    public void Parse(string input)
    {
        var settings = new XmlReaderSettings
        {
            XmlResolver = null,
            DtdProcessing = DtdProcessing.Prohibit
        };
        using var reader = XmlReader.Create(input, settings);
    }
}

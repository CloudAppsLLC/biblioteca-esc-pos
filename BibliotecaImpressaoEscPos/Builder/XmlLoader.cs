using System.IO;
using System.Text;
using System.Xml.XPath;

namespace BibliotecaImpressaoEscPos.Builder
{
    public class XmlLoader
    {
        public static XPathNodeIterator Load(string content)
        {
            var bytes = Encoding.ASCII.GetBytes($"<root>{content}</root>");
            var doc = new XPathDocument(new MemoryStream(bytes));
            var xml = doc.CreateNavigator();
            return xml.SelectSingleNode("root").SelectChildren(XPathNodeType.All);
        }
    }
}

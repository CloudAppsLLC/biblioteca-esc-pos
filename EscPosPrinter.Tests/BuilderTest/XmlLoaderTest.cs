using EscPosPrinter.Builder;
using System.Xml.XPath;
using Xunit;
using Xunit.Extensions;

namespace EscPosPrinter.Tests.BuilderTest
{
    public class XmlLoaderTest
    {
        [Theory(DisplayName = "Carga de xml em objeto de navegação!")]
        [InlineData(
            @"
                <ce>centralizado</ce>
                <sl>30</sl>
                <l></l>
                normal
                <sl>30</sl>
                <l/>
                <ad>
                    a direita
                    <sl>30</sl>
                    <b>negrito e a direita</b>
                </ad>
                <sl>30</sl>
                <b>apenas negrito</b>
                <sl>30</sl>
                <c>condensado</c>
                <sl>30</sl>
                <ad>normal a direita</ad>
                <sl>30</sl>
                <sl>30</sl>
                <sl>30</sl>
                <gui/>
             ", 17, "ce", 8, "sl", "ad/sl", "negrito e a direita", "ad/b"
        )]
        public void LoadTest(string xml, int equalExpected, string firstNodeNotNull, int equalExpected_b, string equalNodesAtuals_b, string secondNodeNotNull, string textExpected_c, string equalNodeAtual_c)
        {
            var result = XmlLoader.Load(xml);

            Assert.Equal(equalExpected, result.Current.SelectChildren(XPathNodeType.All).Count);
            Assert.NotNull(result.Current.SelectSingleNode(firstNodeNotNull));
            Assert.Equal(equalExpected_b, result.Current.Select(equalNodesAtuals_b).Count);

            Assert.NotNull(result.Current.SelectSingleNode(secondNodeNotNull));
            Assert.Equal(textExpected_c, result.Current.SelectSingleNode(equalNodeAtual_c).Value);
        }
    }
}

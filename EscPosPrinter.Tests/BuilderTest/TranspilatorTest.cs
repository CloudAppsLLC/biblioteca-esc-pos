using EscPosPrinter.Builder;
using System.Collections.Generic;
using System.Xml.XPath;
using Xunit;
using Xunit.Extensions;

namespace EscPosPrinter.Tests.BuilderTest
{
    public class TranspilatorTest
    {
        [Theory(DisplayName = "Transpilador de tags para array de Command!")]
        [InlineData(
            @"
               <ce>centralizado</ce>
               <l></l>
               normal
               <ad>
                   a direita
                   <sl>30</sl>
                   <b>negrito e a direita</b>
               </ad>
               <b>apenas negrito</b>
               <c>condensado</c>
               <ad>normal a direita</ad>
               <sl>30</sl>
               <gui/>
             "
        )]
        public void TranspileElementsTest(string xml)
        {
            XPathNodeIterator nodes = XmlLoader.Load(xml);

            var commands = Transpilator.TranspileElements(nodes);
            
            Assert.IsType<List<Command>>(commands);
            Assert.NotNull(commands);
            Assert.True(commands.Count > 0);
        }
    }
}

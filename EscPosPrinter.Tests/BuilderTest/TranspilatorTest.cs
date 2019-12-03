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
        [InlineData(
            @"
	            02
	            <c>
		            a
		            <ce>
			            <b>NÃO-FISCAL</b>
		            </ce>
		            <l></l>
		            Loja: 0052 - FOR57-DesMoreira2-CE
		            <l></l>
		            De :02/12/2019 a 02/12/2019
		            <l></l>
		            Operador: 3071   - FRANCISCO ANTONIO DOS SANTOS  
		            <l></l>
		            Finalizador    Qtd.Docs    Valor Liquido
		            <l></l>
		            -----------    ---------  -------------
		            <l></l>
		            DINHEIRO            -         45,44
		            <l></l>
		            -----------    ---------  -------------
		            <l></l>Sub-Tot.(1)         0         45,44       
		            <l></l>
		            -----------    ---------  -------------
		            <l></l>
		            Faturamento Ecf:              45,44
		            <l></l>
		            -----------    ---------  -------------
		            <l></l>
		            --------------------------------------------------FIM
		            <l></l>
	            </c>
	            <c>
		            <l></l>
		            <l></l>
	            </c>
	            __________________________________________
	            <l></l>
	            CAIXA: 084  LOJA: 52
	            <l></l>
	            <sl>1</sl>
	            <gui></gui>
	            <l></l>
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

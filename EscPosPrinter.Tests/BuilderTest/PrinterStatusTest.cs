using Xunit;
using Xunit.Extensions;

namespace EscPosPrinter.Tests.BuilderTest
{

    public class PrinterStatusTest
    {
        [Theory(DisplayName = "Deve exibir que a gaveta está aberta!")]
        [InlineData((byte)0x04)]
        public void DeveExibirGavetaAberta(byte valor)
        {
            PrinterStatus test = new PrinterStatus();
            test.SetEstadoImpressora(valor);
            Assert.True(test.SinalGaveta);
        }
        [Theory(DisplayName = "Deve exibir que a gaveta está fehcada!")]
        [InlineData((byte)0x00)]
        public void DeveExibirGavetaFechado(byte valor)
        {
            PrinterStatus test = new PrinterStatus();
            test.SetEstadoImpressora(valor);
            Assert.False(test.SinalGaveta);
        }

        [Theory(DisplayName = "Deve exibir que a tampa está aberta!")]
        [InlineData((byte)0x04)]
        public void DeveExibirTampaAberta(byte valor)
        {
            PrinterStatus test = new PrinterStatus();
            test.SetIndicadorDesligamento(valor);
            Assert.True(test.TampaAberta);
        }

        [Theory(DisplayName = "Deve exibir que a tampa está fechada!")]
        [InlineData((byte)0x00)]
        public void DeveExibirTampaFechada(byte valor)
        {
            PrinterStatus test = new PrinterStatus();
            test.SetIndicadorDesligamento(valor);
            Assert.False(test.TampaAberta);
        }

        [Theory(DisplayName = "Deve exibir que a impressora está sem papel!")]
        [InlineData((byte)0x60)]
        public void DeveExibirSemPapel(byte valor)
        {
            PrinterStatus test = new PrinterStatus();
            test.SetSensorPapel(valor);
            Assert.True(test.SemPapel);
        }

        [Theory(DisplayName = "Deve exibir que a impressora está com papel!")]
        [InlineData((byte)0x00)]
        public void DeveExibirComPapel(byte valor)
        {
            PrinterStatus test = new PrinterStatus();
            test.SetSensorPapel(valor);
            Assert.False(test.SemPapel);
        }

        [Theory(DisplayName = "Deve exibir que a impressora está com pouco papel!")]
        [InlineData((byte)0x0C)]
        public void DeveExibirComPoucoPapel(byte valor)
        {
            PrinterStatus test = new PrinterStatus();
            test.SetSensorPapel(valor);
            Assert.True(test.PoucoPapel);
        }

        [Theory(DisplayName = "Deve exibir que a impressora está com papel (Sensor de pouco papel)!")]
        [InlineData((byte)0x00)]
        public void DeveExibirComPapelPeloSensorPoucoPapel(byte valor)
        {
            PrinterStatus test = new PrinterStatus();
            test.SetSensorPapel(valor);
            Assert.False(test.PoucoPapel);
        }

        [Theory(DisplayName = "Deve exibir que a impressora está em erro!")]
        [InlineData((byte)0x40)]
        public void DeveExibirEmErro(byte valor)
        {
            PrinterStatus test = new PrinterStatus();
            test.SetIndicadorErro(valor);
            Assert.True(test.EmErro);
        }


        [Theory(DisplayName = "Deve exibir que a impressora não está em erro!")]
        [InlineData((byte)0x00)]
        public void DeveExibirEmNaoErro(byte valor)
        {
            PrinterStatus test = new PrinterStatus();
            test.SetIndicadorErro(valor);
            Assert.False(test.EmErro);
        }

    }
}

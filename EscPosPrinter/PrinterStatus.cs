using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EscPosPrinter
{
    public class PrinterStatus
    {
        public bool SinalGaveta { get; set; }        
        public bool TampaAberta { get; set; }
        public bool SemPapel { get; set; }
        public bool PoucoPapel { get; set; }
        public bool EmErro { get; set; }

        public void SetEstadoImpressora(byte valor)
        {
            SinalGaveta = (valor & (1 << 2)) != 0;
        }

        public void SetIndicadorDesligamento(byte valor)
        {
            TampaAberta = (valor & (1 << 2)) != 0;
        }

        public void SetSensorPapel(byte valor)
        {
            SemPapel = (valor & (1 << 6)) != 0;
            PoucoPapel = (valor & (1 << 3)) != 0;
        }

        public void SetIndicadorErro(byte valor)
        {
            EmErro = (valor & (1 << 6)) != 0;
        }
    }
}

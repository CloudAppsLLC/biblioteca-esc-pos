using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BibliotecaImpressaoEscPos.Enums
{
    public enum BarcodeType
    {
        UPC_A = 0,
        UPC_E = 1,
        JAN13_EAN13 = 2,
        JAN8_EAN8 = 3,
        CODE39 = 4,
        ITF = 5,
        CODEBAR = 6,
        CODE93 = 72,
        CODE128 = 73
    }
}

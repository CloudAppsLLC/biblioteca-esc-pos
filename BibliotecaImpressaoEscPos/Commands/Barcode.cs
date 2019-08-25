using System;
using BibliotecaImpressaoEscPos.Enums;

namespace BibliotecaImpressaoEscPos.Commands
{
    public class Barcode
    {
        public static int[] Condensed = { 27, 33, 1 };
        public static int[] SelectPrintPositionReadingHumanCharacter(PositionReadingHumanCharacter positionReadingHumanCharacter) => new int[] { 29, 72, (int)positionReadingHumanCharacter };
        public static int[] Height(int height) => new int[] { 29, 104, height };
        public static int[] Width(int width) => new int[] { 29, 119, width };
        public static int[] Config(int contentLength, BarcodeType barcodeType = BarcodeType.CODE128) => new int[] { 29, 107, (int)barcodeType, (contentLength / 2) + 2, 123, 67 };
        public static int[] LeftPosition(int value) => new int[] { 29, 120, value };
    }
}

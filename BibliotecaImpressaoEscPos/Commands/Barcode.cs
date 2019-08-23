using BibliotecaImpressaoEscPos.Enums;

namespace BibliotecaImpressaoEscPos.Commands
{
    public class Barcode
    {
        public static int[] SelectPrintPositionReadingHumanCharacter(PositionReadingHumanCharacter positionReadingHumanCharacter) => new int[] { 29, 72, (int)positionReadingHumanCharacter };
    }
}

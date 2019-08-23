using BibliotecaImpressaoEscPos.Enums;

namespace BibliotecaImpressaoEscPos.Commands
{
    public class Text
    {
        public static int[] Initialize = { 27, 64 };
        public static int[] NewLine = { 10 };
        public static int[] Guillotine = { 29, 86, 66, 0 };
        public static int[] Normal = { 27, 69, 0 };
        public static int[] Bold = { 27, 69, 1 };

        public static int[] TextAlign(Align align) => new int[] { 27, 97, (int)align };
        public static int[] LineSpacing(int n) => new int[] { 27, 51, n };
        public static int[] LetterSpacing(int n) => new int[] { 27, 32, n };
    }
}

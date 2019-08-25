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
        public static int[] LineSpacing(int value) => new int[] { 27, 51, value };
        public static int[] LetterSpacing(int value) => new int[] { 27, 32, value };
        public static int[] MarginLeft(int value) => new int[] { 29, 76, value, 0 };
    }
}

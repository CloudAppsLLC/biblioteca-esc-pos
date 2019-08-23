using BibliotecaImpressaoEscPos.Enums;

namespace BibliotecaImpressaoEscPos.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var printer = new EscPos(4);
            printer.OpenPort();

            printer.SetLetterSpacing(10);
            printer.SetLineSpacing(25);
            printer.WriteTextLine("Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC.");
            printer.SetLineSpacing(0);
            printer.SetLetterSpacing(0);

            printer.WriteText("\n \n \n");

            printer.SetTextAlign(Align.CENTER);
            printer.WriteTextLine("obscure");
            printer.WriteTextLine("the undoubtable");
            printer.WriteTextLine("by Cicero, written in 45 BC.");
            printer.SetTextAlign(Align.LEFT);

            printer.WriteText("\n \n \n");

            printer.ShowBold();
            printer.WriteTextLine("'Lorem ipsum dolor sit amet..', comes from a line in section 1.10.32. The standard chunk of Lorem Ipsum used since the 1500s is reproduced below ");
            printer.ShowNormal();

            printer.ShowGuilotine();

            printer.ClosePort();
        }
    }
}

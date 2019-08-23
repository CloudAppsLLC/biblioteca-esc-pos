using BibliotecaImpressaoEscPos.Enums;
using System;
using System.IO.Ports;
using static BibliotecaImpressaoEscPos.Commands.Text;

namespace BibliotecaImpressaoEscPos
{
    public class EscPos : IDisposable
    {
        private SerialPort Port;

        public EscPos(int portCOM)
        {
            Port = new SerialPort($"COM{portCOM.ToString().Trim()}");
        }

        public void OpenPort()
        {
            try
            {
                Port.Open();
                SendCommand(Initialize);
            }
            catch (UnauthorizedAccessException)
            {
                OpenPort();
            }
        }

        public void SetLineSpacing(int n)
        {
            SendCommand(LineSpacing(n));
        }

        public void SetLetterSpacing(int n)
        {
            SendCommand(LetterSpacing(n));
        }

        public void ClosePort()
        {
            Port.Close();
        }

        public void SendCommand(int[] command)
        {
            SendCommand(IntArrayToStringCmd(command));
        }

        public void SetTextAlign(Align align)
        {
            SendCommand(TextAlign(align));
        }

        public void SendCommand(string command)
        {
            Port.Write(command);
        }

        private string IntArrayToStringCmd(int[] command)
        {
            string @return = string.Empty;

            for (int i = 0; i < command.Length; i++)
            {
                @return += Convert.ToChar(command[i]).ToString();
            }
            Console.WriteLine(@return);

            return @return;
        }

        public void WriteText(string text)
        {
            Port.Write(text);
        }

        public void ShowBold()
        {
            SendCommand(Bold);
        }

        public void WriteTextLine(string text)
        {
            Port.Write(text);
            Port.Write("\n ");
        }

        public void Dispose()
        {
            Port.Close();
        }

        public void ShowNormal()
        {
            SendCommand(Normal);
        }

        public void ShowGuilotine()
        {
            SendCommand(Guillotine);
        }
    }
}

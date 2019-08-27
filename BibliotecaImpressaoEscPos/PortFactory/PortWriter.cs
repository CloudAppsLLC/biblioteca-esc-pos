using System;
using System.IO;
using System.IO.Ports;
using System.Text;

namespace EscPosPrinter.PortFactory
{
    public class PortWriter : IDisposable
    {
        private SerialPort PortCOM;

        public bool Initialized { get; private set; }
        public Exception InternalException { get; private set; }
        public string InternalExceptionCustomMessage { get; private set; }

        public string GetPortName() => PortCOM.PortName;

        public void Write(byte[] buffer, int offset, int count) => PortCOM.Write(buffer, offset, count);

        private void ErrorTreatment(Exception ex, string text)
        {
            var ErrorMessage = $"{text} ({ex.Message})";
            Console.WriteLine(ErrorMessage);
            Initialized = false;
            InternalException = ex;
            InternalExceptionCustomMessage = text;
        }

        public PortWriter(string serialPort)
        {
            SerialPort printerPort = new SerialPort(serialPort, 9600);

            if (printerPort != null)
            {
                System.Console.WriteLine($"Porta {serialPort}: OK!");
                if (printerPort.IsOpen)
                {
                    printerPort.Close();
                }
            }

            Console.WriteLine($"Abrindo a porta {serialPort}...");

            try
            {
                printerPort.Open();
                Initialized = true;
            }
            catch (InvalidOperationException ioex)
            {
                ErrorTreatment(ioex, "Erro de operação inválida!");
            }
            catch (ArgumentOutOfRangeException aorex)
            {
                ErrorTreatment(aorex, "Erro de argumentos fora do intervalo!");
            }
            catch (ArgumentException aex)
            {
                ErrorTreatment(aex, "Erro de argumento!");
            }
            catch (IOException ioex)
            {
                ErrorTreatment(ioex, "Erro de I/O!");
            }
            catch (UnauthorizedAccessException uaex)
            {
                ErrorTreatment(uaex, "Erro de acesso não autorizado!");
            }

            PortCOM = printerPort;
        }

        public void WriteByte(byte valueToWrite)
        {
            byte[] tempArray = { valueToWrite };
            PortCOM.Write(tempArray, 0, 1);
        }

        public void WriteToBuffer(string text, string localEncoding)
        {
            text = text.Trim('\n').Trim('\r');

            byte[] originalBytes = Encoding.UTF8.GetBytes(text);
            byte[] outputBytes = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(localEncoding), originalBytes);

            PortCOM.Write(outputBytes, 0, outputBytes.Length);
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

        public void WriteIntegers(params int[] vTWs)
        {
            PortCOM.Write(IntArrayToStringCmd(vTWs));
        }

        public void Dispose()
        {
            PortCOM.Close();
        }
    }
}

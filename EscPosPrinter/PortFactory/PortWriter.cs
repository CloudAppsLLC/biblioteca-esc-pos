using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;

namespace EscPosPrinter.PortFactory
{
    public enum PortStatus
    {
        Unauthorized,
        PortNotFound,
        OK

    }
    public class PortWriter : IDisposable
    {
        private SerialPort PortCOM;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode, IntPtr securityAttrs, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

        public bool Initialized { get; private set; }
        public Exception InternalException { get; private set; }
        public string InternalExceptionCustomMessage { get; private set; }

        public string GetPortName() => PortCOM.PortName;

        public void Write(byte[] buffer, int offset, int count) => PortCOM.Write(buffer, offset, count);

        private void ErrorTreatment(Exception ex, string text)
        {
            var ErrorMessage = $"{text} ({ex.Message})";
            Initialized = false;
            InternalException = ex;
            InternalExceptionCustomMessage = text;
        }

        private PortStatus CheckPortStatus(string portName )
        {
            int tstInt;

            if (!portName.ToLower().Contains("com") && int.TryParse(portName, out tstInt) )
            {
                portName = "COM" + tstInt.ToString();
            }
            int dwFlagsAndAttributes = 0x40000000;
            
            if (Environment.OSVersion.Platform == PlatformID.Win32Windows)
            {
                dwFlagsAndAttributes = 0x80;                
            }
          
            SafeFileHandle hFile = CreateFile(@"\\.\" + portName, -1073741824, 0, IntPtr.Zero, 3, dwFlagsAndAttributes, IntPtr.Zero);
            if (hFile.IsInvalid)
            {
                int error = Marshal.GetLastWin32Error();
                hFile.Close();
                if ( error == 5) // Unauthorized error
                {                    
                    return PortStatus.Unauthorized;
                }

                return PortStatus.PortNotFound;
                
                
            }
            hFile.Close();
            return PortStatus.OK;
        }

        public PortWriter(string serialPort)
        {
            SerialPort printerPort = new SerialPort(serialPort, 9600);                     

            if (printerPort != null)
            {
                if (printerPort.IsOpen)
                {
                    printerPort.Close();
                    System.Threading.Thread.Sleep(200);
                }
            }

            try
            {
                while (true)
                {
                    if ( CheckPortStatus(serialPort) != PortStatus.Unauthorized)
                    {
                        printerPort.Open();
                        Initialized = true;
                        break;
                    }
                }                
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
            if ( text == null)
            {
                return;
            }
            text = text.Trim('\n').Trim('\r');
            
            byte[] originalBytes =  Encoding.GetEncoding(850).GetBytes(text);
            
            foreach(var c in originalBytes)
            {
                WriteByte((byte)c);
            }
            return;
            
        }

        public byte[] GetBytes(string text, string localEncoding)
        {
            byte[] originalBytes = Encoding.UTF8.GetBytes(text);
            byte[] outputBytes = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(localEncoding), originalBytes);

            return outputBytes;
        }

        private string IntArrayToStringCmd(int[] command)
        {
            string @return = string.Empty;

            for (int i = 0; i < command.Length; i++)
            {
                @return += Convert.ToChar(command[i]).ToString();
            }

            return @return;
        }

        public void WriteIntegers(params int[] vTWs)
        {
            PortCOM.Write(IntArrayToStringCmd(vTWs));
        }

        public int ReadChar()
        {
            int oldTimeout = PortCOM.ReadTimeout;            
            
            try
            {
                PortCOM.ReadTimeout = 10;
                return PortCOM.ReadChar();
            }
            catch( TimeoutException)
            {
                return -1;
            }
            finally
            {
                PortCOM.ReadTimeout = oldTimeout;
            }
        }

        public void Dispose()
        {
            PortCOM.Close();
            System.Threading.Thread.Sleep(200);
        }
    }
}

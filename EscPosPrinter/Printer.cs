using EscPosPrinter.PortFactory;
using EscPosPrinter.PortFactory.Enums;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Collections;
using System.Drawing.Imaging;

namespace EscPosPrinter
{
    public enum StatusType
    {
        EstadoImpressora = 1,
        IndicadorDesligamento = 2,
        IndicadorErro = 3,
        SensorPapel = 4
    }

    public class Printer : PortWriter, IPrinter
    {
        private const byte ESC = 27;
        private const byte GS = 29;
        private const byte DLE = 16;
        private const byte EOT = 4;
        private const byte FF = 12;
        private const byte DC4 = 20;

        private byte MaxPrinting = 7;
        private byte HeatingTime = 80;
        private byte HeatingInterval = 2;
        private string namePrinter = "";

        private Dictionary<char, byte> mapSpecialCharacter;

        public Dictionary<char, byte> MapSpecialCharacter
        {
            get
            {
                if (mapSpecialCharacter == null)
                {
                    mapSpecialCharacter = new Dictionary<char, byte>()
                    {
                        {'Ç',128 },
                        {'ç', 135 },
                        {'º', 167 },
                        {'Ô', 147 },
                        {'ô', 147 },
                        {'Õ', 79 },
                        {'õ', 111} ,
                        {'|', 124 },
                };
                }
                return mapSpecialCharacter;
            }
            set
            {
                mapSpecialCharacter = value;
            }
        }

        public int PictureLineSleepTimeMs { get; set; } = 40;
        public int WriteLineSleepTimeMs { get; set; } = 0;

        public string LocalEncoding { get; set; }

        public Printer(int serialPort) : base($"COM{serialPort}")
        {
            Constructor(MaxPrinting, HeatingTime, HeatingInterval);
        }

        public Printer(int serialPort, byte maxPrinting, byte heatingTime, byte heatingInterval) : base($"COM{serialPort}")
        {
            Constructor(maxPrinting, heatingTime, heatingInterval);
        }

        public Printer(int serialPort, int timeout) : base($"COM{serialPort}")
        {
            Constructor(MaxPrinting, HeatingTime, HeatingInterval, timeout);
        }

        public Printer(int serialPort, byte maxPrinting, byte heatingTime, byte heatingInterval, int timeout) : base($"COM{serialPort}")
        {
            Constructor(maxPrinting, heatingTime, heatingInterval, timeout);
        }

        private void Constructor(byte maxPrinting, byte heatingTime, byte heatingInterval, int timeout = 0)
        {
            Action initialize = () =>
            {
                try
                {
                    if (base.Initialized)
                    {
                        LocalEncoding = "ibm850";

                        MaxPrinting = maxPrinting;
                        HeatingTime = heatingTime;
                        HeatingInterval = heatingInterval;

                        Reset();

                        SetPrintingParameters(maxPrinting, heatingTime, heatingInterval);
                        SendEncoding(LocalEncoding);
                    }
                    else
                    {
                        throw base.InternalException;
                    }
                }
                catch (Exception ex)
                {
                    if (timeout > 0) Constructor(maxPrinting, heatingTime, heatingInterval, timeout);
                    else throw ex;
                }
            };

            if (timeout > 0)
            {
                Builder.Timedout.CallWithTimeout(initialize, timeout);
            }
            else
            {
                initialize.Invoke();
            }
        }
        public void WriteLine(string text)
        {
            WriteToBuffer(text, LocalEncoding);
            WriteByte(10);
            //for (var i = 0; i < text.Length; i++)
            //{
            //    if (!MapSpecialCharacter.ContainsKey(text[i]))
            //    {
            //        WriteToBuffer(text[i].ToString());
            //        continue;
            //    }
            //    WriteByte(MapSpecialCharacter[text[i]]);

            //}
            //WriteToBuffer(text, LocalEncoding);
           

            Thread.Sleep(WriteLineSleepTimeMs);
        }        

        public void SetInversionOn()
        {
            WriteByte(GS);
            WriteByte(66);
            WriteByte(1);
        }

        public void SetInversionOff()
        {
            WriteByte(GS);
            WriteByte(66);
            WriteByte(0);
        }

        public void WriteLineInvert(string text)
        {
            SetInversionOn();

            WriteLine(text);

            SetInversionOff();

            LineFeed();
        }

        public void SetBigOn()
        {
            const byte DoubleHeight = 1 << 4;
            const byte DoubleWidth = 1 << 5;
            const byte Bold = 1 << 3;

            WriteByte(ESC);
            WriteByte(33);
            WriteByte(DoubleHeight + DoubleWidth + Bold);
        }

        public void SetBifOff()
        {
            WriteByte(ESC);
            WriteByte(33);
            WriteByte(0);
        }

        public void WriteLineBig(string text)
        {
            SetBigOn();

            WriteLine(text);

            SetBifOff();
        }

        public void WriteLine(string text, PrintingStyle style)
        {
            WriteLine(text, (byte)style);
        }

        public void SetStyleOn(byte style, ref byte underlineHeight)
        {
            if (BitTest(style, 0))
            {
                style = BitClear(style, 0);
                underlineHeight = 1;
            }

            if (BitTest(style, 7))
            {
                style = BitClear(style, 7);
                underlineHeight = 2;
            }

            if (underlineHeight != 0)
            {
                WriteByte(ESC);
                WriteByte(45);
                WriteByte(underlineHeight);
            }

            WriteByte(ESC);
            WriteByte(33);
            WriteByte(style);
        }

        public void SetStyleOff(ref byte underlineHeight)
        {
            if (underlineHeight != 0)
            {
                WriteByte(ESC);
                WriteByte(45);
                WriteByte(0);
            }
            WriteByte(ESC);
            WriteByte(33);
            WriteByte(0);
        }

        public void WriteLine(string text, byte style)
        {
            byte underlineHeight = 0;

            SetStyleOn(style, ref underlineHeight);

            WriteLine(text);

            SetStyleOff(ref underlineHeight);
        }

        public void BoldOn()
        {
            WriteByte(ESC);
            WriteByte(32);
            WriteByte(1);
            WriteByte(ESC);
            WriteByte(69);
            WriteByte(1);
        }

        public void BoldOff()
        {
            WriteByte(ESC);
            WriteByte(32);
            WriteByte(0);
            WriteByte(ESC);
            WriteByte(69);
            WriteByte(0);
        }

        public void WriteLineBold(string text)
        {
            BoldOn();
            WriteLine(text);
            BoldOff();
        }

        public void WhiteOnBlackOn()
        {
            WriteByte(GS);
            WriteByte(66);
            WriteByte(1);
        }

        public void WhiteOnBlackOff()
        {
            WriteByte(GS);
            WriteByte(66);
            WriteByte(0);
        }

        public void SetSize(bool doubleWidth, bool doubleHeight)
        {
            int sizeValue = (Convert.ToInt32(doubleWidth)) * (0xF0) + (Convert.ToInt32(doubleHeight)) * (0x0F);
            WriteByte(GS);
            WriteByte(33);
            WriteByte((byte)sizeValue);
        }

        public void LineFeed()
        {
            WriteByte(10);
        }

        public void LineFeed(byte lines)
        {
            WriteByte(ESC);
            WriteByte(100);
            WriteByte(lines);
        }

        public void Guillotine()
        {
            WriteIntegers(29, 86, 66, 0);
        }

        public void Indent(byte columns)
        {
            if (columns < 0 || columns > 31)
            {
                columns = 0;
            }

            WriteByte(ESC);
            WriteByte(66);
            WriteByte(columns);
        }

        public void SetLineSpacing(byte lineSpacing)
        {
            WriteByte(ESC);
            WriteByte(51);
            WriteByte(lineSpacing);
        }

        public void SetAlignLeft()
        {
            WriteByte(ESC);
            WriteByte(97);
            WriteByte(0);
        }

        public void SetAlignCenter()
        {
            WriteByte(ESC);
            WriteByte(97);
            WriteByte(1);
        }

        public void SetAlignRight()
        {
            WriteByte(ESC);
            WriteByte(97);
            WriteByte(2);
        }

        public void HorizontalLine(string nameprinter)
        {
            //Elgin length = 64
            //Sweda length = 56
            int length = 56;
            namePrinter = nameprinter;
            if (nameprinter == "elgin")
            {
                length = 61;
            }

            for (int i = 0; i < length; i++)
            {
                WriteByte(0xC4);
            }
            WriteByte(10);

        }       

        public void Reset()
        {
            WriteByte(ESC);
            WriteByte(64);
            Thread.Sleep(50);

            SendEncoding(LocalEncoding);
        }

        public void PrintBarcode(BarcodeType type, string data)
        {
            byte[] originalBytes;
            byte[] outputBytes;

            if (type == BarcodeType.code93 || type == BarcodeType.code128)
            {
                originalBytes = Encoding.UTF8.GetBytes(data);
                outputBytes = originalBytes;
            }
            else
            {
                originalBytes = Encoding.UTF8.GetBytes(data.ToUpper());
                outputBytes = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(LocalEncoding), originalBytes);
            }

            switch (type)
            {
                case BarcodeType.upc_a:
                    if (data.Length == 11 || data.Length == 12)
                    {
                        WriteByte(GS);
                        WriteByte(107);
                        WriteByte(0);
                        Write(outputBytes, 0, data.Length);
                        WriteByte(0);
                    }
                    break;
                case BarcodeType.upc_e:
                    if (data.Length == 11 || data.Length == 12)
                    {
                        WriteByte(GS);
                        WriteByte(107);
                        WriteByte(1);
                        Write(outputBytes, 0, data.Length);
                        WriteByte(0);
                    }
                    break;
                case BarcodeType.ean13:
                    WriteByte(GS);
                    WriteByte(107);
                    WriteByte(2);
                    Write(outputBytes, 0, data.Length);
                    WriteByte(0);
                    break;
                case BarcodeType.ean8:

                    WriteByte(GS);
                    WriteByte(107);
                    WriteByte(3);
                    Write(outputBytes, 0, data.Length);
                    WriteByte(0);

                    break;
                case BarcodeType.code39:

                    WriteByte(GS);
                    WriteByte(107);
                    WriteByte(4);
                    Write(outputBytes, 0, data.Length);
                    WriteByte(0);

                    break;
                case BarcodeType.i25:

                    WriteByte(GS);
                    WriteByte(107);
                    WriteByte(5);
                    Write(outputBytes, 0, data.Length);
                    WriteByte(0);

                    break;
                case BarcodeType.codebar:

                    WriteByte(GS);
                    WriteByte(107);
                    WriteByte(6);
                    Write(outputBytes, 0, data.Length);
                    WriteByte(0);

                    break;
                case BarcodeType.code93: //todo: overload PrintBarcode method with a byte array parameter
                    if (data.Length > 1)
                    {
                        WriteByte(GS);
                        WriteByte(107);
                        WriteByte(7); //todo: use format 2 (init string : 29,107,72) (0x00 can be a value, too)
                        Write(outputBytes, 0, data.Length);
                        WriteByte(0);
                    }
                    break;
                //FUNCIONANDO PARA ELGIN I9
                case BarcodeType.code128: //todo: overload PrintBarcode method with a byte array parameter
                    if (data.Length > 1)
                    {
                        WriteByte(GS);
                        WriteByte(107);

                        WriteByte(73); //todo: use format 2 (init string : 29,107,73) (0x00 can be a value, too
                        WriteByte((byte)(originalBytes.Length + 2)); //length

                        WriteByte(123); // {
                        WriteByte(65); // A

                        Write(originalBytes, 0, originalBytes.Length);
                        WriteByte(0);
                    }
                    break;
                case BarcodeType.code128C:
                    if ( data.Length > 1 && data.Length % 2 == 0)
                    {
                        // O padrão C imprime pares de números em um único byte.
                        // Largura
                        WriteByte(GS);
                        WriteByte((byte)'w');
                        WriteByte(2);

                        // Altura
                        WriteByte(GS);
                        WriteByte((byte)'h');
                        WriteByte(50);

                        // Não imprime código
                        WriteByte(GS);
                        WriteByte((byte)'H');
                        WriteByte(0);
                        
                        var length = originalBytes.Length;
                        if (originalBytes.Length % 2 > 0)
                        {
                            length++;
                        }

                        var maxLength = length / 2;
                        var counter = 0;
                        int sum = 0;
                        List<byte> bytes = new List<byte>();
                        var strNum = "";

                        for (int i = 0; i < maxLength; i++)
                        {
                            strNum = "";
                            sum = 0;
                            for (int j = 0; j < 2; j++)
                            {
                                strNum += data[counter++];
                            }
                            sum = int.Parse(strNum);
                            bytes.Add((byte)sum);
                        }


                        WriteByte(GS);
                        WriteByte((byte)'k');

                        WriteByte(73);                         
                        WriteByte((byte)(bytes.Count)); //length

                        WriteByte(123); // {                        
                        WriteByte((byte)'C'); // C

                        Write(bytes.ToArray(), 0, bytes.Count);
                        WriteByte(10);

                    }
                    break;

                //case BarcodeType.code128: //todo: overload PrintBarcode method with a byte array parameter

                //   WriteByte(GS);
                //    WriteByte(107);

                //    WriteByte(73); //todo: use format 2 (init string : 29,107,73) (0x00 can be a value, too
                //    WriteByte((byte)(originalBytes.Length)); //length

                //    WriteByte(123); // {
                //    WriteByte(65); // A
                //                   /* foreach(var b in originalBytes)
                //                     {
                //                         WriteByte(b);
                //                     }*/

                //    Write(originalBytes, 0, originalBytes.Length);
                //    WriteByte(0);
                //    break;

                case BarcodeType.code11:
                    if (data.Length > 1)
                    {
                        WriteByte(GS);
                        WriteByte(107);
                        WriteByte(9);
                        Write(outputBytes, 0, data.Length);
                        WriteByte(0);
                    }
                    break;
                case BarcodeType.msi:
                    if (data.Length > 1)
                    {
                        WriteByte(GS);
                        WriteByte(107);
                        WriteByte(10);
                        Write(outputBytes, 0, data.Length);
                        WriteByte(0);
                    }
                    break;
            }
        }

        public void SetLargeBarcode(int large)
        {
            WriteByte(GS);
            WriteByte(119);
            WriteByte((byte)large);
        }

        public void SetHeigthBarcode(int heigth)
        {
            WriteByte(GS);
            WriteByte(104);
            WriteByte(50);
        }

        public void SetBarcodeLeftSpace(byte spacingDots)
        {
            WriteByte(GS);
            WriteByte(120);
            WriteByte(spacingDots);
        }

        public void SetFontSize(byte height = 1)
        {
            WriteByte(ESC);
            WriteByte(33);
            WriteByte(height);
        }

        public void SetEspaceBetweenLines(byte space = 0)
        {
            WriteByte(ESC);
            WriteByte(51);
            WriteByte(space);
        }

        public void SetMarginLeft(byte value = 10, byte margin = 2)
        {
            //sweda = 2  //elgin = 0
            WriteByte(GS);
            WriteByte(76);
            WriteByte(value);
            WriteByte(margin);
        }

        public void SetColumn(byte column, byte value)
        {
            WriteByte(ESC);
            WriteByte(68);
            WriteByte(column);
            WriteByte(value);

        }

        public void NextTab()
        {
            WriteByte(9);

        }

        public void SetPrintingParameters(byte maxPrinting, byte heatingTime, byte heatingInterval)
        {
            WriteByte(ESC);
            WriteByte(55);
            WriteByte(maxPrinting);
            WriteByte(heatingTime);
            WriteByte(heatingInterval);
        }

        public void Sleep()
        {
            WriteByte(ESC);
            WriteByte(61);
            WriteByte(0);
        }

        public void WakeUp()
        {
            WriteByte(ESC);
            WriteByte(61);
            WriteByte(1);
        }

        public override string ToString()
        {
            return string.Format("Printer:\n\tSerialPort={0},\n\tMaxPrinting={1}," +
                "\n\tHeatingTime={2},\n\tHeatingInterval={3},\n\tPictureLineSleepTimeMs={4}," +
                "\n\tWriteLineSleepTimeMs={5},\n\tLocalEncoding={6}", GetPortName(), MaxPrinting,
                HeatingTime, HeatingInterval, PictureLineSleepTimeMs, WriteLineSleepTimeMs, LocalEncoding);
        }

        public void FeedDots(byte dotsToFeed)
        {
            WriteByte(ESC);
            WriteByte(74);
            WriteByte(dotsToFeed);
        }
        public void SetEncodingPtBR()
        {
            WriteByte(ESC);
            WriteByte((byte)'t');
            WriteByte(2);// pc850
        }

        public void SetEncoding(byte v)
        {
            WriteByte(ESC);
            WriteByte((byte)'t');
            WriteByte(v);
        }

        private void SendEncoding(string encoding)
        {
            this.LocalEncoding = encoding;
            SetEncodingPtBR();

            return;



            switch (encoding)
            {
                case "IBM437":
                    WriteByte(ESC);
                    WriteByte(116);
                    WriteByte(0);
                    break;
                case "ibm850":
                    WriteByte(ESC);
                    WriteByte(116);
                    WriteByte(1);
                    break;
            }
        }

        static private bool BitTest(byte valueToTest, int testBit)
        {
            return ((valueToTest & (byte)(1 << testBit)) == (byte)(1 << testBit));
        }

        static private byte BitSet(byte originalValue, byte bit)
        {
            return originalValue |= (byte)((byte)1 << bit);
        }

        static private byte BitClear(byte originalValue, int bit)
        {
            return originalValue &= (byte)(~(1 << bit));
        }

        public void WriteToBuffer(string text) => WriteToBuffer(text, LocalEncoding);

        public void SetMarginLeft(byte value)
        {
            WriteByte(GS);
            WriteByte(76);
            WriteByte(value);
            WriteByte(0);
        }

        public void SetLetterSpacing(byte letterSpacing)
        {
            WriteByte(ESC);
            WriteByte(32);
            WriteByte(letterSpacing);
        }

        public void ExecuteActions(IList<Action> actionsForPrinter)
        {
            foreach (var action in actionsForPrinter)
            {
                action.Invoke();
            }
        }

        public void PrintQrCode(string data)
        {
            string QRdata = data;
            int store_len = QRdata.Length + 3; // 414
            byte store_pL = (byte)(store_len % 256); // 158
            byte store_pH = (byte)(store_len / 256); // 1            

            byte[] modelQR = { 29, 40, 107, 4, 0, 49, 65, 50, 0 }; //FUNCTION 181
            byte[] sizeQR = { 29, 40, 107, 3, 0, 49, 67, 5 }; //FUNCTION 167
            byte[] errorQR = { 29, 40, 107, 3, 0, 49, 69, 48 }; //FUNCTION 169
            byte[] storeQR = { 29, 40, 107, store_pL, store_pH, 49, 80, 48 };  //FUNCTION 180
            byte[] printQR = { 29, 40, 107, 3, 0, 49, 81, 48 }; //FUNCTION 181

            byte[] originalBytes;

            originalBytes = Encoding.UTF8.GetBytes(QRdata.ToUpper());

            Write(modelQR, 0, modelQR.Length);
            Write(sizeQR, 0, sizeQR.Length);
            Write(errorQR, 0, errorQR.Length);
            Write(storeQR, 0, storeQR.Length);
            Write(originalBytes, 0, originalBytes.Length);
            Write(printQR, 0, printQR.Length);

        }

        public void PrintImage(string fileName, int maxWidth = 300, Bitmap bpm = null)
        {
            if (bpm == null)
            {
                if (!File.Exists(fileName))
                {
                    throw (new Exception("File does not exist."));
                }
            }

            BitmapData data = GetBitmapData(fileName, maxWidth, bpm);
            BitArray dots = data.Dots;
            byte[] width = BitConverter.GetBytes(data.Width);

            int offset = 0;
            MemoryStream stream = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(stream);

            if (namePrinter != "elgin")
            {
                //bw.Write((char)27);
                //bw.Write('@');
            }

            bw.Write((char)27);
            bw.Write('3');
            bw.Write((byte)24);

            while (offset < data.Height)
            {
                bw.Write((char)27);
                bw.Write('*');         // bit-image mode
                bw.Write((byte)33);    // 24-dot double-density
                bw.Write(width[0]);  // width low byte
                bw.Write(width[1]);  // width high byte

                for (int x = 0; x < data.Width; ++x)
                {
                    for (int k = 0; k < 3; ++k)
                    {
                        byte slice = 0;
                        for (int b = 0; b < 8; ++b)
                        {
                            int y = (((offset / 8) + k) * 8) + b;
                            // Calculate the location of the pixel we want in the bit array.
                            // It'll be at (y * width) + x.
                            int i = (y * data.Width) + x;

                            // If the image is shorter than 24 dots, pad with zero.
                            bool v = false;
                            if (i < dots.Length)
                            {
                                v = dots[i];
                            }
                            slice |= (byte)((v ? 1 : 0) << (7 - b));
                        }

                        bw.Write(slice);
                    }
                }
                offset += 24;
                if (offset < data.Height)
                {
                    bw.Write((char)0x0A);
                }

            }
            // Restore the line spacing to the default of 30 dots.
            bw.Write((char)27);
            bw.Write('3');
            bw.Write((byte)30);

            bw.Flush();
            byte[] bytes = stream.ToArray();
            Write(bytes, 0, bytes.Length);
        }

        public BitmapData GetBitmapData(string bmpFileName, int xmultiplier = 350, Bitmap bpm = null)
        {
            Bitmap bitmap = null;
            if (bpm != null)
            {
                bitmap = bpm;
            }
            else
            {
                bitmap = (Bitmap)Bitmap.FromFile(bmpFileName);
            }

            var threshold = 127;
            var index = 0;
            double multiplier = xmultiplier; // this depends on your printer model. for Beiyang you should use 1000
            double scale = (double)(multiplier / (double)bitmap.Width);
            int xheight = (int)(bitmap.Height * scale);
            int xwidth = (int)(bitmap.Width * scale);
            var dimensions = xwidth * xheight;
            var dots = new BitArray(dimensions);

            for (var y = 0; y < xheight; y++)
            {
                for (var x = 0; x < xwidth; x++)
                {
                    var _x = (int)(x / scale);
                    var _y = (int)(y / scale);
                    var color = bitmap.GetPixel(_x, _y);
                    var luminance = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                    dots[index] = (luminance < threshold);
                    index++;
                }
            }
            return new BitmapData()
            {
                Dots = dots,
                Height = (int)(bitmap.Height * scale),
                Width = (int)(bitmap.Width * scale)
            };

        }       

        public class BitmapData
        {
            public BitArray Dots
            {
                get;
                set;
            }

            public int Height
            {
                get;
                set;
            }

            public int Width
            {
                get;
                set;
            }
        }
        public void setConfigurationInitial(IPrinter printer, string nameprinter = "sweda")
        {
            printer.Reset();
            if (nameprinter.Equals("sweda"))
            {
                printer.SetEspaceBetweenLines(45);
                printer.SetFontSize(5);
            }
            else
            {
                printer.SetEspaceBetweenLines(45);
                printer.SetMarginLeft(20, 0);
                printer.SetFontSize(5);
                MapSpecialCharacter['Ô'] = 140;
                MapSpecialCharacter['Õ'] = 153;
                MapSpecialCharacter['õ'] = 148;

            }
        }

        public string GetPrinterModel()
        {
            WriteByte(GS);
            WriteByte(73);
            WriteByte(67);

            Thread.Sleep(3);
            string ret = "";
            int c;
            c = ReadChar();
            while (c >= 0)
            {
                c = ReadChar();
                ret += (char)c;
            }

            return ret;
        }

        public byte GetPrinterStatus(StatusType statusType)
        {
            WriteByte(DLE);
            WriteByte(EOT);
            WriteByte((byte)statusType);
            Thread.Sleep(100);
            int c = ReadChar();
            while (ReadChar() != -1)
            {
                Thread.Sleep(1);
            }
            return (byte)c;
        }

        public void PageModeOn()
        {
            WriteByte(ESC);
            WriteByte((byte)'L');
        }

        public void PageModeOff()
        {
            WriteByte(ESC);
            WriteByte((byte)'S');
        }

        public void PrintPageMode()
        {
            WriteByte(ESC);
            WriteByte(FF);
        }

        static int MmToDots(int dpi, double mm)
        {
            double fator = dpi / 25.4;
            return Convert.ToInt32(fator * mm);
        }

        static void CalculePos(int dpi, double valorEmMilimetro, out int l, out int h)
        {
            int valorTotal = MmToDots(dpi, valorEmMilimetro);
            int divisao = valorTotal > 256 ? valorTotal / 256 : 0;
            int resto = valorTotal - (divisao * 256);
            h = divisao;
            l = resto;
        }

        public void SetModePageArea(double x, double y, double width, double height, int dpi = 203)
        {
            int xl, xH, yl, yH, xwl, xwH, ywl, ywH;
            // calculando x inicial
            CalculePos(dpi, x, out xl,  out xH);
            // calculando y inicial
            CalculePos(dpi, y, out yl, out yH);
            // calculando largura 
            CalculePos(dpi, width, out xwl, out xwH);
            // calculando altura 
            CalculePos(dpi, height, out ywl, out ywH);
            // Enviando o comando
            SetModePageArea(xl, xH, yl, yH, xwl, xwH, ywl, ywH);

        }
        private void SetModePageArea(int xL, int xH, int yL, int yH, int dxL, int dxH, int dyL, int dyH)
        {
            WriteByte(ESC);
            WriteByte((byte)'W');

            WriteByte((byte)xL);
            WriteByte((byte)xH);
            WriteByte((byte)yL);
            WriteByte((byte)yH);
            WriteByte((byte)dxL);
            WriteByte((byte)dxH);
            WriteByte((byte)dyL);
            WriteByte((byte)dyH);
        }

        void IPrinter.HorizontalLine(int dpi, double largura, bool fontSmall)
        {
            WriteByte(ESC);
            WriteByte(116);
            WriteByte(0);

            int larguraEmDots = MmToDots(dpi, largura);
            int laguraFonte = fontSmall ? 9 : 12;
            int qtdCaracters = larguraEmDots / laguraFonte;            
            for (int i = 0; i < qtdCaracters; i++)
            {
                WriteByte(0xC4);
            }
            WriteByte(10);

            SetEncodingPtBR();
        }

        public void SetFontA()
        {
            WriteByte(ESC);
            WriteByte((byte)'M');
            WriteByte(0);
        }
        public void SetFontB()
        {

            WriteByte(ESC);
            WriteByte((byte)'M');
            WriteByte(1);
        }

        public void OpenDrawer()
        {
            WriteByte(DLE);
            WriteByte(DC4);
            WriteByte(0);
            WriteByte(2);

        }
    }
}

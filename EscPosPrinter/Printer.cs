using EscPosPrinter.PortFactory;
using EscPosPrinter.PortFactory.Enums;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace EscPosPrinter
{
    public class Printer : PortWriter, IPrinter
    {
        private byte MaxPrinting = 7;
        private byte HeatingTime = 80;
        private byte HeatingInterval = 2;

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

            Thread.Sleep(WriteLineSleepTimeMs);
        }

        public void SetInversionOn()
        {
            WriteByte(29);
            WriteByte(66);
            WriteByte(1);
        }

        public void SetInversionOff()
        {
            WriteByte(29);
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

            WriteByte(27);
            WriteByte(33);
            WriteByte(DoubleHeight + DoubleWidth + Bold);
        }

        public void SetBifOff()
        {
            WriteByte(27);
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
                WriteByte(27);
                WriteByte(45);
                WriteByte(underlineHeight);
            }

            WriteByte(27);
            WriteByte(33);
            WriteByte(style);
        }

        public void SetStyleOff(ref byte underlineHeight)
        {
            if (underlineHeight != 0)
            {
                WriteByte(27);
                WriteByte(45);
                WriteByte(0);
            }
            WriteByte(27);
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
            WriteByte(27);
            WriteByte(32);
            WriteByte(1);
            WriteByte(27);
            WriteByte(69);
            WriteByte(1);
        }

        public void BoldOff()
        {
            WriteByte(27);
            WriteByte(32);
            WriteByte(0);
            WriteByte(27);
            WriteByte(69);
            WriteByte(0);
        }

        public void WriteLineBold(string text)
        {
            BoldOn();

            WriteLine(text);

            BoldOff();

            LineFeed();
        }

        public void WhiteOnBlackOn()
        {
            WriteByte(29);
            WriteByte(66);
            WriteByte(1);
        }

        public void WhiteOnBlackOff()
        {
            WriteByte(29);
            WriteByte(66);
            WriteByte(0);
        }

        public void SetSize(bool doubleWidth, bool doubleHeight)
        {
            int sizeValue = (Convert.ToInt32(doubleWidth)) * (0xF0) + (Convert.ToInt32(doubleHeight)) * (0x0F);
            WriteByte(29);
            WriteByte(33);
            WriteByte((byte)sizeValue);
        }

        public void LineFeed()
        {
            WriteByte(10);
        }

        public void LineFeed(byte lines)
        {
            WriteByte(27);
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

            WriteByte(27);
            WriteByte(66);
            WriteByte(columns);
        }

        public void SetLineSpacing(byte lineSpacing)
        {
            WriteByte(27);
            WriteByte(51);
            WriteByte(lineSpacing);
        }

        public void SetAlignLeft()
        {
            WriteByte(27);
            WriteByte(97);
            WriteByte(0);
        }

        public void SetAlignCenter()
        {
            WriteByte(27);
            WriteByte(97);
            WriteByte(1);
        }

        public void SetAlignRight()
        {
            WriteByte(27);
            WriteByte(97);
            WriteByte(2);
        }

        public void HorizontalLine(int length)
        {
            if (length > 0)
            {
                if (length > 32)
                {
                    length = 32;
                }

                for (int i = 0; i < length; i++)
                {
                    WriteByte(0xC4);
                }
                WriteByte(10);
            }
        }

        public void Reset()
        {
            WriteByte(27);
            WriteByte(64);
            Thread.Sleep(50);
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
                        WriteByte(29);
                        WriteByte(107);
                        WriteByte(0);
                        Write(outputBytes, 0, data.Length);
                        WriteByte(0);
                    }
                    break;
                case BarcodeType.upc_e:
                    if (data.Length == 11 || data.Length == 12)
                    {
                        WriteByte(29);
                        WriteByte(107);
                        WriteByte(1);
                        Write(outputBytes, 0, data.Length);
                        WriteByte(0);
                    }
                    break;
                case BarcodeType.ean13:
                    if (data.Length == 12 || data.Length == 13)
                    {
                        WriteByte(29);
                        WriteByte(107);
                        WriteByte(2);
                        Write(outputBytes, 0, data.Length);
                        WriteByte(0);
                    }
                    break;
                case BarcodeType.ean8:
                    if (data.Length == 7 || data.Length == 8)
                    {
                        WriteByte(29);
                        WriteByte(107);
                        WriteByte(3);
                        Write(outputBytes, 0, data.Length);
                        WriteByte(0);
                    }
                    break;
                case BarcodeType.code39:
                    if (data.Length > 1)
                    {
                        WriteByte(29);
                        WriteByte(107);
                        WriteByte(4);
                        Write(outputBytes, 0, data.Length);
                        WriteByte(0);
                    }
                    break;
                case BarcodeType.i25:
                    if (data.Length > 1 || data.Length % 2 == 0)
                    {
                        WriteByte(29);
                        WriteByte(107);
                        WriteByte(5);
                        Write(outputBytes, 0, data.Length);
                        WriteByte(0);
                    }
                    break;
                case BarcodeType.codebar:
                    if (data.Length > 1)
                    {
                        WriteByte(29);
                        WriteByte(107);
                        WriteByte(6);
                        Write(outputBytes, 0, data.Length);
                        WriteByte(0);
                    }
                    break;
                case BarcodeType.code93: //todo: overload PrintBarcode method with a byte array parameter
                    if (data.Length > 1)
                    {
                        WriteByte(29);
                        WriteByte(107);
                        WriteByte(7); //todo: use format 2 (init string : 29,107,72) (0x00 can be a value, too)
                        Write(outputBytes, 0, data.Length);
                        WriteByte(0);
                    }
                    break;
                case BarcodeType.code128: //todo: overload PrintBarcode method with a byte array parameter
                    if (data.Length > 1)
                    {
                        WriteByte(29);
                        WriteByte(107);
                        WriteByte(8); //todo: use format 2 (init string : 29,107,73) (0x00 can be a value, too)
                        Write(outputBytes, 0, data.Length);
                        WriteByte(0);
                    }
                    break;
                case BarcodeType.code11:
                    if (data.Length > 1)
                    {
                        WriteByte(29);
                        WriteByte(107);
                        WriteByte(9);
                        Write(outputBytes, 0, data.Length);
                        WriteByte(0);
                    }
                    break;
                case BarcodeType.msi:
                    if (data.Length > 1)
                    {
                        WriteByte(29);
                        WriteByte(107);
                        WriteByte(10);
                        Write(outputBytes, 0, data.Length);
                        WriteByte(0);
                    }
                    break;
            }
        }

        public void SetLargeBarcode(bool large)
        {
            if (large)
            {
                WriteByte(29);
                WriteByte(119);
                WriteByte(3);
            }
            else
            {
                WriteByte(29);
                WriteByte(119);
                WriteByte(2);
            }
        }

        public void SetBarcodeLeftSpace(byte spacingDots)
        {
            WriteByte(29);
            WriteByte(120);
            WriteByte(spacingDots);
        }

        public void PrintImage(string fileName)
        {

            if (!File.Exists(fileName))
            {
                throw (new Exception("File does not exist."));
            }

            PrintImage(new Bitmap(fileName));

        }

        public void PrintImage(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;

            byte[,] imgArray = new byte[width, height];

            if (width != 384 || height > 65635)
            {
                throw (new Exception("Image width must be 384px, height cannot exceed 65635px."));
            }

            // Processing image data	
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < (image.Width / 8); x++)
                {
                    imgArray[x, y] = 0;
                    for (byte n = 0; n < 8; n++)
                    {
                        Color pixel = image.GetPixel(x * 8 + n, y);
                        if (pixel.GetBrightness() < 0.5)
                        {
                            imgArray[x, y] += (byte)(1 << n);
                        }
                    }
                }
            }

            // Print LSB first bitmap
            WriteByte(18);
            WriteByte(118);

            WriteByte((byte)(height & 255));   // height LSB
            WriteByte((byte)(height >> 8));    // height MSB


            for (int y = 0; y < height; y++)
            {
                System.Threading.Thread.Sleep(PictureLineSleepTimeMs);
                for (int x = 0; x < (width / 8); x++)
                {
                    WriteByte(imgArray[x, y]);
                }
            }
        }

        public void SetPrintingParameters(byte maxPrinting, byte heatingTime, byte heatingInterval)
        {
            WriteByte(27);
            WriteByte(55);
            WriteByte(maxPrinting);
            WriteByte(heatingTime);
            WriteByte(heatingInterval);
        }

        public void Sleep()
        {
            WriteByte(27);
            WriteByte(61);
            WriteByte(0);
        }

        public void WakeUp()
        {
            WriteByte(27);
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
            WriteByte(27);
            WriteByte(74);
            WriteByte(dotsToFeed);
        }

        private void SendEncoding(string encoding)
        {
            switch (encoding)
            {
                case "IBM437":
                    WriteByte(27);
                    WriteByte(116);
                    WriteByte(0);
                    break;
                case "ibm850":
                    WriteByte(27);
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
            WriteByte(29);
            WriteByte(76);
            WriteByte(value);
            WriteByte(0);
        }

        public void SetLetterSpacing(byte letterSpacing)
        {
            WriteByte(27);
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
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;

namespace BibliotecaImpressaoEscPos.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var port = "COM4";
            SerialPort printerPort = new SerialPort(port, 9600);

            if (printerPort != null)
            {
                System.Console.WriteLine($"Porta {port}: OK!");
                if (printerPort.IsOpen)
                {
                    printerPort.Close();
                }
            }

            System.Console.WriteLine($"Abrindo a porta {port}...");
            try
            {
                printerPort.Open();
            }
            catch
            {
                System.Console.WriteLine("Erro de I/O!");
                System.Console.ReadKey();
                Environment.Exit(0);
            }

            Printer printer = new Printer(printerPort, 2, 180, 2);
            printer.WakeUp();
            System.Console.WriteLine(printer.ToString());

            TestReceipt(printer);

            printer.SetBarcodeLeftSpace(25);
            TestBarcode(printer);

            //TestImage(printer);

            printer.WriteLineSleepTimeMs = 200;
            printer.WriteLine("Default style...");
            printer.WriteLine("PrintingStyle.Bold...", Printer.PrintingStyle.Bold);
            printer.WriteLine("PrintingStyle.DeleteLine...", Printer.PrintingStyle.DeleteLine);
            printer.WriteLine("PrintingStyle.DoubleHeight...", Printer.PrintingStyle.DoubleHeight);
            printer.WriteLine("PrintingStyle.DoubleWidth...", Printer.PrintingStyle.DoubleWidth);
            printer.WriteLine("PrintingStyle.Reverse...", Printer.PrintingStyle.Reverse);
            printer.WriteLine("PrintingStyle.Underline...", Printer.PrintingStyle.Underline);
            printer.WriteLine("PrintingStyle.Updown...", Printer.PrintingStyle.Updown);
            printer.WriteLine("PrintingStyle.ThickUnderline...", Printer.PrintingStyle.ThickUnderline);
            printer.SetAlignCenter();
            printer.WriteLine("BIG TEXT!", ((byte)Printer.PrintingStyle.Bold + (byte)Printer.PrintingStyle.DoubleHeight + (byte)Printer.PrintingStyle.DoubleWidth));
            printer.SetAlignLeft();
            printer.WriteLine("Default style again");

            printer.LineFeed(3);
            printer.Guillotine();

            printer.Sleep();
            System.Console.WriteLine("Printer offline!");

            printerPort.Close();

            System.Console.ReadKey();
        }

        static void TestReceipt(Printer printer)
        {
            Dictionary<string, int> ItemList = new Dictionary<string, int>(100);
            printer.SetLineSpacing(0);
            printer.SetAlignCenter();
            printer.WriteLine("MY SHOP", (byte)Printer.PrintingStyle.DoubleHeight + (byte)Printer.PrintingStyle.DoubleWidth);
            printer.WriteLine("My address, CITY");
            printer.LineFeed();
            printer.LineFeed();

            ItemList.Add("Item #1", 8990);
            ItemList.Add("Item #2 goes here", 2000);
            ItemList.Add("Item #3", 1490);
            ItemList.Add("Item number four", 490);
            ItemList.Add("Item #5 is cheap", 245);
            ItemList.Add("Item #6", 2990);
            ItemList.Add("The seventh item", 790);

            int total = 0;
            foreach (KeyValuePair<string, int> item in ItemList)
            {
                CashRegister(printer, item.Key, item.Value);
                total += item.Value;
            }

            printer.HorizontalLine(32);

            double dTotal = Convert.ToDouble(total) / 100;
            double VAT = 10.0;

            printer.WriteLine(String.Format("{0:0.00}", (dTotal)).PadLeft(32));

            printer.WriteLine("VAT 10,0%" + String.Format("{0:0.00}", (dTotal * VAT / 100)).PadLeft(23));

            printer.WriteLine(String.Format("$ {0:0.00}", dTotal * VAT / 100 + dTotal).PadLeft(16), Printer.PrintingStyle.DoubleWidth);

            printer.LineFeed();

            printer.WriteLine("CASH" + String.Format("{0:0.00}", (double)total / 100).PadLeft(28));
            printer.LineFeed();
            printer.LineFeed();
            printer.SetAlignCenter();
            printer.WriteLine("Have a good day.", Printer.PrintingStyle.Bold);

            printer.LineFeed();
            printer.SetAlignLeft();
            printer.WriteLine("Seller : Bob");
            printer.WriteLine("09-28-2011 10:53 02331 509");
            printer.LineFeed();
            printer.LineFeed();
            printer.LineFeed();
        }

        static void TestBarcode(Printer printer)
        {
            Printer.BarcodeType myType = Printer.BarcodeType.ean13;
            string myData = "3350030103392";
            printer.WriteLine(myType.ToString() + ", data: " + myData);
            printer.SetLargeBarcode(true);
            printer.LineFeed();
            printer.PrintBarcode(myType, myData);
            printer.SetLargeBarcode(false);
            printer.LineFeed();
            printer.PrintBarcode(myType, myData);
        }

        static void TestImage(Printer printer)
        {
            printer.WriteLine("Test image:");
            Bitmap img = new Bitmap("../../../mono-logo.png");
            printer.LineFeed();
            printer.PrintImage(img);
            printer.LineFeed();
            printer.WriteLine("Image OK");
        }

        static void CashRegister(Printer printer, string item, int price)
        {
            printer.Reset();
            printer.Indent(0);

            if (item.Length > 24)
            {
                item = item.Substring(0, 23) + ".";
            }

            printer.WriteToBuffer(item.ToUpper());
            printer.Indent(25);
            string sPrice = String.Format("{0:0.00}", (double)price / 100);

            sPrice = sPrice.PadLeft(7);

            printer.WriteLine(sPrice);
            printer.Reset();
        }
    }
}

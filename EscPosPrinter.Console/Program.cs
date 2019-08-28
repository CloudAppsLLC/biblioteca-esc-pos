using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using EscPosPrinter.PortFactory.Enums;

namespace EscPosPrinter.Console
{
    class Program
    {
        static int tryCount = 0;
        static readonly Stopwatch watch = new Stopwatch();

        static void Main(string[] args)
        {
            watch.Start();
            TesteInterpretador(4);
            watch.Stop();
            System.Console.WriteLine($"Tempo decorrido: {watch.ElapsedMilliseconds}ms");
            System.Console.WriteLine();

            //TestePrinter();
            System.Console.ReadKey();
        }

        static void TesteInterpretador(int port)
        {
            try
            {
                using (IPrinter printer = new Printer(port))
                {
                    var xml = @"<ce>centralizado</ce>
                                <l></l>
                                normal
                                <ad>
                                    a direita
                                    <sl>30</sl>
                                    <b>negrito e a direita</b>
                                </ad>
                                <b>apenas negrito</b>
                                <c>condensado</c>
                                <ad>normal a direita</ad>
                                <sl>30</sl>
                                <gui/>";

                    var actionsForPrinter = new Interpreter(printer).GenerateActionsForPrinter(xml);

                    if (actionsForPrinter != null)
                        printer.ExecuteActions(actionsForPrinter);

                    printer.Reset();
                    printer.Sleep();
                }
            }
            catch (Exception ex)
            {
                tryCount++;
                System.Console.WriteLine($"Tentativa: {tryCount}");
                TesteInterpretador(port);
            }
        }

        static void TestePrinter()
        {
            try
            {
                using (IPrinter printer = new Printer(4, 2, 180, 2))
                {
                    printer.WakeUp();
                    System.Console.WriteLine(printer.ToString());

                    TestReceipt(printer);

                    printer.SetBarcodeLeftSpace(25);
                    TestBarcode(printer);

                    //TestImage(printer);

                    printer.WriteLineSleepTimeMs = 200;
                    printer.WriteLine("Default style...");
                    printer.WriteLine("PrintingStyle.Bold...", PrintingStyle.Bold);
                    printer.WriteLine("PrintingStyle.DeleteLine...", PrintingStyle.DeleteLine);
                    printer.WriteLine("PrintingStyle.DoubleHeight...", PrintingStyle.DoubleHeight);
                    printer.WriteLine("PrintingStyle.DoubleWidth...", PrintingStyle.DoubleWidth);
                    printer.WriteLine("PrintingStyle.Reverse...", PrintingStyle.Reverse);
                    printer.WriteLine("PrintingStyle.Underline...", PrintingStyle.Underline);
                    printer.WriteLine("PrintingStyle.Updown...", PrintingStyle.Updown);
                    printer.WriteLine("PrintingStyle.ThickUnderline...", PrintingStyle.ThickUnderline);
                    printer.SetAlignCenter();
                    printer.WriteLine("BIG TEXT!", ((byte)PrintingStyle.Bold + (byte)PrintingStyle.DoubleHeight + (byte)PrintingStyle.DoubleWidth));
                    printer.SetAlignLeft();
                    printer.WriteLine("Default style again");

                    printer.LineFeed(3);
                    printer.Guillotine();

                    printer.Sleep();
                }
                System.Console.WriteLine("Printer offline!");

            }
            catch (Exception ex)
            {
                System.Console.WriteLine("{0} => {1}", ex.Message, ex.StackTrace);
            }
        }

        static void TestReceipt(IPrinter printer)
        {
            var ItemList = new Dictionary<string, int>(100);
            printer.SetLineSpacing(0);
            printer.SetAlignCenter();
            printer.WriteLine("MY SHOP", (byte)PrintingStyle.DoubleHeight + (byte)PrintingStyle.DoubleWidth);
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
            foreach (var item in ItemList)
            {
                var key = item.Key;
                var value = item.Value;
                printer.Reset();
                printer.Indent(0);

                if (key.Length > 24)
                {
                    key = key.Substring(0, 23) + ".";
                }

                printer.WriteToBuffer(key.ToUpper());
                printer.Indent(25);
                var sPrice = string.Format("{0:0.00}", (double)value / 100);

                sPrice = sPrice.PadLeft(7);

                printer.WriteLine(sPrice);
                printer.Reset();
                total += item.Value;
            }

            printer.HorizontalLine(32);

            double dTotal = Convert.ToDouble(total) / 100;
            double VAT = 10.0;

            printer.WriteLine(string.Format("{0:0.00}", (dTotal)).PadLeft(32));

            printer.WriteLine("VAT 10,0%" + string.Format("{0:0.00}", (dTotal * VAT / 100)).PadLeft(23));

            printer.WriteLine(string.Format("$ {0:0.00}", dTotal * VAT / 100 + dTotal).PadLeft(16), PrintingStyle.DoubleWidth);

            printer.LineFeed();

            printer.WriteLine("CASH" + string.Format("{0:0.00}", (double)total / 100).PadLeft(28));
            printer.LineFeed();
            printer.LineFeed();
            printer.SetAlignCenter();
            printer.WriteLine("Have a good day.", PrintingStyle.Bold);

            printer.LineFeed();
            printer.SetAlignLeft();
            printer.WriteLine("Seller : Bob");
            printer.WriteLine("09-28-2011 10:53 02331 509");
            printer.LineFeed();
            printer.LineFeed();
            printer.LineFeed();
        }

        static void TestBarcode(IPrinter printer)
        {
            var myType = BarcodeType.ean13;
            string myData = "3350030103392";
            printer.WriteLine(myType.ToString() + ", data: " + myData);
            printer.SetLargeBarcode(true);
            printer.LineFeed();
            printer.PrintBarcode(myType, myData);
            printer.SetLargeBarcode(false);
            printer.LineFeed();
            printer.PrintBarcode(myType, myData);
        }

        static void TestImage(IPrinter printer)
        {
            printer.WriteLine("Test image:");
            var img = new Bitmap("../../../mono-logo.png");
            printer.LineFeed();
            printer.PrintImage(img);
            printer.LineFeed();
            printer.WriteLine("Image OK");
        }
    }
}

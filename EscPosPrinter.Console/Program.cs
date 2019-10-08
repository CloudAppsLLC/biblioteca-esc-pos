using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using EscPosPrinter.PortFactory.Enums;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;
using ImageMagick;

namespace EscPosPrinter.Console
{
    class Program
    {
        static int tryCount = 0;
        static readonly Stopwatch watch = new Stopwatch();

        static void Main(string[] args)
        {
            //watch.Start();
            //TesteInterpretador(3);
            //watch.Stop();
            //System.Console.WriteLine($"Tempo decorrido: {watch.ElapsedMilliseconds}ms");
            //System.Console.WriteLine(); 

            TestePrinter();
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
                System.Console.WriteLine($"{ex.Message} => {ex.StackTrace}");
            }
        }

        static void TestePrinter()
        {
            int x = 0;
            try
            {
                using (IPrinter printer = new Printer(7, 2, 180, 2))
                {
                //    printer.MapSpecialCharacter['Ô'] = 147;
                //    printer.MapSpecialCharacter['õ'] = 111;
                //    printer.MapSpecialCharacter['Õ'] = 79;
                    printer.WakeUp();
                    printer.WriteLineSleepTimeMs = 200;
                    printer.Reset();


                  //  printer.setConfigurationInitial(printer, "elgin");

                    //for (byte i = 0; i < 8; i++) {
                    //    printer.SetFontSize1(i);
                    //    printer.WriteLine("= TESTE = ");
                    //}

                    // #region Definições5
                    //// printer.SetEncodingCarac(0);
                    // #endregion
                    // string barcode = "23190514200166000166599000100880000147661655"; // string myData = "23190514200166000166599000100880000147661655";
                    // // TestImage(printer);
                    // var cpf = "99999999999";
                    // var tributoFed = "41,85";
                    // var tributoEst = "22,60";
                    // var tributoMun = "0,00";
                    // var operador = "3071";

                    // // TestQRcode(printer);
                    // printer.SetAlignCenter();
                    // printer.SetMarginLeft(20, 0);
                    // //printer.WriteLineSleepTimeMs = 200;
                    // printer.WriteLine("OBSERVACOES DO CONTRIBUINTE");
                   //  printer.SetAlignLeft();
                   //  printer.WriteLine("--------------------------------------------------------");
                   //  printer.WriteLine("CLIENTE SEMPRE PAGUE MENOS");
                    // printer.WriteLine("OLA CLEBIO,");
                    // printer.WriteLine("VOCE E UM CLIENTE SEMPRE.");
                    // printer.WriteLine("NESSA COMPRA VOCE ECONOMIZOU R$ 48,36");
                    // printer.WriteLine("*ESSE VALOR E UMA ESTIMATIVA DO SEU SALDO DE COMPRAS, EXCLUINDO MEDICAMENTOS E SERVICOS. LEMBRE-SE QUE SUAS COMPRAS SAO CONTABILIZADAS EM ATE 15 DIAS. CONFIRA O REGULAMENTO E SEUS BENEFICIOS EM PORTAL.PAGUEMENOS.COM.BR/FIDELIDADE.");
                    // printer.WriteLine("--------------------------------------------------------");
                    // printer.WriteLine("CPF CLIENTE SEMPRE: " + cpf.ToString().Substring(0, 3) + ".***.***-" + cpf.ToString().Substring(9, 2) + " \n");
                    // printer.WriteLine("Operador: " + operador + "     Vendedor: ");
                    // printer.WriteLine("Trib aprox R$: " + tributoFed + " Fed e R$: " + tributoEst + " Est e R$: " + tributoMun + " Muni");
                    // printer.WriteLine("Fonte: IBPT ca7gi3");
                    // printer.WriteLine("Obrigado e Volte Sempre.");
                    // printer.WriteLine("Loja : 0052 - PDV : 0092");
                    // printer.WriteLine("--------------------------------------------------------");
                    // printer.SetAlignCenter();
                    // printer.WriteLine("SAT Nº 900.010.106");
                    // printer.WriteLine(DateTime.Now.ToString("dd/MM/yyyy") + " - " + DateTime.Now.ToString("HH:mm:ss"));
                    // printer.SetAlignLeft();
                    // printer.LineFeed();
                    // printer.SetAlignCenter();
                    // printer.SetMarginLeft(30, 0);
                    // printer.WriteLine(Regex.Replace(barcode, ".{4}", "$0 "));   
                    // #region barcode
                    // // printer.SetAlignLeft();
                    // printer.SetMarginLeft(20, 0);
                    // TestBarcode(printer, barcode);
                    // #endregion
                    // printer.LineFeed();
                    //  TestQRcode(printer);
                    // printer.LineFeed();
                    // printer.WriteLine("Consulte o QR Code pelo aplicativo 'De olho na nota', disponível na AppStore (Apple) e PlayStore (Android).");


                    //printer.WriteLine("\x1d\x57\x2c\x01");   //# Set print area width of 300
                    //printer.WriteLine("\x1b\x24\x64\x00");  // # Set absolute print position to 100
                    //printer.WriteLine("\x1b\x4d\x01");       # Select character font
                    ////# Write text to see multiple lines
                    //printer.WriteLine("Print area width of 300 and absolute print position of 100. Only the first line should have this absolute print position.");
                    //print()




                    printer.WakeUp();
                    System.Console.WriteLine(printer.ToString());





                    //printer.SetTestPag();

                    //  TestReceipt(printer);
                    //SetDadosLoja(printer);
                    printer.Reset();
                    //  printer.SetFontSize(6);
                    //    printer.SetEspaceBetweenLines(45);
                    // printer.SetMarginLeft(10, 2);
                    // printer.setConfigurationInitial(printer, "sweda");
                    // printer.SetAlignLeft();
                    //printer.HorizontalLine(100);
                    //printer.SetColumn(2, 32);

                    //   printer.SetModoPag();
                    //printer.NextTable();
                    //printer.NextTable();
                    //TestImage(printer);
                    // printer.SetMarginLeft(10,0);
                    // printer.HorizontalLine("elgin");
                    //// printer.SetFontSize(10);


                    // printer.BoldOn();
                    // printer.WriteToBuffer("Total R$");
                    //printer.WriteLineBold();

                    // printer.SetFontSize(5);
                    //printer.BoldOn();
                    //printer.WriteToBuffer(string.Format("{0:0,0.00}", 110.50).PadLeft(46, ' '));
                    //printer.BoldOff();
                    //printer.LineFeed();
                    //printer.HorizontalLine("elgin");
                    //printer.WriteLine("teste alguma coisa dois");
                    // printer.HorizontalLine(56);
                    //printer.SetLetterSpacing(100);[     
                                  
                    printer.Reset();
                    //printer.WriteLine("ãéíóúÁÉÍÓÚÔÕÇAEIOUaeiou");                   
                    //printer.SetFontSize(32);
                    //printer.WriteLine("teste alguma coisa dois");
                    //printer.WriteLine("-------------------------------------------------------");
                    //printer.SetFontSize(1);
                    //printer.WriteLine("teste alguma coisa dois");
                    //printer.WriteLine("-------------------------------------------------------");
                    //printer.SetFontSize(2);
                    //printer.WriteLine("teste alguma coisa dois");
                    //  printer.setONuvem();
                    //  printer.setCedilha();
                    // printer.setOchapeu();


                    //printer.SetLineSpacing(30);
                    //printer.WriteLine("teste alguma coisa dois");
                    // printer.SetLetterSpacing(200);
                    // printer.WriteLine("-------------------------------------------------------");
                    // printer.convertImage(@"C:\cosmos\NFCe\arquivos-pdf\imagem.pdf");

                    // string text = "Farmacia Pague Menos. \n Isso é um test para imprimir imagem"; 

                    // DrawText(text, Color.FromArgb(50, Color.Gray), 300, @"C:\Users\90004444\Downloads\");
                    //TextoImage(printer, "EMPREENDIMENTOS PAGUE MENOS S.A", "CNPJ: 06626253024689 IE: 77871411", "IM: 0000000000000000", "RUA CONDE DE BONFIM DE ALMEIDA", "RIO DE JANEIRO - RJ");

                    //ImagetoImage(printer, "RUA CONDE DE BONFIM RUA CONDE DE BONFIM RUA CONDE DE BONFIM");
                    //printer.WriteLine("-------------------------------------------------------");

                    //printer.WriteLine("ÓÚÕÔáéíóúãõâêÂÊ");
                    //printer.SetFontSize(0);

                    //printer.WriteLine("Isso e um TESTE de IMPRESSORA, teste");
                    //printer.SetFontSize(1);

                    //printer.WriteLine("Isso e um TESTE de IMPRESSORA, teste");
                    //printer.SetFontSize(2);

                    //printer.WriteLine("Isso e um TESTE de IMPRESSORA, teste");




                    //printer.WriteLine("-------------------------------------------------------");
                    //printer.SetModoPosition(49);
                    //printer.SetLetterSpacing(50);
                    //  printer.WriteLine("quero ver");
                    //   printer.WriteLine("quero ver");
                    //   printer.WriteLine("quero ver");
                    //    printer.WriteLine("quero ver");
                    //printer.MainTest();

                    //TestImage(printer);
                    //    printer.SetPrintModoPag();

                    // printer.FeedDots(100);
                    //   printer.NextTable();
                    //   printer.NextTable();

                    // printer.SetModoPosition(48);
                    //   printer.WriteToBuffer("teste");




                    //     printer.SetModoPagtr();
                    // printer.SetAlignRight();


                    //     printer.SendEncodingtete();
                    //     printer.SetModoPosition(49);
                    //     printer.WriteLine("você é ação! ");



                    // printer.SetColumn(1, 8);
                    //     printer.SetModoPosition(51);

                    //    printer.WriteToBuffer("teste 1");
                    //    printer.NextTable();
                    //    printer.WriteToBuffer("teste 2");
                    // printer.NextTable();
                    //   printer.WriteLine("3");
                    //  printer.SetColumn(80, 8);
                    //   TestImage(printer);
                    //   printer.NextTable();
                    //   printer.WriteLine("4");






                    //   printer.WriteLineSleepTimeMs = 200;
                    //   printer.WriteLine("Default style...");
                    //   printer.WriteLine("PrintingStyle.Bold...", PrintingStyle.Bold);
                    //printer.WriteLine("PrintingStyle.DeleteLine...", PrintingStyle.DeleteLine);
                    //printer.WriteLine("PrintingStyle.DoubleHeight...", PrintingStyle.DoubleHeight);
                    //printer.WriteLine("PrintingStyle.DoubleWidth...", PrintingStyle.DoubleWidth);
                    //printer.WriteLine("PrintingStyle.Reverse...", PrintingStyle.Reverse);
                    //printer.WriteLine("PrintingStyle.Underline...", PrintingStyle.Underline);
                    //printer.WriteLine("PrintingStyle.Updown...", PrintingStyle.Updown);
                    //printer.WriteLine("PrintingStyle.ThickUnderline...", PrintingStyle.ThickUnderline);
                    //printer.SetAlignCenter();
                    //printer.WriteLine("BIG TEXT!", ((byte)PrintingStyle.Bold + (byte)PrintingStyle.DoubleHeight + (byte)PrintingStyle.DoubleWidth));
                    //printer.SetAlignLeft();
                    //printer.WriteLine("Default style again");

                    //TestBarcode(printer);

                    // TestQRcode(printer);

                    //   printer.LineFeed(3);

                    PrinterStatus ps = new PrinterStatus();
                    while (true)
                    {

                        System.Console.WriteLine("");
                        System.Console.WriteLine("OBTENDO NOVA LEITURA:");
                        var ret = printer.GetPrinterModel();
                        System.Console.WriteLine($"Nome da impressora {ret}");
                        for (int j = 1; j < 5; j++)
                        {
                            var bt = (printer as Printer).GetPrinterStatus((StatusType)j);
                            switch( (StatusType) j)
                            {
                                case StatusType.IndicadorErro:
                                    ps.SetIndicadorErro(bt);
                                    break;
                                case StatusType.EstadoImpressora:
                                    ps.SetEstadoImpressora(bt);
                                    break;
                                case StatusType.IndicadorDesligamento:
                                    ps.SetIndicadorDesligamento(bt);
                                    break;
                                case StatusType.SensorPapel:
                                    ps.SetSensorPapel(bt);
                                    break;
                            }
                           
                            //System.Console.Write($"O status do byte: {j}");
                            //var bytes = "";
                            //for (int i = 0; i < 8; i++)
                            //{

                            //    if ((bt & (1 << i)) != 0)
                            //    {
                            //        bytes += "L";
                            //        continue;
                            //    }
                            //    bytes += "D";
                            //}
                            //System.Console.WriteLine(bytes);
                        }
                        System.Console.WriteLine($"SENSOR SEM PAPEL: {ps.SemPapel}");
                        System.Console.WriteLine($"SENSOR POUCO PAPEL: {ps.PoucoPapel}");
                        System.Console.WriteLine($"SENSOR TAMPA ABERTA: {ps.TampaAberta}");
                        System.Console.WriteLine($"SENSOR ERRO: {ps.EmErro}");
                        System.Console.WriteLine($"SENSOR GAVETA: {ps.SinalGaveta}");


                        //var bt = (printer as Printer).GetPrinterStatus(StatusType.SensorPapel);
                        //if ( (bt & (bt << 5)) > 0)
                        //{
                        //    System.Console.WriteLine("Impressora sem papel");
                        //}
                        //else
                        //{
                        //    System.Console.WriteLine("Impressora com papel");
                        //}
                        //if ((bt & (bt << 2)) > 0)
                        //{
                        //    System.Console.WriteLine("Tampa aberta");
                        //}
                        //else
                        //{
                        //    System.Console.WriteLine("Tampa normal");
                        //}
                        //if ((bt & (bt << 6)) > 0)
                        //{
                        //    System.Console.WriteLine("Impressora em erro");
                        //}
                        //else
                        //{
                        //    System.Console.WriteLine("Impressora sem erro");
                        //}
                        System.Threading.Thread.Sleep(1000);
                    }
                        //printer.Guillotine();

                        printer.Sleep();
                    x++;
                }
                System.Console.WriteLine("Printer offline!");

            }
            catch (Exception ex)
            {
                System.Console.WriteLine("{0} => {1}", ex.Message, ex.StackTrace);
                x = 0;
            }
            finally
            {
                if (x < 1) TestePrinter();
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

            //printer.HorizontalLine(32);

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

        //static void ImagetoImage(IPrinter printer, string logradouro)
        //{
        //    string pathtexto = @"C:\Users\90004444\Desktop\image\arquivo.png";
        //    string pathlogo = @"C:\Users\90004444\Desktop\image\logo.bmp";
        //    string resultado = @"C:\Users\90004444\Desktop\image\resultado.png";

        //  // Bitmap test = printer.TexttoImage(printer, "EMPREENDIMENTOS PAGUE MENOS S.A", "CNPJ: 06626253024689 IE: 77871411", "IM: 0000000000000000",logradouro, "RIO DE JANEIRO - RJ");


        //    using (MagickImage logo = new MagickImage(pathlogo))
        //    {
        //        MagickImage texto = new MagickImage(test);
        //        logo.Composite(texto, 380, -10);
        //        logo.Write(resultado);
        //        Bitmap bitmap = logo.ToBitmap();


        //        int maxheight;
        //       // maxheight = Math.Max( test.Height , 240);
        //       // string str = "";
        //        //int testlength = logradouro.Length;

        //        //if (logradouro.Length > 33)
        //        //{
        //        //    maxheight = 350;
        //        //   // str = logradouro.Substring(33, (logradouro.Length - 33));
        //        //}

        //        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, 1400, maxheight);
        //        Bitmap resi = bitmap.Clone(rect, bitmap.PixelFormat);

        //        resi.Save(resultado);

        //        printer.PrintImage(@"C:\Users\90004444\Desktop\image\resultado.png", 700, resi);


        //    }
        //}

        //static void TextoImage(IPrinter printer, string empresa, string cnpj, string im, string logradoro, string cidade)
        //{

        //    int maxheight = 320;
        //    string str = "";
        //    int testlength = logradoro.Length;

        //    if (logradoro.Length > 33)
        //    {
        //        maxheight = 330;
        //        str = logradoro.Substring(33 , (logradoro.Length - 33)); 
        //    }          

        //    Bitmap bmp = new Bitmap(900, maxheight, PixelFormat.Format24bppRgb);
        //    //define o arquivo de imagem   
        //    Graphics g = Graphics.FromImage(bmp);
        //    //define o controle de graficos   
            
        //    System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, 900, maxheight);
        //    //cria um quadrado aonde a pintura ser atuada   
            
        //    System.Drawing.Text.PrivateFontCollection privateFonts = new PrivateFontCollection();
        //    privateFonts.AddFontFile(@"C:\PMenosIntFis\Fonts\OpenSans-CondBold.ttf");
        //    System.Drawing.Font font = new System.Drawing.Font(privateFonts.Families[0], 36);

        //    g.FillRectangle(Brushes.White, rect);

        //    g.DrawString(empresa, new System.Drawing.Font("Verdana", 28, FontStyle.Bold), SystemBrushes.WindowText, new PointF(10, 10));
        //    g.DrawString(cnpj, new System.Drawing.Font("Verdana", 28, FontStyle.Regular), SystemBrushes.WindowText, new PointF(10, 60));
        //    g.DrawString(im, new System.Drawing.Font("Verdana", 28, FontStyle.Regular), SystemBrushes.WindowText, new PointF(10, 110));
        //    if(str != "")
        //    {
        //        g.DrawString(logradoro.Substring(0,33), new System.Drawing.Font("Verdana", 28, FontStyle.Regular), SystemBrushes.WindowText, new PointF(10, 160));
        //        g.DrawString(str, new System.Drawing.Font("Verdana", 28, FontStyle.Regular), SystemBrushes.WindowText, new PointF(10, 210));
        //        g.DrawString(cidade, new System.Drawing.Font("Verdana", 28, FontStyle.Regular), SystemBrushes.WindowText, new PointF(10, 260));
        //    }
        //    else
        //    { 
        //         g.DrawString(logradoro, new System.Drawing.Font("Verdana", 28, FontStyle.Regular), SystemBrushes.WindowText, new PointF(10, 160));
        //         g.DrawString(cidade, new System.Drawing.Font("Verdana", 28, FontStyle.Regular), SystemBrushes.WindowText, new PointF(10, 210));
        //    }
        //    //pinta o bmp, com o formato do rect, q ‚ o rectangulo q defini ali em cima   

        //    g.Save();
        //    //salva as configura‡äes(nÆo fa‡o ideia se isso ‚ necessario)   
        //    bmp.Save(@"C:\Users\90004444\Desktop\image\arquivo.png", System.Drawing.Imaging.ImageFormat.Png);
        //    //Salva o documento. troque o 2ø arg para se adequar ao seu gosto, nÆo esque‡a de manter o 1ø correto tamb‚m   

        //    g.Dispose();

        //    bmp.Dispose();

        //    g = null;

        //    bmp = null;
        //}
        
        static void TestBarcode(IPrinter printer, string barcode)
        {
            var myType = BarcodeType.code128;
            // string myData = "23190514200166000166599000100";
            //string myData = "23190514200166000166599000100880000147661655";
            //printer.WriteLine(myType.ToString() + ", data: " + myData);
            //printer.SetAlignCenter();
            printer.SetBarcodeLeftSpace(1);
            printer.SetHeigthBarcode(1);
            printer.SetLargeBarcode(1);
            //printer.SetHeigthBarcode(20);
            printer.PrintBarcode(myType, barcode);
            //printer.SetLargeBarcode(false);
            //printer.LineFeed();
            //printer.PrintBarcode(myType, myData);
            //printer.LineFeed(5);
        }

        static void TestQRcode(IPrinter printer)
        {
            printer.SetAlignCenter();
            printer.PrintQrCode(@"35150909165024000175590000193130072726117830|20150924062259|50.00||hdMEPiER6rjZKyKA+4+voi1nncxsAGFbYsEEqnh04SbvUEI/haUF4GUBPxT6Q2Uhf9f8QYgxiwxWo3GxRrvj4WnNeTYgAqUAYmOANPItNkOw0CppmZ4R8i1ZOlnftVhksCM0zrl4RiKgoazbN44hUu2nQf0W/JLvFXzXu12JlcSThNtmyJ6m9WBsMc/sf9BE14HDoXMyKRIQYt5TkEjilHH9Ffa0saRyUIp+Fji89/Moq8YCCFC+qC44XGxsvNCeeHUNOc1LgPP0DbU1miwpVnrBlEl87RU8Iy0r8fN/fNhbcStkwfTEvhYvZz42nEKHrmGTpGZYkHuTFCNZPq7aCA==");
            //printer.PrintQrCode(@"3350030103392");
            printer.SetAlignLeft();
            printer.LineFeed();

        }

        static void TestImage(IPrinter printer)
        {
            // printer.WriteLine("Test image:");
            // printer.LineFeed();
            printer.PrintImage(@"C:\Users\90004444\Downloads\sat-1.jpg", 580);
            //  printer.LineFeed();
            //  printer.WriteLine("Image OK");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using EscPosPrinter.PortFactory.Enums;

namespace EscPosPrinter.Console
{
    class Program
    {
        static int tryCount = 0;
        static readonly Stopwatch watch = new Stopwatch();

        static void Main(string[] args)
        {
            //watch.Start();
            TesteInterpretador(5);
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
                    var xml = @"02
	<c>
        <l></l>
		a
        <l></l>
		<ce>
			<b>NÃO-FISCAL</b>
		</ce>
		<l></l>
		Loja: 0052 - FOR57-DesMoreira2-CE
		<l></l>
		De :02/12/2019 a 02/12/2019
		<l></l>
		Operador: 3071   - FRANCISCO ANTONIO DOS SANTOS  
		<l></l>
		Finalizador    Qtd.Docs    Valor Liquido
		<l></l>
		-----------    ---------  -------------
		<l></l>
		DINHEIRO            -         45,44
		<l></l>
		-----------    ---------  -------------
		<l></l>Sub-Tot.(1)         0         45,44       
		<l></l>
		-----------    ---------  -------------
		<l></l>
		Faturamento Ecf:              45,44
		<l></l>
		-----------    ---------  -------------
		<l></l>
		--------------------------------------------------FIM
		<l></l>
	</c>
	<c>
		<l></l>
		<l></l>
	</c>
	__________________________________________
	<l></l>
	CAIXA: 084  LOJA: 52
	<l></l>
	<sl>1</sl>
	<gui></gui>
	<l></l>
";

                    printer.SetMarginLeft(0);
                    printer.SetFontB();
                    printer.SetAlignLeft();

                    var interpreter = new Interpreter(printer);
                    interpreter.ExecuteInterpretationForPrinter(xml);

                    printer.Reset();
                    printer.Sleep();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"{ex.Message} => {ex.StackTrace}");
            }
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

        static void TestePrinter()
        {
            int x = 0;
            try
            {
                for (int tentativa = 0; tentativa < 1; tentativa++)
                    using (IPrinter printer = new Printer(5))
                    {
                        printer.WakeUp();
                        printer.WriteLineSleepTimeMs = 200;
                        printer.Reset();
                      
                        printer.SetMarginLeft(0);
                        printer.SetMarginLeft(0);
                        printer.SetFontB();
                        printer.SetAlignLeft();

                        //printer.PrintBarcode(BarcodeType.code128C, "12345678901234567890123456789012345678901234");
                        printer.PrintImage(@"C:\Cosmos\NFCe\Logo\pmenos.bmp", MmToDots(180, 26), MmToDots(180, 72));
                        printer.WriteLine("LINHA1");
                        printer.HorizontalLine(180, 72, true);
                        printer.WriteLine("LINHA2");
                        //printer.HorizontalLine(180, 72, true);
                        //printer.WriteLine("LINHA3");
                        //printer.HorizontalLine(180, 72, true);
                        //printer.WriteLine("LINHA4");
                        printer.LineFeed(3);
                        printer.Guillotine();                       

                        PrinterStatus ps = new PrinterStatus();
                        int contador = 1;
                        while (contador-- > 0)
                        {

                            System.Console.WriteLine("");
                            System.Console.WriteLine("OBTENDO NOVA LEITURA:");
                            var ret = printer.GetPrinterModel();
                            System.Console.WriteLine($"Nome da impressora {ret}");
                            for (int j = 1; j < 5; j++)
                            {
                                var bt = (printer as Printer).GetPrinterStatus((StatusType)j);
                                switch ((StatusType)j)
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

                            }
                            System.Console.WriteLine($"SENSOR SEM PAPEL: {ps.SemPapel}");
                            System.Console.WriteLine($"SENSOR POUCO PAPEL: {ps.PoucoPapel}");
                            System.Console.WriteLine($"SENSOR TAMPA ABERTA: {ps.TampaAberta}");
                            System.Console.WriteLine($"SENSOR ERRO: {ps.EmErro}");
                            System.Console.WriteLine($"SENSOR GAVETA: {ps.SinalGaveta}");


                            System.Threading.Thread.Sleep(100);
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
                System.Threading.Thread.Sleep(200);
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
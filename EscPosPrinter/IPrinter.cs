using EscPosPrinter.PortFactory.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace EscPosPrinter
{
    public interface IPrinter : IDisposable
    {
        Dictionary<char, byte> MapSpecialCharacter { get; set; }
        int PictureLineSleepTimeMs { get; set; }
        int WriteLineSleepTimeMs { get; set; }
        string LocalEncoding { get; set; }
        void WriteLine(string text);
        void WriteLine(string text, byte style);
        void WriteLine(string text, PrintingStyle style);
        void WriteLineBold(string text);
        void WriteLineBig(string text);
        void WriteLineInvert(string text);
        void BoldOn();
        void BoldOff();
        void SetBigOn();
        void SetBifOff();
        void SetInversionOn();
        void SetInversionOff();
        void SetStyleOn(byte style, ref byte underlineHeight);
        void SetStyleOff(ref byte underlineHeight);
        void WhiteOnBlackOn();
        void SetMarginLeft(byte value);
        void WhiteOnBlackOff();
        void SetSize(bool doubleWidth, bool doubleHeight);
        void LineFeed();
        void LineFeed(byte lines);
        void Indent(byte columns);
        void SetLineSpacing(byte lineSpacing);
        void SetAlignLeft();
        void SetAlignCenter();
        void SetAlignRight();
        void HorizontalLine(string nameprinter);
        void HorizontalLine(int dpi, double largura, bool fontSmall);
        void ExecuteActions(IList<Action> actionsForPrinter);
        void Reset();
        void PrintBarcode(BarcodeType type, string data);
        void SetLargeBarcode(int large);
        void SetBarcodeLeftSpace(byte spacingDots);
        void PrintImage(string image, int maxWidth = 350, Bitmap bpm = null);
        void SetPrintingParameters(byte maxPrinting, byte heatingTime, byte heatingInterval);
        void Sleep();
        void WakeUp();
        void FeedDots(byte dotsToFeed);
        void WriteToBuffer(string text);
        void Guillotine();
        void SetLetterSpacing(byte letterSpacing);
        void PrintQrCode(string data);
        void SetHeigthBarcode(int heigth);
        void SetFontSize(byte height = 1);
        void SetEspaceBetweenLines(byte space = 0);
        void SetMarginLeft(byte value = 10, byte margin = 2);
        void SetColumn(byte column, byte value);
        void NextTab();
        void PageModeOn();
        void PageModeOff();
        void PrintPageMode();        
        void SetModePageArea(double x, double y, double width, double height, int dpi = 203);
        void setConfigurationInitial(IPrinter printer, string nameprinter = "sweda");
        string GetPrinterModel();

        void SetEncodingPtBR();
        void SetEncoding(byte v);
        void SetFontA();
        void SetFontB();
    }
}
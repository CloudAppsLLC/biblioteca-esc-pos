using BibliotecaImpressaoEscPos.PortFactory.Enums;
using System;
using System.Drawing;

namespace BibliotecaImpressaoEscPos
{
    public interface IPrinter : IDisposable
    {
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
        void WhiteOnBlackOff();
        void SetSize(bool doubleWidth, bool doubleHeight);
        void LineFeed();
        void LineFeed(byte lines);
        void Indent(byte columns);
        void SetLineSpacing(byte lineSpacing);
        void SetAlignLeft();
        void SetAlignCenter();
        void SetAlignRight();
        void HorizontalLine(int length);
        void Reset();
        void PrintBarcode(BarcodeType type, string data);
        void SetLargeBarcode(bool large);
        void SetBarcodeLeftSpace(byte spacingDots);
        void PrintImage(string fileName);
        void PrintImage(Bitmap image);
        void SetPrintingParameters(byte maxPrinting, byte heatingTime, byte heatingInterval);
        void Sleep();
        void WakeUp();
        void FeedDots(byte dotsToFeed);
        void WriteToBuffer(string text);
        void Guillotine();
    }
}
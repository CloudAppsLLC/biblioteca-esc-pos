namespace EscPosPrinter.PortFactory.Enums
{
    public enum PrintingStyle
    {
        Reverse = 1 << 1,
        Updown = 1 << 2,
        Bold = 1 << 3,
        DoubleHeight = 1 << 4,
        DoubleWidth = 1 << 5,
        DeleteLine = 1 << 6,
        Underline = 1 << 0,
        ThickUnderline = 1 << 7
    }
}

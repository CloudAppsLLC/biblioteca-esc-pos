using EscPosPrinter.Builder;
using System;
using System.Collections.Generic;

namespace EscPosPrinter
{
    public class Interpreter : IInterpreter
    {
        private IPrinter Printer;

        public Interpreter(IPrinter printer)
        {
            Printer = printer;
        }

        public void ExecuteInterpretationForPrinter(string text)
        {
            var elements = XmlLoader.Load(text);
            var commands = Transpilator.TranspileElements(elements);

            Printer.WakeUp();

            foreach (var command in commands)
            {
                if (!string.IsNullOrEmpty(command.Tag))
                {
                    switch (command.Tag)
                    {
                        case "ad":
                            Printer.SetAlignRight();
                            Printer.WriteLine(command.Value);
                            break;
                        case "/ad":
                            Printer.SetAlignLeft();
                            break;
                        case "s":
                            Printer.SetUnderline(2);
                            Printer.WriteToBuffer(command.Value);
                            break;
                        case "/s":
                            Printer.SetUnderline(0);
                            break;

                        case "b":
                            Printer.BoldOn();
                            Printer.WriteToBuffer(" " + command.Value);
                            break;
                        case "/b":
                            Printer.BoldOff();
                            break;

                        case "c":
                            Printer.WriteToBuffer(command.Value + " ");
                            break;
                        case "/c":
                            break;

                        case "ce":
                            Printer.SetAlignCenter();
                            Printer.WriteToBuffer(command.Value);
                            break;
                        case "/ce":
                            Printer.SetAlignLeft();
                            break;

                        case "l":
                            Printer.LineFeed();
                            break;
                        case "/l":
                            break;

                        case "sl":
                            Printer.LineFeed(byte.Parse(command.Value));
                            break;
                        case "/sl":
                            break;

                        case "gui":
                            Printer.LineFeed();
                            Printer.Guillotine();
                            break;
                        case "/gui":
                            break;

                        default:
                            Printer.WriteToBuffer(command.Value);
                            break;
                    }
                }
                else
                {
                    Printer.WriteToBuffer(command.Value);
                }

                Printer.SetAlignLeft();
            }
        }
    }
}
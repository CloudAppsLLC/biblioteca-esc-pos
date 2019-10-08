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

        public IList<Action> GenerateActionsForPrinter(string text)
        {
            var actions = new List<Action>();
            try
            {
                var elements = XmlLoader.Load(text);
                var commands = Transpilator.TranspileElements(elements);
                
                foreach (var command in commands)
                {
                    if (!string.IsNullOrEmpty(command.Tag))
                    {
                        switch (command.Tag)
                        {
                            case "ad":
                                actions.Add(() => Printer.SetAlignRight());
                                actions.Add(() => Printer.WriteLine(command.Value));
                                break;
                            case "/ad":
                                actions.Add(() => Printer.SetAlignLeft());
                                break;

                            case "b":
                                actions.Add(() => Printer.BoldOn());
                                actions.Add(() => Printer.WriteToBuffer(" " + command.Value));
                                break;
                            case "/b":
                                actions.Add(() => Printer.BoldOff());
                                break;

                            case "c":
                                actions.Add(() => Printer.WriteToBuffer(command.Value + " "));
                                break;
                            case "/c":
                                break;

                            case "ce":
                                actions.Add(() => Printer.SetAlignCenter());
                                actions.Add(() => Printer.WriteLine(command.Value));
                                break;
                            case "/ce":
                                actions.Add(() => Printer.SetAlignLeft());
                                break;

                            case "l":
                                actions.Add(() => Printer.LineFeed());
                                break;
                            case "/l":
                                break;

                            case "sl":
                                actions.Add(() => Printer.LineFeed(byte.Parse(command.Value)));
                                break;
                            case "/sl":
                                break;

                            case "gui":
                                actions.Add(() => Printer.Guillotine());
                                break;
                            case "/gui":
                                break;

                            default:
                                actions.Add(() => Printer.WriteToBuffer(command.Value));
                                break;
                        }
                    }
                    else
                    {
                        actions.Add(() => Printer.WriteToBuffer(command.Value));
                    }
                }

                return actions;
            }
            catch
            {
                return null;
            }
        }
    }
}
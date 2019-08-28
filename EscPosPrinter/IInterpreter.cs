using System;
using System.Collections.Generic;

namespace EscPosPrinter
{
    public interface IInterpreter
    {
        IList<Action> GenerateActionsForPrinter(string text);
    }
}

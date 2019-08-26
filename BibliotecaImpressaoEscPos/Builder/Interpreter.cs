using System.Collections.Generic;
using System.Xml.XPath;

namespace BibliotecaImpressaoEscPos.Builder
{
    public static class Interpreter
    {
        private static IList<string> commands;

        static Interpreter()
        {
            commands = new List<string>();
        }

        public static IList<string> InterpreteElements(XPathNodeIterator elements)
        {
            foreach (XPathNavigator child in elements)
            {
                if (string.IsNullOrEmpty(child.Name))
                {
                    commands.Add(child.Value);
                }
                else
                {
                    commands.Add($"<{child.Name}>");
                    InterpreteElements(child.SelectChildren(XPathNodeType.All));
                    commands.Add($"</{child.Name}>");
                }
            }

            return commands;
        }
    }
}

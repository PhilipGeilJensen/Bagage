using System;
using System.Collections.Generic;
using System.Text;

namespace Luggage
{
    /// <summary>
    /// Class that holds the messages and also print out the latest
    /// </summary>
    class TextOutput
    {
        List<string> text = new List<string>();
        /// <summary>
        /// Print out the latest lines
        /// </summary>
        public void PrintLines()
        {
            for (int i = 0; i < 6; i++)
            {
                Console.SetCursorPosition(0, 20 + i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            int line = 0;
            if (text.Count >= 5)
            {
                for (int i = text.Count - 6; i < text.Count; i++)
                {
                    if (text.Count > 0 && i >= 0)
                    {
                        Console.SetCursorPosition(0, 20 + line);
                        Console.WriteLine(text[i]);
                        line++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < text.Count; i++)
                {
                    Console.SetCursorPosition(0, 20 + line);
                    Console.WriteLine(text[i]);
                    line++;
                }
            }
        }
        /// <summary>
        /// Add a message line to the list
        /// </summary>
        /// <param name="line"></param>
        public void AddLines(string line)
        {
            text.Add(line);
        }
    }
}

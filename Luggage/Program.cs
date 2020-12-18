using System;
using System.Collections.Generic;
using System.Threading;

namespace Luggage
{
    class Program
    {
        static LostAndFound laf = new LostAndFound();
        static Sorting s = new Sorting();
        static List<Counter> c = new List<Counter>() { new Counter(1, 1, s), new Counter(2, 2, s), new Counter(3, 3, s) };
        static List<Terminal> t = new List<Terminal>() { new Terminal(1, laf, s), new Terminal(2, laf, s), new Terminal(3, laf, s) };
        static List<string> destinations = new List<string>() { "Rome", "Barcelona", "Athen", "Alanya", "Madrid", "Fuengirola", "Lissabon", "Venedig", "Paris", "Berlin", "Los Angeles", "New York", "Rio" };
        static TextOutput to = new TextOutput();
        static Random rand = new Random();
        static int plainId = 1;
        static void Main(string[] args)
        {
            Thread template = new Thread(PrintTemplate);
            Thread lc = new Thread(LuggageCounter);
            Thread ct = new Thread(CounterTerminal);
            template.Start();
            lc.Start();
            ct.Start();

            for (int i = 0; i < 3; i++)
            {
                ThreadPool.QueueUserWorkItem(Plain);
                Thread.Sleep(rand.Next(0, 30000));
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Method for the Thread that handles the creation of luggage and adding it through the right counter
        /// </summary>
        static void LuggageCounter()
        {
            int luggageId = 0;
            while (true)
            {
                List<int> openCounters = new List<int>();
                foreach (Counter counter in c)
                {
                    if (counter.CounterOpen)
                    {
                        openCounters.Add(counter.Id);
                    }
                }
                if (openCounters.Count > 0)
                {
                    Thread.Sleep(1000);
                    Luggage l = new Luggage(luggageId, openCounters[rand.Next(0, openCounters.Count)]);
                    foreach (Counter counter in c)
                    {
                        if (counter.Terminal == l.Terminal)
                        {
                            l.Destination = counter.Destination;
                            counter.SendLuggageToSorting(l);
                        }
                    }
                    luggageId++;
                }
            }
        }

        /// <summary>
        /// Method for the Thread that transfers the luggage from the sorting to the terminal
        /// </summary>
        static void CounterTerminal()
        {
            while (true)
            {
                Thread.Sleep(1000);
                Luggage l = s.Peek();
                foreach (Terminal terminal in t)
                {
                    if (l.Terminal == terminal.Id)
                    {
                        terminal.AddLuggage();
                    }
                }
            }
        }

        /// <summary>
        /// Method for Threads handling the plains
        /// </summary>
        /// <param name="callback"></param>
        static void Plain(object callback)
        {
            while (true)
            {
                Terminal te = null;
                foreach (Terminal terminal in t)
                {
                    if (terminal.TerminalClosed && te == null)
                    {
                        te = terminal;
                        te.TerminalClosed = false;
                    }
                }
                Plain p = new Plain(plainId, te.Id, destinations[rand.Next(0, destinations.Count)]);
                plainId++;

                Counter co = null;

                foreach (Counter counter in c)
                {
                    if (!counter.CounterOpen && co == null)
                    {
                        co = counter;
                        counter.CounterOpen = true;
                        counter.Destination = p.Destination;
                    }
                }
                Thread.Sleep(rand.Next(10000, 30000));
                to.AddLines("Plain to " + p.Destination + " at terminal " + p.Terminal + " has arrived");
                te.Plain = p;
                te.PlainArrived = true;
                to.AddLines("Luggage is being loaded to plain " + p.Id);
                while (te.luggage.Count != 0)
                {
                    Thread.Sleep(500);
                    Luggage l = te.DequeueLuggage();
                    if (l.Destination == p.Destination)
                    {
                        p.EnqueueLuggage(l);
                    } else
                    {
                        laf.AddLuggage(l);
                    }
                }
                to.AddLines("Luggage has been loaded");
                to.AddLines("Ready for takeoff with " + p.luggage.Count + " bags");
                te.TerminalClosed = true;
                te.PlainArrived = false;
                co.CounterOpen = false;
                co.Destination = "";
                Console.Clear();
                Thread.Sleep(rand.Next(10000, 30000));
            }
        }
        /// <summary>
        /// Prints the template for the program every 500 milisecond
        /// </summary>
        static void PrintTemplate()
        {
            while (true)
            {
                Thread.Sleep(500);
                foreach (Counter counter in c)
                {
                    if (!counter.CounterOpen)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {

                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    int index = c.IndexOf(counter);
                    Console.SetCursorPosition(0, index * 5);
                    Console.WriteLine("Counter {0}", counter.Id);
                    Console.SetCursorPosition(0, index * 5 + 1);
                    Console.WriteLine("Destination {0}", counter.Destination);
                }

                foreach (Terminal terminal in t)
                {
                    if (terminal.TerminalClosed)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    int index = t.IndexOf(terminal);
                    Console.SetCursorPosition(70, index * 5);
                    Console.WriteLine("Terminal {0}", terminal.Id);
                    Console.SetCursorPosition(70, index * 5 + 1);
                    Console.WriteLine("Luggage amount: {0}", AddZero(terminal.luggage.Count));
                    Console.ForegroundColor = ConsoleColor.White;


                    if (terminal.PlainArrived)
                    {
                        Console.SetCursorPosition(95, index * 5);
                        Console.WriteLine(" ------------ ");
                        Console.SetCursorPosition(95, index * 5 + 1);
                        Console.WriteLine("| Plain {0}   |", AddZero(terminal.Plain.Id));
                        Console.SetCursorPosition(95, index * 5 + 2);
                        Console.WriteLine("|     {0}     |", AddZero(terminal.Plain.luggage.Count));
                        Console.SetCursorPosition(95, index * 5 + 3);
                        Console.WriteLine(" ------------ "); 
                    }
                }

                Console.SetCursorPosition(35, 0);
                Console.WriteLine("Sorting");
                Console.SetCursorPosition(35, 1);
                Console.WriteLine("Luggage amount: {0}", AddZero(s.luggage.Count));



                Console.SetCursorPosition(35, 4);
                Console.WriteLine("Lost and found");
                Console.SetCursorPosition(35, 5);
                Console.WriteLine("Luggage amount: {0}", AddZero(laf.luggage.Count));

                to.PrintLines();
            }
        }

        static string AddZero(int num)
        {
            if (num < 10)
            {
                return "0" + num.ToString();
            }
            else
            {
                return num.ToString();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Luggage
{
    class Terminal
    {
        // Max size 20
        public Queue<Luggage> luggage = new Queue<Luggage>();
        public int Id { get; set; }
        public bool PlainArrived { get; set; }
        public bool TerminalClosed { get; set; }
        public Plain Plain { get; set; }
        LostAndFound laf;
        Sorting s;

        public Terminal(int id, LostAndFound lost, Sorting sorting)
        {
            Id = id;
            PlainArrived = false;
            TerminalClosed = true;
            laf = lost;
            s = sorting;
        }
        /// <summary>
        /// Adds the luggage to the terminal queue
        /// </summary>
        public void AddLuggage()
        {
            if (!TerminalClosed)
            {
                lock (luggage)
                {
                    while (luggage.Count == 20)
                    {
                        Console.Clear();
                        Monitor.Wait(luggage);
                    }
                    luggage.Enqueue(s.DequeueLuggage());
                    Monitor.PulseAll(luggage);
                } 
            } else
            {
                laf.AddLuggage(s.DequeueLuggage());
            }
        }
        /// <summary>
        /// Returns the next piece of luggage in the queue
        /// </summary>
        /// <returns></returns>
        public Luggage DequeueLuggage()
        {
            lock(luggage)
            {
                while (luggage.Count == 0)
                {
                    Monitor.Wait(luggage);
                }
                Monitor.PulseAll(luggage);
                return luggage.Dequeue();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Luggage
{
    class Counter
    {
        Sorting s;
        public int Id { get; set; }
        public int Terminal { get; set; }
        public bool CounterOpen { get; set; }
        public string Destination { get; set; }

        public Counter(int id, int terminal, Sorting s)
        {
            Id = id;
            Terminal = terminal;
            this.s = s;
            CounterOpen = false;
        }
        /// <summary>
        /// Sends the luggage to the sorting
        /// </summary>
        /// <param name="l"></param>
        public void SendLuggageToSorting(Luggage l)
        {
            s.EnqueueLuggage(l);
        }
    }
}

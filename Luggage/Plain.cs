using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Luggage
{
    class Plain
    {
        // Max size = 20;
        public Queue<Luggage> luggage = new Queue<Luggage>();
        public int Id { get; set; }
        public int Terminal { get; set; }
        public string Destination { get; set; }

        public Plain(int id, int terminal, string dest)
        {
            Id = id;
            Terminal = terminal;
            Destination = dest;
        }

        /// <summary>
        /// Adds the luggage to the plain
        /// </summary>
        /// <param name="l"></param>
        public void EnqueueLuggage(Luggage l)
        {
            if (luggage.Count <= 20)
            {
                luggage.Enqueue(l);
            }
        }
    }
}

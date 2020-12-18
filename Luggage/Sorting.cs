using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Luggage
{
    class Sorting
    {
        // Max size 20
        public Queue<Luggage> luggage = new Queue<Luggage>();

        /// <summary>
        /// Adds the luggage to the sorting queue
        /// </summary>
        /// <param name="l"></param>
        public void EnqueueLuggage(Luggage l)
        {
            lock (luggage)
            {
                while (luggage.Count == 20)
                {
                    Monitor.Wait(luggage);
                }
                luggage.Enqueue(l);
                Monitor.PulseAll(luggage);
            }
        }
        /// <summary>
        /// Get the next piece of luggage from the sorting queue
        /// </summary>
        /// <returns></returns>
        public Luggage DequeueLuggage()
        {
            Luggage l;
            lock (luggage)
            {
                while (luggage.Count == 0)
                {
                    Monitor.Wait(luggage);
                }
                l = luggage.Dequeue();
                Monitor.PulseAll(luggage);
            }

            return l;
        }
        /// <summary>
        /// Shows the next piece of luggage in the sorting queue
        /// </summary>
        /// <returns></returns>
        public Luggage Peek()
        {
            lock (luggage)
            {
                while (luggage.Count == 0)
                {
                    Monitor.Wait(luggage);
                }
                Monitor.PulseAll(luggage);
                return luggage.Peek();
            }
        }
    }
}

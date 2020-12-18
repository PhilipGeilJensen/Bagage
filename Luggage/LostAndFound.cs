using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Luggage
{
    class LostAndFound
    {
        public List<Luggage> luggage = new List<Luggage>();

        public void AddLuggage(Luggage l)
        {
            luggage.Add(l);
        }

    }
}

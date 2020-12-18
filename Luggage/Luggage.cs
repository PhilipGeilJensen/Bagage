using System;
using System.Collections.Generic;
using System.Text;

namespace Luggage
{
    class Luggage
    {
        public int Id { get; set; }
        public int Terminal { get; set; }
        public string Destination { get; set; }

        public Luggage(int id, int terminal)
        {
            Id = id;
            Terminal = terminal;
        }
    }
}

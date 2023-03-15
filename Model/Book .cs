using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace redis.Model
{
     public class Book
    {
        public string Isbn { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int Rating { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileBiblioteca.Models
{
    public class Book
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string editorial { get; set; }
        public string author { get; set; }
        public int categoryID { get; set; }
        public object category { get; set; }
        public int quantity { get; set; }
    }
}

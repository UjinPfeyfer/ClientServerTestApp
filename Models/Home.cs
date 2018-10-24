using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientServerTestApp.Models
{
    public class Home
    {
        public IList<int> SelectedItems { get; set; }
        public IList<Category> Categories { get; set; }
        public IList<Product> Products { get; set; }

        public Home()
        {
            SelectedItems = new List<int>();
            Categories = new List<Category>();
            Products = new List<Product>();
        }
    }
}

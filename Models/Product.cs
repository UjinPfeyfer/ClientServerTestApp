using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ClientServerTestApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public int Count { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public override string ToString()
        {
            return Name + ";" + Price + ";" + Count + ";" + Description + ";" + CategoryId + Environment.NewLine;
        }
    }

    
}

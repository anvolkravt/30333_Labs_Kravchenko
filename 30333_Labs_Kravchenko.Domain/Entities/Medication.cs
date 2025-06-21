using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _30333_Labs_Kravchenko.Domain.Entities
{
    public class Medication : Entity
    {
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string? Image { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}

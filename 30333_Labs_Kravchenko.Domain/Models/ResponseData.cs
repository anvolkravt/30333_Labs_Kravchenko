using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _30333_Labs_Kravchenko.Domain.Models
{
    public class ResponseData<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string? ErrorMessage { get; set; }
    }
}

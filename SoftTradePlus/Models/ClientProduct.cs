using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftTradePlus.Models
{
    public class ClientProduct
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public Client Client  { get; set; }
    }
}

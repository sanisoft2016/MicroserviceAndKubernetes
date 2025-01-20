﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Model
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        //public ProductModel? Product { get; set; } = null;
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now.ToUniversalTime();
    }
}

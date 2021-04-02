using System;
using System.Collections.Generic;

namespace SofTracerAPI.Models.Orders
{
    public class OrderItemWithNames
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
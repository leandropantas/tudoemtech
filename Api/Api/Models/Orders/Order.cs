using System;
using System.Collections.Generic;

namespace SofTracerAPI.Models.Orders
{
    public class Order
    {
        public string OrderId { get; set; }
        public string ClientId { get; set; }
        public List<OrderItem> Items { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
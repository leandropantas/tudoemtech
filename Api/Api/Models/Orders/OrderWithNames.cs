using System;
using System.Collections.Generic;

namespace SofTracerAPI.Models.Orders
{
    public class OrderWithNames
    {
        public string OrderId { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public List<OrderItemWithNames> Items { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
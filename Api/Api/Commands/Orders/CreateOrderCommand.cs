using System;
using System.Collections.Generic;

namespace SofTracerAPI.Commands.Orders
{
    public class CreateOrderCommand
    {

        public string OrderId { get; set; }
        public string ClientId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<CreateOrderItemCommand> Items { get; set; } 

    }
}
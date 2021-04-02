using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SofTracerAPI.Commands.Orders
{
    public class CreateOrderItemCommand
    {

        public string ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
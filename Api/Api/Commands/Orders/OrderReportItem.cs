using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SofTracerAPI.Commands.Orders
{
    public class OrderReportItem
    {

        public string OrderId { get; set; }
        public string ClientName { get; set; }
        public string ProductName { get; set; }
        public double ProductValue { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
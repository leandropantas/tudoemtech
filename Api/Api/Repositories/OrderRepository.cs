using MySql.Data.MySqlClient;
using SofTracerAPI.Commands.Orders;
using SofTracerAPI.Models.Clients;
using SofTracerAPI.Models.Orders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;

namespace SofTracerAPI.Repositories
{
    public class OrderRepository
    {
        private readonly MySqlConnection _context;

        public OrderRepository(MySqlConnection context)
        {
            _context = context;
        }

        #region Queries

        public bool FindIfExistsOrdersByClientId(string clientId)
        {
            StringBuilder sql = new StringBuilder("SELECT COUNT(*) FROM orders WHERE clientId=@clientId");
            MySqlCommand command = new MySqlCommand(sql.ToString(), _context);
            command.Parameters.Add("@clientId", MySqlDbType.VarChar).Value = clientId;

            return int.Parse(command.ExecuteScalar().ToString()) > 0;
        }

        public Order FindByOrderId(string orderId)
        {
            StringBuilder sql = new StringBuilder(GetQueryWithoutNames());
            sql.AppendLine("WHERE orderId=@orderId");
            MySqlCommand command = new MySqlCommand(sql.ToString(), _context);
            command.Parameters.Add("@orderId", MySqlDbType.VarChar).Value = orderId;

            Order order = null;
            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (order == null)
                    {
                        order = new Order();
                        PopulateOrderWithoutNames(reader, order);
                    }
                    OrderItem newItem = new OrderItem();
                    PopulateOrderItemWithoutNames(reader, newItem);
                    order.Items.Add(newItem);
                }
            }

            return order;
        }


        public List<Order> FindAll()
        {
            List<Order> orders = new List<Order>();
            MySqlCommand command = new MySqlCommand(GetQueryWithoutNames(), _context);

            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {

                    string orderId = reader["orderId"].ToString();
                    string clientId = reader["clientId"].ToString();
                    
                    Order order = orders.Find(item => item.ClientId == clientId && item.OrderId == orderId);

                    if (order == null)
                    {
                        order = new Order();
                       PopulateOrderWithoutNames(reader, order);
                        orders.Add(order);
                    }

                    OrderItem newItem = new OrderItem();
                    PopulateOrderItemWithoutNames(reader, newItem);
                    order.Items.Add(newItem);
                }
            }

            return orders;
        }

        private void PopulateOrderWithoutNames(IDataReader reader, Order order)
        {
            order.ClientId = reader["clientId"].ToString();
            order.OrderId = reader["orderId"].ToString();
            order.OrderDate = System.DateTime.Parse(reader["orderDate"].ToString()).Date;
            order.Items = new List<OrderItem>();
        }

        private void PopulateOrderItemWithoutNames(IDataReader reader, OrderItem order)
        {
            order.ProductId = reader["productId"].ToString();
            order.Quantity = int.Parse(reader["quantity"].ToString());
        }

        public List<OrderWithNames> FindAllWithNames()
        {
            List<OrderWithNames> orders = new List<OrderWithNames>();
            MySqlCommand command = new MySqlCommand(GetQueryWithNames(), _context);

            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {

                    string orderId = reader["orderId"].ToString();
                    string clientId = reader["clientId"].ToString();

                    OrderWithNames order = orders.Find(item => item.ClientId == clientId && item.OrderId == orderId);

                    if (order == null)
                    {
                        order = new OrderWithNames();
                        PopulateOrderWithNames(reader, order);
                        orders.Add(order);
                    }

                    OrderItemWithNames newItem = new OrderItemWithNames();
                    PopulateOrderItemWithNames(reader, newItem);
                    order.Items.Add(newItem);
                }
            }

            return orders;
        }

        public OrderWithNames FindByOrderIdWithNames(string orderId)
        {
            StringBuilder sql = new StringBuilder(GetQueryWithNames());
            sql.AppendLine("WHERE o.orderId=@orderId");
            MySqlCommand command = new MySqlCommand(sql.ToString(), _context);
            command.Parameters.Add("@orderId", MySqlDbType.VarChar).Value = orderId;

            OrderWithNames order = null;
            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (order == null)
                    {
                        order = new OrderWithNames();
                        PopulateOrderWithNames(reader, order);
                    }
                    OrderItemWithNames newItem = new OrderItemWithNames();
                    PopulateOrderItemWithNames(reader, newItem);
                    order.Items.Add(newItem);
                }
            }

            return order;
        }

        private void PopulateOrderWithNames(IDataReader reader, OrderWithNames order)
        {
            order.ClientId = reader["clientId"].ToString();
            order.ClientName = reader["clientName"].ToString();
            order.OrderId = reader["orderId"].ToString();
            order.OrderDate = System.DateTime.Parse(reader["orderDate"].ToString()).Date;
            order.Items = new List<OrderItemWithNames>();
        }

        private void PopulateOrderItemWithNames(IDataReader reader, OrderItemWithNames order)
        {
            order.ProductId = reader["productId"].ToString();
            order.ProductName = reader["productName"].ToString();
            order.Quantity = int.Parse(reader["quantity"].ToString());
        }

        private string GetQueryWithoutNames()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT orderId");
            sql.AppendLine(",clientId");
            sql.AppendLine(",productId");
            sql.AppendLine(",orderDate");
            sql.AppendLine(",quantity");
            sql.AppendLine("FROM orders");
            return sql.ToString();
        }

        private string GetQueryWithNames()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT o.orderId");
            sql.AppendLine(",c.id AS clientId");
            sql.AppendLine(",c.name AS clientName");
            sql.AppendLine(",p.id AS productId");
            sql.AppendLine(",p.name AS productName");
            sql.AppendLine(",o.orderDate");
            sql.AppendLine(",o.quantity");
            sql.AppendLine("FROM orders o");
            sql.AppendLine("LEFT JOIN products p");
            sql.AppendLine("ON p.id=o.productId");
            sql.AppendLine("LEFT JOIN clients c");
            sql.AppendLine("ON c.id=o.clientId");
            return sql.ToString();
        }

        #endregion Queries

        #region CreateOrder

        public void Create(CreateOrderCommand model)
        {
            foreach(CreateOrderItemCommand item in model.Items)
            {
                MySqlCommand command = _context.CreateCommand();
                command.CommandText = GetCreateOrderCommandText();
                command.Parameters.Add("@orderId", MySqlDbType.VarChar).Value = model.OrderId;
                command.Parameters.Add("@clientId", MySqlDbType.VarChar).Value = model.ClientId;
                command.Parameters.Add("@orderDate", MySqlDbType.Date).Value = model.OrderDate.Date;
                command.Parameters.Add("@productId", MySqlDbType.VarChar).Value = item.ProductId;
                command.Parameters.Add("@quantity", MySqlDbType.Int32).Value = item.Quantity;
                command.ExecuteNonQuery();
            }

        }

        static private string GetCreateOrderCommandText()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO orders (").AppendLine();
            sql.AppendLine("orderId").AppendLine();
            sql.AppendLine(",clientId").AppendLine();
            sql.AppendLine(",productId").AppendLine();
            sql.AppendLine(",orderDate").AppendLine();
            sql.AppendLine(",quantity)").AppendLine();
            sql.AppendLine("VALUES (").AppendLine();
            sql.AppendLine("@orderId");
            sql.AppendLine(",@clientId");
            sql.AppendLine(",@productId");
            sql.AppendLine(",@orderDate");
            sql.AppendLine(",@quantity)");
            return sql.ToString();
        }

        #endregion CreateOrder

        #region DeleteOrder

        public void DeleteByOrderId(string orderId)
        {
            MySqlCommand command = _context.CreateCommand();
            StringBuilder query = new StringBuilder();
            query.AppendLine("DELETE FROM orders");
            query.AppendLine("WHERE orderId=@orderId");
            command.CommandText = query.ToString();
            command.Parameters.Add("@orderId", MySqlDbType.VarChar).Value = orderId;
            command.ExecuteNonQuery();
        }

        public void DeleteByUserId(string clientId)
        {
            MySqlCommand command = _context.CreateCommand();
            StringBuilder query = new StringBuilder();
            query.AppendLine("DELETE FROM orders");
            query.AppendLine("WHERE clientId=@clientId");
            command.CommandText = query.ToString();
            command.Parameters.Add("@clientId", MySqlDbType.VarChar).Value = clientId;
            command.ExecuteNonQuery();
        }

        #endregion DeleteOrder

        #region CreateCSV

        public bool CreateCsv(string clientId)
        {

            ClientRepository clientRepository = new ClientRepository(_context);
            Client client = clientRepository.FindById(clientId);

            string docPath = ConfigurationManager.AppSettings["CsvPath"];
            docPath = string.Concat(docPath, client.Name.Trim(), ".csv");


            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT o.orderId");
            sql.AppendLine(",c.name AS clientName");
            sql.AppendLine(",p.name AS productName");
            sql.AppendLine(",p.value AS productValue");
            sql.AppendLine(",o.orderDate");
            sql.AppendLine(",o.quantity");
            sql.AppendLine(",(o.quantity * p.value) AS total");
            sql.AppendLine("FROM orders o");
            sql.AppendLine("LEFT JOIN products p");
            sql.AppendLine("ON p.id=o.productId");
            sql.AppendLine("LEFT JOIN clients c");
            sql.AppendLine("ON c.id=o.clientId");
            sql.AppendLine("WHERE o.clientId=@clientId");
            MySqlCommand command = new MySqlCommand(sql.ToString(), _context);
            command.Parameters.Add("@clientId", MySqlDbType.VarChar).Value = clientId;

            List<OrderReportItem> reportItems = new List<OrderReportItem>();
            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    OrderReportItem item = new OrderReportItem();
                    PopulateOrderReportItem(reader, item);
                    reportItems.Add(item);
                }
            }

            StringBuilder csvBuilder = new StringBuilder();

            csvBuilder.AppendLine("Código Pedido;Nome Cliente; NomeProduto; Data Pedido; Quantidade Pedido; Valor Produto; Valor Total");
            
            foreach(OrderReportItem item in reportItems)
            {
                string formatedDate = item.OrderDate.ToString("dd/MM/yyyy");
                string formatedProductValue = item.ProductValue.ToString("N3");
                string formatedTotal = item.Total.ToString("N3");
                csvBuilder.AppendFormat("{0};{1};{2};{3};{4};{5};{6}", item.OrderId, item.ClientName, item.ProductName, formatedDate, item.Quantity, formatedProductValue, formatedTotal).AppendLine();
            }

            try
            {
                File.WriteAllText(docPath, csvBuilder.ToString());
            }
            catch (InvalidCastException err)
            {
                return false;
            }


            return true;
        }

        private void PopulateOrderReportItem(IDataReader reader, OrderReportItem orderReportItem)
        {
            orderReportItem.OrderId = reader["orderId"].ToString();
            orderReportItem.ClientName = reader["clientName"].ToString();
            orderReportItem.ProductName = reader["productName"].ToString();
            orderReportItem.OrderDate = DateTime.Parse(reader["orderDate"].ToString());
            orderReportItem.Quantity = int.Parse(reader["quantity"].ToString());
            orderReportItem.Total = double.Parse(reader["total"].ToString());
        }

        #endregion CreateCSV

    }
}
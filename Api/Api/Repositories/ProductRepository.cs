using MySql.Data.MySqlClient;
using SofTracerAPI.Commands.Products;
using SofTracerAPI.Models.Products;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SofTracerAPI.Repositories
{
    public class ProductRepository
    {
        private readonly MySqlConnection _context;

        public ProductRepository(MySqlConnection context)
        {
            _context = context;
        }

        #region Queries

        public List<Product> FindAll()
        {
            List<Product> products = new List<Product>();
            MySqlCommand command = new MySqlCommand(GetQuery(), _context);
            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Product product = new Product();
                    PopulateProduct(reader, product);
                    products.Add(product);
                }
            }

            return products;
        }

        public Product FindById(string productId)
        {
            StringBuilder sql = new StringBuilder(GetQuery());
            sql.AppendLine("WHERE id=@id");
            MySqlCommand command = new MySqlCommand(sql.ToString(), _context);
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = productId;

            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Product product = new Product();
                    PopulateProduct(reader, product);
                    return product;
                }
            }

            return null;
        }

        private string GetQuery()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT id");
            sql.AppendLine(",name");
            sql.AppendLine(",value");
            sql.AppendLine("FROM products");
            return sql.ToString();
        }

        private void PopulateProduct(IDataReader reader, Product product)
        {
            product.Id = reader["id"].ToString();
            product.Name = reader["name"].ToString();
            product.Value = double.Parse(reader["value"].ToString());
        }

        #endregion Queries

        #region CreateProduct

        public void Create(CreateProductCommand model)
        {
            MySqlCommand command = _context.CreateCommand();
            command.CommandText = GetCreateProductCommandText();
            CreateProductPopulateParameters(model, command);
            command.ExecuteNonQuery();
        }

        private static void CreateProductPopulateParameters(CreateProductCommand model, MySqlCommand command)
        {
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = model.Id;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = model.Name;
            command.Parameters.Add("@value", MySqlDbType.Double).Value = model.Value;
        }

        static private string GetCreateProductCommandText()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO products (").AppendLine();
            sql.AppendLine("id").AppendLine();
            sql.AppendLine(",name").AppendLine();
            sql.AppendLine(",value)").AppendLine();
            sql.AppendLine("VALUES (").AppendLine();
            sql.AppendLine("@id");
            sql.AppendLine(",@name");
            sql.AppendLine(",@value)");
            return sql.ToString();
        }

        #endregion CreateProduct

        #region UpdateProduct

        public void Update(UpdateProductCommand model)
        {
            MySqlCommand command = _context.CreateCommand();
            command.CommandText = GetUpdateProductCommandText();
            UpdateProductPopulateParameters(model, command);
            command.ExecuteNonQuery();
        }

        private static void UpdateProductPopulateParameters(UpdateProductCommand model, MySqlCommand command)
        {
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = model.Id;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = model.Name;
            command.Parameters.Add("@value", MySqlDbType.Double).Value = model.Value;
        }

        static private string GetUpdateProductCommandText()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE products").AppendLine();
            sql.AppendLine("SET name=@name").AppendLine();
            sql.AppendLine(",value=@value").AppendLine();
            sql.AppendLine("WHERE id=@id");
            return sql.ToString();
        }

        #endregion UpdateProduct

        #region DeleteProduct

        public void Delete(string productId)
        {
            MySqlCommand command = _context.CreateCommand();
            StringBuilder query = new StringBuilder();
            query.AppendLine("DELETE FROM products");
            query.AppendLine("WHERE id=@id");
            command.CommandText = query.ToString();
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = productId;
            command.ExecuteNonQuery();
        }

        #endregion DeleteProduct
    }
}
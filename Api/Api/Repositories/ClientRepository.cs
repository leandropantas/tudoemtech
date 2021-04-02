using MySql.Data.MySqlClient;
using SofTracerAPI.Commands.Clients;
using SofTracerAPI.Models.Clients;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SofTracerAPI.Repositories
{
    public class ClientRepository
    {
        private readonly MySqlConnection _context;

        public ClientRepository(MySqlConnection context)
        {
            _context = context;
        }

        #region Queries

        public List<Client> FindAll()
        {
            List<Client> clients = new List<Client>();
            MySqlCommand command = new MySqlCommand(GetQuery(), _context);
            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Client client = new Client();
                    PopulateClient(reader, client);
                    clients.Add(client);
                }
            }

            return clients;
        }

        public Client FindById(string clientId)
        {
            StringBuilder sql = new StringBuilder(GetQuery());
            sql.AppendLine("WHERE id=@id");
            MySqlCommand command = new MySqlCommand(sql.ToString(), _context);
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = clientId;

            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Client client = new Client();
                    PopulateClient(reader, client);
                    return client;
                }
            }

            return null;
        }

        public Client FindByEmail(string email)
        {
            StringBuilder sql = new StringBuilder(GetQuery());
            sql.AppendLine("WHERE email=@email");
            MySqlCommand command = new MySqlCommand(sql.ToString(), _context);
            command.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;

            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Client client = new Client();
                    PopulateClient(reader, client);
                    return client;
                }
            }

            return null;
        }

        private string GetQuery()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT id");
            sql.AppendLine(",name");
            sql.AppendLine(",email");
            sql.AppendLine("FROM clients");
            return sql.ToString();
        }

        private void PopulateClient(IDataReader reader, Client client)
        {
            client.Id = reader["id"].ToString();
            client.Name = reader["name"].ToString();
            client.Email = reader["email"].ToString();
        }

        #endregion Queries

        #region CreateClient

        public void Create(CreateClientCommand model)
        {
            MySqlCommand command = _context.CreateCommand();
            command.CommandText = GetCreateClientCommandText();
            CreateClientPopulateParameters(model, command);
            command.ExecuteNonQuery();
        }

        private static void CreateClientPopulateParameters(CreateClientCommand model, MySqlCommand command)
        {
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = model.Id;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = model.Name;
            command.Parameters.Add("@email", MySqlDbType.VarChar).Value = model.Email;
        }

        static private string GetCreateClientCommandText()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO clients (").AppendLine();
            sql.AppendLine("id").AppendLine();
            sql.AppendLine(",name").AppendLine();
            sql.AppendLine(",email)").AppendLine();
            sql.AppendLine("VALUES (").AppendLine();
            sql.AppendLine("@id");
            sql.AppendLine(",@name");
            sql.AppendLine(",@email)");
            return sql.ToString();
        }

        #endregion CreateClient

        #region UpdateClient

        public void Update(UpdateClientCommand model)
        {
            MySqlCommand command = _context.CreateCommand();
            command.CommandText = GetUpdateClientCommandText();
            UpdateClientPopulateParameters(model, command);
            command.ExecuteNonQuery();
        }

        private static void UpdateClientPopulateParameters(UpdateClientCommand model, MySqlCommand command)
        {
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = model.Id;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = model.Name;
            command.Parameters.Add("@email", MySqlDbType.VarChar).Value = model.Email;
        }

        static private string GetUpdateClientCommandText()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE clients").AppendLine();
            sql.AppendLine("SET name=@name").AppendLine();
            sql.AppendLine(",email=@email").AppendLine();
            sql.AppendLine("WHERE id=@id");
            return sql.ToString();
        }

        #endregion UpdateClient

        #region DeleteClient

        public void Delete(string clientId)
        {
            MySqlCommand command = _context.CreateCommand();
            StringBuilder query = new StringBuilder();
            query.AppendLine("DELETE FROM clients");
            query.AppendLine("WHERE id=@id");
            command.CommandText = query.ToString();
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = clientId;
            command.ExecuteNonQuery();
        }

        #endregion DeleteClient
    }
}
using MySql.Data.MySqlClient;
using System.Configuration;

namespace API.Misc
{
    public class Context
    {
        private readonly MySqlConnection _context = new MySqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ConnectionString);

        public Context()
        {
            _context.Open();
        }

        public MySqlConnection GetContext()
        {
            return _context;
        }

    }
}
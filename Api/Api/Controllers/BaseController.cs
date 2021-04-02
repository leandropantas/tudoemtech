using MySql.Data.MySqlClient;
using API.Misc;
using System;
using System.Web.Http;

namespace SofTracerAPI.Controllers
{
    public abstract class BaseController : ApiController, IDisposable
    {
        public MySqlConnection Context { get { return new Context().GetContext(); } }
        public DefaultMessages DefaultMessages { get { return new DefaultMessages(); } }

        new protected virtual void Dispose()
        {
            Context.Close();
        }
    }
}
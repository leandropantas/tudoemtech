using SofTracerAPI.Commands;
using SofTracerAPI.Controllers;
using SoftTracerAPI.Commands.Users;
using SoftTracerAPI.Repositories;
using SoftTracerAPI.Models;
using System.Web.Http;
using SoftTracerAPI.Misc;

namespace SoftTracerAPI.Controllers
{
    public class UsersController : BaseController
    {
  
        [HttpPost]
        public IHttpActionResult CreateUser([FromBody] CreateUserCommand command)
        {
            if (command == null) { return BadRequest(DefaultMessages.InvalidBody); }
            ValidationError error = new CreateUserCommandValidator().Validate(command);
            if (error.IsInvalid) { return BadRequest(error.Error); }
            UsersRepository repository = new UsersRepository(Connection);
            if (repository.UserExists(command.UserId)) { return BadRequest("Já existe um usuário com este nome."); }
            if (repository.EmailExists(command.Email)) { return BadRequest("Já existe um usuário com este e-mail."); }
            repository.Create(command);
            return Ok();
        }

        [HttpPost]
        [Route("~/api/users/authentication")]
        public IHttpActionResult AuthenticateUser([FromBody] FindAuthenticationCommand command)
        {
            if (command == null) { return BadRequest(DefaultMessages.InvalidBody); }
            ValidationError error = new FindTokenCommandValidator().Validate(command);
            if (error.IsInvalid) { return BadRequest(error.Error); }
            UsersRepository repository = new UsersRepository(Connection);
            Authentication authentication = repository.FindAuthentication(command);
            if (authentication == null) { return BadRequest("Usuário ou senha inválidos."); }
            return Ok(authentication);
        }

    }
}
using SofTracerAPI.Commands;
using SofTracerAPI.Commands.Clients;
using SofTracerAPI.Repositories;
using System.Web.Http;

namespace SofTracerAPI.Controllers
{
    public class ClientsController : BaseController
    {
        private readonly ClientRepository _repository;

        public ClientsController()
        {
            _repository = new ClientRepository(Context);
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] CreateClientCommand command)
        {
            if (command == null) { return BadRequest(DefaultMessages.InvalidBody); }
            ValidationError error = new CreateClientCommandValidation().Validate(command);
            if (error.IsInvalid) { return BadRequest(error.Error); }

            if (_repository.FindById(command.Id) != null) { return BadRequest("Já existe um Cliente com este código."); }
            if (_repository.FindByEmail(command.Email) != null) { return BadRequest("Já existe um Cliente com este e-mail."); }
            _repository.Create(command);
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult FindAll()
        {
            return Ok(_repository.FindAll());
        }

        [HttpGet]
        public IHttpActionResult FindAById([FromUri] string id)
        {
            if (string.IsNullOrEmpty(id)) { return BadRequest("Necessário informar um código de Cliente válido."); }

            if (_repository.FindById(id) == null) { return BadRequest("Não existe um Cliente com este código."); }
            return Ok(_repository.FindById(id));
        }

        [HttpPut]
        public IHttpActionResult Update([FromBody] UpdateClientCommand command)
        {
            if (command == null) { return BadRequest(DefaultMessages.InvalidBody); }
            ValidationError error = new UpdateClientCommandValidation().Validate(command);
            if (error.IsInvalid) { return BadRequest(error.Error); }

            if (_repository.FindById(command.Id) == null) { return BadRequest("Não existe um Cliente com este código."); }
            if (_repository.FindByEmail(command.Email) != null) { return BadRequest("Já existe um Cliente com este e-mail."); }
            _repository.Update(command);
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete([FromUri] string id)
        {
            if (string.IsNullOrEmpty(id)) { return BadRequest("Necessário informar um código de Cliente válido."); }

            if (_repository.FindById(id) == null) { return BadRequest("Não existe um Cliente com este código."); }
            _repository.Delete(id);
            return Ok();
        }
    }
}
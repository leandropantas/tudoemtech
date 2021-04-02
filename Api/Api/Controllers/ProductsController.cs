using SofTracerAPI.Commands;
using SofTracerAPI.Commands.Products;
using SofTracerAPI.Repositories;
using System.Web.Http;

namespace SofTracerAPI.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly ProductRepository _repository;

        public ProductsController()
        {
            _repository = new ProductRepository(Context);
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] CreateProductCommand command)
        {
            if (command == null) { return BadRequest(DefaultMessages.InvalidBody); }
            ValidationError error = new CreateProductCommandValidation().Validate(command);
            if (error.IsInvalid) { return BadRequest(error.Error); }

            if (_repository.FindById(command.Id) != null) { return BadRequest("Já existe um Produto com este código."); }
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
            if (string.IsNullOrEmpty(id)) { return BadRequest("Necessário informar um código de Produto válido."); }

            if (_repository.FindById(id) == null) { return BadRequest("Não existe um Produto com este código."); }
            return Ok(_repository.FindById(id));
        }

        [HttpPut]
        public IHttpActionResult Update([FromBody] UpdateProductCommand command)
        {
            if (command == null) { return BadRequest(DefaultMessages.InvalidBody); }
            ValidationError error = new UpdateProductCommandValidation().Validate(command);
            if (error.IsInvalid) { return BadRequest(error.Error); }

            if (_repository.FindById(command.Id) == null) { return BadRequest("Não existe um Produto com este código."); }
            _repository.Update(command);
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete([FromUri] string id)
        {
            if (string.IsNullOrEmpty(id)) { return BadRequest("Necessário informar um código de Produto válido."); }

            if (_repository.FindById(id) == null) { return BadRequest("Não existe um Produto com este código."); }
            _repository.Delete(id);
            return Ok();
        }
    }
}
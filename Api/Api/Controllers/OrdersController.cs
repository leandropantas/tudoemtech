using SofTracerAPI.Commands;
using SofTracerAPI.Commands.Orders;
using SofTracerAPI.Repositories;
using System.Web.Http;

namespace SofTracerAPI.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly OrderRepository _repository;

        public OrdersController()
        {
            _repository = new OrderRepository(Context);
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] CreateOrderCommand command)
        {
            if (command == null) { return BadRequest(DefaultMessages.InvalidBody); }
            ValidationError error = new CreateOrderCommandValidation().Validate(command);
            if (error.IsInvalid) { return BadRequest(error.Error); }

            if (_repository.FindByOrderId(command.OrderId) != null) { return BadRequest("Já existe uma Order com este código."); }
            _repository.Create(command);
            return Ok();
        }

        [Route("~/api/orders/{orderId}")]
        [HttpGet]
        public IHttpActionResult FindByOrderId([FromUri] string orderId)
        {
            return Ok(_repository.FindByOrderId(orderId));
        }

        [Route("~/api/orders")]
        [HttpGet]
        public IHttpActionResult FindAll()
        {
            return Ok(_repository.FindAll());
        }

        [Route("~/api/orders/aditional-informations")]
        [HttpGet]
        public IHttpActionResult FindAllWithNames()
        {
            return Ok(_repository.FindAllWithNames());
        }

        [Route("~/api/orders/{orderId}/aditional-informations")]
        [HttpGet]
        public IHttpActionResult FindByOrderIdWithNames([FromUri] string orderId)
        {
            return Ok(_repository.FindByOrderIdWithNames(orderId));
        }

        [Route("~/api/clients/{clientId}/orders/report")]
        [HttpGet]
        public IHttpActionResult CreateOrdersCsvByClient([FromUri] string clientId)
        {
            if (string.IsNullOrEmpty(clientId)) { return BadRequest("Necessário informar um código de Cliente válido."); }

            if (!_repository.FindIfExistsOrdersByClientId(clientId)) { return BadRequest("Não existe um Ordens para este Cliente."); }

            return Ok(_repository.CreateCsv(clientId));
        }

        [HttpDelete]
        public IHttpActionResult DeleteByOrderId([FromUri] string id)
        {
            if (string.IsNullOrEmpty(id)) { return BadRequest("Necessário informar um código de Ordem válido."); }

            if (_repository.FindByOrderId(id) == null) { return BadRequest("Não existe uma Ordem com este código."); }
            _repository.DeleteByOrderId(id);
            return Ok();
        }
    }
}
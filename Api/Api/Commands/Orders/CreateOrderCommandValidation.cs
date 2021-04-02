namespace SofTracerAPI.Commands.Orders
{
    public class CreateOrderCommandValidation : BaseValidator, IValidator<CreateOrderCommand>
    {
        public ValidationError Validate(CreateOrderCommand command)
        {
            if (string.IsNullOrEmpty(command.OrderId) || command.OrderId.Contains(" ")) { _manager.AddError("código da Orderm inválido"); }
            if (command.OrderId.Trim().Length > 50 || command.OrderId.Length > 50) { _manager.AddError("código da Ordem não pode ser maior que 50 caracteres"); }
            if (string.IsNullOrEmpty(command.ClientId) || command.ClientId.Contains(" ")) { _manager.AddError("código do Cliente inválido"); }
            if (command.ClientId.Trim().Length > 50 || command.ClientId.Length > 50) { _manager.AddError("código do Cliente não pode ser maior que 50 caracteres"); }
            if (command.OrderDate == null) { _manager.AddError("data da Ordem Inválida"); }
            if (command.Items.Count <= 0) { _manager.AddError("Ordem deve ter ao menos um Produto"); }
            if (command.Items.Find(item => string.IsNullOrEmpty(command.OrderId) || command.OrderId.Contains(" ") || command.OrderId.Trim().Length > 50 || command.OrderId.Length > 50 || item.Quantity <= 0) != null) {_manager.AddError("Contém produtos inválidos");}
            return _manager.GetError();
        }
    }
}
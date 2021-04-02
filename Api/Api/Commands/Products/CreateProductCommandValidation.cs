namespace SofTracerAPI.Commands.Products
{
    public class CreateProductCommandValidation : BaseValidator, IValidator<CreateProductCommand>
    {
        public ValidationError Validate(CreateProductCommand command)
        {
            if (string.IsNullOrEmpty(command.Id) || command.Id.Contains(" ")) { _manager.AddError("código do Produto inválido"); }
            if (command.Id.Trim().Length > 50 || command.Id.Length > 50) { _manager.AddError("código do Produto não pode ser maior que 50 caracteres"); }
            if (string.IsNullOrEmpty(command.Name) || command.Name.Contains(" ")) { _manager.AddError("Nome do Produto inválido"); }
            if (command.Name.Trim().Length > 100 || command.Name.Length > 100) { _manager.AddError("Nome do produto não pode ser maior que 100 caracteres"); }
            if (command.Value <= 0) { _manager.AddError("Valor do Produto tem que ser maior que 0"); }
            return _manager.GetError();
        }
    }
}
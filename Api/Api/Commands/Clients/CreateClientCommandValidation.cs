namespace SofTracerAPI.Commands.Clients
{
    public class CreateClientCommandValidation : BaseValidator, IValidator<CreateClientCommand>
    {
        public ValidationError Validate(CreateClientCommand command)
        {
            if (string.IsNullOrEmpty(command.Id) || command.Id.Contains(" ")) { _manager.AddError("código do Cliente inválido"); }
            if (command.Id.Trim().Length > 50 || command.Id.Length > 50) { _manager.AddError("código do Cliente não pode ser maior que 50 caracteres"); }
            if (string.IsNullOrEmpty(command.Name) || command.Name.Contains(" ")) { _manager.AddError("Nome do Cliente inválido"); }
            if (command.Name.Trim().Length > 50 || command.Name.Length > 50) { _manager.AddError("Nome do Cliente não pode ser maior que 50 caracteres"); }
            if (string.IsNullOrEmpty(command.Email) || command.Email.Contains(" ")) { _manager.AddError("Email inválido"); }
            if (command.Email.Trim().Length > 100 || command.Email.Length > 100) { _manager.AddError("Email do Cliente não pode ser maior que 100 caracteres"); }
            return _manager.GetError();
        }
    }
}
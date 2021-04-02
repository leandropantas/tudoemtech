using SofTracerAPI.Commands;

namespace SoftTracerAPI.Commands.Users
{
    public class CreateUserCommandValidator : BaseValidator, IValidator<CreateUserCommand>
    {
        public ValidationError Validate(CreateUserCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.UserId)) { _manager.AddError("Usuário inválido"); }
            if (string.IsNullOrWhiteSpace(command.Password)) { _manager.AddError("Senha inválida"); }
            if (string.IsNullOrWhiteSpace(command.Email)
                || !$"{command.Email}".Contains("@")
                || !$"{command.Email}".Contains(".")
                || $"{command.Email}".Length < 4) { _manager.AddError("E-mail inválido"); }
            if (string.IsNullOrWhiteSpace(command.DisplayName)) { _manager.AddError("Nome inválido"); }
            if ($"{command.UserId}".Length > 100) { _manager.AddError("Usuário possui muitos caracteres"); }
            if ($"{command.Password}".Length > 100) { _manager.AddError("Senha possui muitos caracteres"); }
            if ($"{command.Email}".Length > 200) { _manager.AddError("E-mail possui muitos caracteres"); }
            if ($"{command.DisplayName}".Length > 250) { _manager.AddError("Nome possui muitos caracteres"); }
            return _manager.GetError();
        }
    }
}
using SofTracerAPI.Commands;

namespace SoftTracerAPI.Commands.Users
{
    public class FindTokenCommandValidator : BaseValidator, IValidator<FindAuthenticationCommand>
    {
        public ValidationError Validate(FindAuthenticationCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.UserId)) { _manager.AddError("Usuário vazio"); }
            if (string.IsNullOrWhiteSpace(command.Password)) { _manager.AddError("Senha vazia"); }
            if ($"{command.UserId}".Length > 100) { _manager.AddError("Usuário possui muitos caracteres"); }
            if ($"{command.Password}".Length > 100) { _manager.AddError("Senha possui muitos caracteres"); }
            return _manager.GetError();
        }
    }
}
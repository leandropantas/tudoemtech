namespace SofTracerAPI.Commands
{
    public interface IValidator<T>
    {
        ValidationError Validate(T command);
    }
}
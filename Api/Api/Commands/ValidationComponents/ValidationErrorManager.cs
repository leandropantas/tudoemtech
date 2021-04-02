using System.Collections.Generic;

namespace SofTracerAPI.Commands
{
    public class ValidationErrorManager
    {
        private readonly List<string> _errors;

        public ValidationErrorManager()
        {
            _errors = new List<string>();
        }

        public void AddError(string error)
        {
            _errors.Add(error);
        }

        public bool HasError(string error)
        {
            return _errors.Contains(error);
        }

        public ValidationError GetError()
        {
            return new ValidationError(_errors);
        }
    }
}
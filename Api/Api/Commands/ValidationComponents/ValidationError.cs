using System.Collections.Generic;

namespace SofTracerAPI.Commands
{
    public class ValidationError
    {
        public ValidationError(List<string> errors)
        {
            IsInvalid = errors.Count > 0;
            Errors = errors;
            Error = string.Join(", ", errors);
            if (IsInvalid) { Error = $"{Error}."; }
        }

        public bool IsInvalid { get; }

        public List<string> Errors { get; }

        public string Error { get; }
    }
}
namespace FitHub.Platform.Common.Exceptions
{
    public class ValidationRuleException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationRuleException(IDictionary<string, string[]> errors)
        {
            Errors = errors;
        }
    }
}

namespace FitHub.Platform.Common.Domain
{
    public class ValidationError
    {
        public required string PropertyName { get; set; }
        public required string ErrorMessage { get; set; }
    }
}

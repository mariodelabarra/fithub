namespace FitHub.Platform.Common.Domain
{
    public class PaginatedRequestIn
    {
        public string OrderBy { get; set; } = string.Empty;
        public string Filter { get; set; } = string.Empty;
        public int? Top { get; set; }
        public int? Skip { get; set; }
    }
}

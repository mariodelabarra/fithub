namespace FitHub.Platform.Workout.Domain
{
    public class PaginatedRequestIn
    {
        public string OrderBy { get; set; }
        public string Filter { get; set; }
        public int Top { get; set; }
        public int Skip { get; set; }
    }
}

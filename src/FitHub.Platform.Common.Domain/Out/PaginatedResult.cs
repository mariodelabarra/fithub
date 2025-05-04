namespace FitHub.Platform.Common.Domain
{
    public class PaginatedResult<TEntity> where TEntity : BaseEntity
    {
        public IEnumerable<TEntity> Items { get; set; }
        public int TotalCount { get; set; }
    }
}

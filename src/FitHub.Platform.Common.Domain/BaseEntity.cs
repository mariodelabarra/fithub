using MongoDB.Entities;

namespace FitHub.Platform.Common.Domain
{
    public abstract class BaseEntity
    {
        public string Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedOn { get; set; }
    }
}

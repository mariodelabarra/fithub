﻿using MongoDB.Entities;

namespace FitHub.Platform.Common.Domain
{
    public class BaseEntity : Entity, ICreatedOn, IModifiedOn
    {
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedOn { get; set; }
    }
}

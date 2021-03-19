using System;

namespace ZennoLab.Infrastructure
{
    public interface IEntity
    {
        int Id { get; set; }
        DateTime CreateDate { get; set; }
        DateTime? LastModifyDate { get; set; }
        DateTime? Removed { get; set; }
    }
}

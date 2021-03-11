using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZennoLab.Interfaces
{
    public interface IEntity
    {
        int Id { get; set; }
        DateTime CreateDate { get; set; }
        DateTime? LastModifyDate { get; set; }
        DateTime? Removed { get; set; }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZennoLab.Infrastructure
{
    public interface IImageSetsRepository
    {
        Task<IEnumerable<IImageSet>> SelectAll();

        Task Insert(IImageSet entity);
    }
}

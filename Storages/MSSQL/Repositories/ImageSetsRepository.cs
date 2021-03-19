using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZennoLab.Infrastructure;

namespace ZennoLab.Storages.MSSQL
{
    public class ImageSetsContext : DbContext
    {
        public DbSet<ImageSet> ImageSets { get; set; }

        public ImageSetsContext(DbContextOptions<ImageSetsContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }

    public class ImageSetsRepository : IImageSetsRepository
    {
        private readonly ImageSetsContext _context;

        public ImageSetsRepository(ImageSetsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IImageSet>> SelectAll()
        {
           return await _context.ImageSets
                    .Where(i => !i.Removed.HasValue)
                    .ToListAsync();
        }

        public async Task Insert(IImageSet entity)
        {
            var imageSet = entity as ImageSet ?? new ImageSet();
            _context.ImageSets.Add(imageSet);
            await _context.SaveChangesAsync();
        }
    }
}

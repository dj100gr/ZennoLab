using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ZennoLab.Models
{
    public class SqlContext : DbContext
    {
        public DbSet<ImageSet> ImageSets { get; set; }
        public SqlContext(DbContextOptions<SqlContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Website.Models.Context
{
    public class ContextNews : DbContext
    {
        public DbSet<News> News { get; set; }
        public ContextNews(DbContextOptions<ContextNews> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

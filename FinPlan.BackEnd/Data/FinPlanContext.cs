using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinPlan.BackEnd.Data
{
    public class FinPlanContext : DbContext
    {
        private readonly IConfiguration _cfg;
        public FinPlanContext(IConfiguration cfg)
        {
            _cfg = cfg;
        }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optsBuilder) => optsBuilder.UseNpgsql(_cfg.GetConnectionString("FinPlanDatabase"));
    }
}

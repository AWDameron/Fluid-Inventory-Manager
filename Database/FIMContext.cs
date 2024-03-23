using FIMS2.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace FIMS2.Database
{
    public class FIMSContext : DbContext
    {
        public FIMSContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "FIMS.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source ={DbPath}");

        public DbSet<Lot> Lots { get; set; }
        public DbSet<CustomerAllocation> CustomerAllocations { get; set; }
        
        public string DbPath { get; set; }
    }
}

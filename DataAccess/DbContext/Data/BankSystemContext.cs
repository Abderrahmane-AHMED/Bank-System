using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DataAccess.DbContext.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DbContext.Data
{
    public partial class BankSystemContext : IdentityDbContext<ApplicationUser>
    {
        public BankSystemContext()
        {
        }

        public BankSystemContext(DbContextOptions<BankSystemContext> options)
            : base(options)
        {

        }

        public virtual DbSet<TbClient> Clients { get; set; }    




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.ApplyConfiguration(new TbClientConfiguration());



        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);



    }
}

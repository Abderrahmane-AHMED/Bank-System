using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DbContext.Configuration
{
    public class TbClientConfiguration : IEntityTypeConfiguration<TbClient>
    {
        public void Configure(EntityTypeBuilder<TbClient> builder)
        {
            builder
             .HasKey(e => e.ClientId);



        }
    }
}

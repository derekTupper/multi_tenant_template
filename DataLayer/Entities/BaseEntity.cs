using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Common
{
    public class BaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        [Timestamp]
        public Byte[] RowVersion { get; set; }
        public bool Active { get; set; }

        protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseEntity>()
                .Property(p => p.RowVersion)
                .IsRowVersion();
        }
    }

 
}


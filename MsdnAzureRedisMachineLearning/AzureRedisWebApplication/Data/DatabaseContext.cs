using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AzureRedisWebApplication.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=AzureRedisWebApplication.DatabaseContext")
        {
        }

        public System.Data.Entity.DbSet<AzureRedisWebApplication.Models.Contact> Contacts { get; set; }
    }
}

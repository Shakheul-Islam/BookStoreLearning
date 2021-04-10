using BOOKSTORE.DOMAIN.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOKSTORE.INFRASTRUCTURE.Persistence
{
   public class AppContext: DbContext
    {
        public AppContext():base(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)
        {

        }
        public AppContext(string name): base(name)
        {

        }
        public virtual DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.AddFromAssembly(typeof(Book).Assembly);
        }
    }
}

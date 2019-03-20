using PPSale.Models.Complement;
using PPSale.Models.Departure;
using PPSale.Models.Entry;
using PPSale.Models.Globals;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace PPSale.Models.Conexion
{
    public class ConexionContext : DbContext
    {
        public ConexionContext() : base("DefaultConnection")
        {

        }

        //Eitar el Borrado en cascada
        protected override void OnModelCreating(DbModelBuilder modelBuilder) { modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>(); }

        //Cerrar la conexion
        protected override void Dispose(bool dispsing) { base.Dispose(dispsing); }

        
        public DbSet<Country> Countries { get; set; }

        public DbSet<Rol> Rols { get; set; }

        public DbSet<Province> Provinces { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<IvaCondition> IvaConditions { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<AsingRolAndUser> AsingRolAndUsers { get; set; }

        public DbSet<Provider> Providers { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<MeasuredUnit> MeasuredUnits { get; set; }

        public DbSet<TypeDocument> TypeDocuments { get; set; }

        public DbSet<CategoryDocument> CategoryDocuments { get; set; }

        public DbSet<DocumentEntry> DocumentEntries { get; set; }

        public DbSet<TypePayment> TypePayments { get; set; }

        public DbSet<Classification> Classifications { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductClassification> ProductClassifications { get; set; }

        public DbSet<DocumentEntryDetail> DocumentEntryDetails { get; set; }

        public DbSet<TempDocEntryDetil> TempDocEntryDetils { get; set; }

        public System.Data.Entity.DbSet<PPSale.Models.Complement.Kardex> Kardexes { get; set; }

        public System.Data.Entity.DbSet<PPSale.Models.Complement.UnitBase> UnitBases { get; set; }
    }
}
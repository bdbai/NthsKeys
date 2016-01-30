namespace NthsKeys.DataModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model : DbContext
    {
        public Model()
            : base("name=DataModel")
        {
        }

        public virtual DbSet<archive> archives { get; set; }
        public virtual DbSet<file> files { get; set; }
        public virtual DbSet<setting> settings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<archive>()
                .Property(e => e.Path)
                .IsUnicode(false);

            modelBuilder.Entity<archive>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<file>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<file>()
                .Property(e => e.Path)
                .IsUnicode(false);

            modelBuilder.Entity<setting>()
                .Property(e => e.Key)
                .IsUnicode(false);

            modelBuilder.Entity<setting>()
                .Property(e => e.Value)
                .IsUnicode(false);
        }
    }
}

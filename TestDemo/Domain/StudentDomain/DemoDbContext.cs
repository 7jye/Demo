using Microsoft.EntityFrameworkCore;
using Student.Domain.Entities;

namespace Student.Domain
{
    public class DemoDbContext:DbContext
    {
        public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
        {

        }

        #region DbSet设置
        public virtual  DbSet<StudentInfo> StudentInfo { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentInfo>(entity =>
            {
                entity.ToTable("StudentInfo")
                .HasKey(x => x.Id);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}

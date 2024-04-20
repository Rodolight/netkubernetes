using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NetKubernetes.Models.Data;

    public class AppDbContext:IdentityDbContext<Usuario>
    {

        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt){

        }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Usuario>().Property(x => x.Id).HasMaxLength(36);
         builder.Entity<Usuario>().Property(x => x.NormalizedUserName).HasMaxLength(90);
    //      builder.Entity<identityRole>().Property(x => x.Id).HasMaxLength(36);
    //      builder.Entity<identityRole>().Property(x => x.NormalizedUserName).HasMaxLength(90);
     }

    public DbSet<Inmueble>? Inmuebles {get;set;}

}

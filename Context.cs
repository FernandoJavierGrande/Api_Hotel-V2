using Api_Hotel_V2.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api_Hotel_V2
{
    public class Context : IdentityDbContext
    {
        public Context(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservacion>().HasKey(r => new { r.ReservaId, r.Fecha });


            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);

        }

        private void SeedData(ModelBuilder modelBuilder)
        {

            var rolAdminId = "9aae0b6d-d50c-4d0a-9b90-2a6873e3845d";
            var usuarioAdminId = "5673b8cf-12de-44f6-92ad-fae4a77932ad";

            var rolAdmin = new IdentityRole()
            {
                Id = rolAdminId,
                Name = "Admin",
                NormalizedName = "Admin"
            };

            var passwordHasher = new PasswordHasher<IdentityUser>();

            var username = "fernando@gmail.com";

            var usuarioAdmin = new IdentityUser()
            {
                Id = usuarioAdminId,
                UserName = username,
                NormalizedUserName = username,
                Email = username,
                NormalizedEmail = username,
                PasswordHash = passwordHasher.HashPassword(null, "Fernando1!")
            };
            //modelBuilder.Entity<IdentityUser>()
            //    .HasData(usuarioAdmin);

            //modelBuilder.Entity<IdentityRole>()
            //    .HasData(rolAdmin);

            //modelBuilder.Entity<IdentityUserClaim<string>>()
            //    .HasData(new IdentityUserClaim<string>()
            //    {
            //        Id = 1,
            //        ClaimType = ClaimTypes.Role,
            //        UserId = usuarioAdminId,
            //        ClaimValue = "Admin"
            //    });
        }
        public DbSet<Afiliado> Afiliados { get; set; }
        public DbSet<Habitacion> Habitaciones { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Reservacion> Reservaciones { get; set; }
    }
}

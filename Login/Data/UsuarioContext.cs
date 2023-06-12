using Login.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;

namespace Login.Data
{
    public class UsuarioContext : DbContext
    {
        public UsuarioContext(DbContextOptions<UsuarioContext> options) : base(options)
        {

        }
        public DbSet<Usuario> Usuarios { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario()
                {
                    UserId = 1,
                    UserName = "Owen",
                    Password = "123456"
                },
                new Usuario()
                {
                    UserId = 1,
                    UserName = "Andres",
                    Password = "123456"
                });
        }
    }
}

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BookLibEFC
{
    public class Author
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string LastName { get; set; }
    }
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Author Author { get; set; }
    }

    public class LibContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public LibContext(DbContextOptions<LibContext> options) : base(options)
        {
          Database.EnsureCreated();   // создание БД, если ее нет
        }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
    public class LibContextFactory : IDesignTimeDbContextFactory<LibContext>
    {
        public LibContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibContext>();

            // получаем конфигурацию из файла appsettings.json
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("config.json");
            IConfigurationRoot config = builder.Build();

            // получаем строку подключения из файла appsettings.json
            string connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString, opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
            return new LibContext(optionsBuilder.Options);
        }
    }
}

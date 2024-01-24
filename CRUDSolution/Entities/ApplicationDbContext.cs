using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entities
{
    //commands for Package Manager
    //Add-Migration <name> in the folder where u created the DbContext File
    //Update-Database -Verbose  here Verbose switch to view the sql commands in cmd
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        //name of DBSEt is plural strongly recommended
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //optional but if we want to change table name in future
            //Fluent Api use to map database table with model e.g.set primary key, non nullable value, required column value etc, fluent api get more procedence over data anotation
            //e.g of Fluent APi is below
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>()
                .ToTable("Persons")
                //.Property(temp=>temp.TIN)
                //.HasColumnType("varchar(8)")
                //.HasDefaultValue("ABC123")
                ;
            

            //Data Seeding
            string countriesJson = File.ReadAllText("Countries.json");
            List<Country> countries = JsonSerializer.Deserialize<List<Country>>(countriesJson);
            foreach (Country country in countries)
            {
                modelBuilder.Entity<Country>().HasData(country);
            }
            string personsJson = File.ReadAllText("Persons.json");
            List<Person> persons = JsonSerializer.Deserialize<List<Person>>(personsJson);
            foreach (Person person in persons)
            {
                modelBuilder.Entity<Person>().HasData(person);
            }
        }

    }
}

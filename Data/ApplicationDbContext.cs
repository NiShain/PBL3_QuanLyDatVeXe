using Microsoft.EntityFrameworkCore;
using PBL3_QuanLyDatXe.Models;
namespace PBL3_QuanLyDatXe.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Account> Users { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Line> Lines { get; set; }

        public DbSet<Ticket> Tickets { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Account)
                .WithOne(u => u.Customer)
                .HasForeignKey<Customer>(c => c.UserId);
        }

    }
}

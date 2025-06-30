using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MealTimes.Core.Models;
using Microsoft.Extensions.Logging;

namespace MealTimes.Repository
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration? _configuration;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration? configuration = null)
            : base(options)
        {
            _configuration = configuration;
        }

        // DbSets for each entity
        public DbSet<Admin> Admins { get; set; }
        public DbSet<CorporateCompany> CorporateCompanies { get; set; }
        public DbSet<HomeChef> HomeChefs { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<ThirdPartyDeliveryService> ThirdPartyDeliveryServices { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<User> Users { get; set; } // Ensure User table exists in the DbContext
        public DbSet<OrderMeal> OrderMeals { get; set; } // New join table
        public DbSet<CompanySubscriptionHistory> CompanySubscriptionHistories { get; set; }
        public DbSet<DeliveryPerson> DeliveryPersons { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<DietaryPreference> DietaryPreferences { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-J5IS95J\\SQLEXPRESS;Initial Catalog=MealTimes;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;")
                    .LogTo(Console.WriteLine, LogLevel.Information); ;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Admin to Corporate Company (1:N)
            modelBuilder.Entity<CorporateCompany>()
                .HasOne<Admin>()
                .WithMany()
                .HasForeignKey(c => c.AdminID)
                .OnDelete(DeleteBehavior.Restrict);

            // Corporate Company to Employee (1:N)
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.CorporateCompany)
                .WithMany(c => c.Employees)
                .HasForeignKey(e => e.CompanyID)
                .OnDelete(DeleteBehavior.Cascade);

            // Corporate Company to Subscription Plan (N:1)
            modelBuilder.Entity<CorporateCompany>()
                .HasOne(c => c.ActiveSubscriptionPlan)
                .WithMany()
                .HasForeignKey(c => c.ActiveSubscriptionPlanID)
                .OnDelete(DeleteBehavior.Restrict);

            // HomeChef to Meal (1:N)
            modelBuilder.Entity<Meal>()
                .HasOne(m => m.Chef)
                .WithMany(c => c.Meals)
                .HasForeignKey(m => m.ChefID)
                .OnDelete(DeleteBehavior.Cascade);

            // Employee to Order (1:N)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Employee)
                .WithMany(e => e.Orders)
                .HasForeignKey(o => o.EmployeeID)
                .OnDelete(DeleteBehavior.NoAction);

            // HomeChef to Order (1:N)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Chef)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.ChefID)
                .OnDelete(DeleteBehavior.NoAction);

            // ✅ Many-to-Many Relationship: Order ↔ Meals via OrderMeal
            modelBuilder.Entity<OrderMeal>()
                .HasKey(om => new { om.OrderID, om.MealID });

            modelBuilder.Entity<OrderMeal>()
                .HasOne(om => om.Order)
                .WithMany(o => o.OrderMeals)
                .HasForeignKey(om => om.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderMeal>()
                .HasOne(om => om.Meal)
                .WithMany(m => m.OrderMeals)
                .HasForeignKey(om => om.MealID)
                .OnDelete(DeleteBehavior.Cascade);

            // Order to Third-Party Delivery Service (1:1)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.ThirdPartyDeliveryService)
                .WithOne(d => d.Order)
                .HasForeignKey<ThirdPartyDeliveryService>(d => d.OrderID)
                .OnDelete(DeleteBehavior.NoAction);

            // Order to Payment (1:1)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderID)
                .OnDelete(DeleteBehavior.NoAction);

            // Order to Feedback (1:N)
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Order)
                .WithMany(o => o.Feedbacks)
                .HasForeignKey(f => f.OrderID)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee to Feedback (1:N)
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Employee)
                .WithMany(e => e.Feedbacks)
                .HasForeignKey(f => f.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Explicit User Foreign Key Fixes
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HomeChef>()
                .HasOne(h => h.User)
                .WithOne(u => u.HomeChef)
                .HasForeignKey<HomeChef>(h => h.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CorporateCompany>()
                .HasOne(c => c.User)
                .WithOne(u => u.CorporateCompany)
                .HasForeignKey<CorporateCompany>(c => c.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-one: User <--> Admin
            modelBuilder.Entity<User>()
                .HasOne(u => u.Admin)
                .WithOne(a => a.User)
                .HasForeignKey<Admin>(a => a.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-one: User <--> HomeChef
            modelBuilder.Entity<User>()
                .HasOne(u => u.HomeChef)
                .WithOne(h => h.User)
                .HasForeignKey<HomeChef>(h => h.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-one: User <--> CorporateCompany
            modelBuilder.Entity<User>()
                .HasOne(u => u.CorporateCompany)
                .WithOne(c => c.User)
                .HasForeignKey<CorporateCompany>(c => c.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-one: User <--> Employee
            modelBuilder.Entity<User>()
                .HasOne(u => u.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<Employee>(e => e.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .Property(o => o.DeliveryStatus)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.PaymentStatus)
                .HasConversion<string>();

            // ✅ Allow Payment to link optionally to a SubscriptionPlan
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.SubscriptionPlan)
                .WithMany() // If SubscriptionPlan has a Payments collection, replace with .WithMany(sp => sp.Payments)
                .HasForeignKey(p => p.SubscriptionPlanID)
                .OnDelete(DeleteBehavior.NoAction);

            // ✅ Allow Payment to optionally reference the CorporateCompany (who paid for the subscription)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.CorporateCompany)
                .WithMany() // If CorporateCompany has a Payments collection, replace with .WithMany(c => c.Payments)
                .HasForeignKey(p => p.CorporateCompanyID)
                .OnDelete(DeleteBehavior.NoAction);

            // One-to-one: User <--> DeliveryPerson
            modelBuilder.Entity<User>()
                .HasOne(u => u.DeliveryPerson)
                .WithOne(dp => dp.User)
                .HasForeignKey<DeliveryPerson>(dp => dp.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // DeliveryPerson to Delivery (1:N)
            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.DeliveryPerson)
                .WithMany(dp => dp.Deliveries)
                .HasForeignKey(d => d.DeliveryPersonID)
                .OnDelete(DeleteBehavior.SetNull);

            // Order to Delivery (1:1)
            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.Order)
                .WithOne(o => o.Delivery)
                .HasForeignKey<Delivery>(d => d.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            // Enum conversion for DeliveryStatus
            modelBuilder.Entity<Delivery>()
                .Property(d => d.Status)
                .HasConversion<string>();

            modelBuilder.Entity<DietaryPreference>(entity =>
            {
                entity.HasKey(dp => dp.DietaryPreferenceID);

                entity.Property(dp => dp.Allergies)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

                entity.Property(dp => dp.Preferences)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

                entity.Property(dp => dp.Restrictions)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

                entity.Property(dp => dp.CustomNotes)
                    .HasMaxLength(1000);

                entity.HasOne(dp => dp.Employee)
                    .WithOne(e => e.DietaryPreference)
                    .HasForeignKey<DietaryPreference>(dp => dp.EmployeeID)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}


using WSM.Domain.Entities;
using WSM.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;


namespace WSM.Infrastructure.DatabaseContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<EndpointUsage> EndpointUsages { get; set; }
        public DbSet<MikrotikCHR> MikrotikCHRs { get; set; }
        public DbSet<MikrotikEndpoint> MikrotikEndpoints { get; set; }
        public DbSet<EndpointCloseToExpiry> EndpointCloseToExpiry { get; set; }
        public DbSet<UserEndpoint> UserEndpoints { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id); // Configuring the primary key
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(250); // Making UserName required with max length
                entity.Property(e => e.TelegramId).IsRequired(); // Making TelegramId required
                entity.Property(e => e.FirstName).HasMaxLength(250); // Setting max length for FirstName
                entity.Property(e => e.LastName).HasMaxLength(250); // Setting max length for LastName
                entity.Property(e => e.Email).HasMaxLength(250); // Setting max length for Email
            });


            #endregion
            #region EndpointUasge
            modelBuilder.Entity<EndpointUsage>(entity =>
            {
                entity.HasKey(e => e.Id); // Configuring the primary key
                                          //entity.Property(e => e.MikrotikEndpointId).IsRequired(); // Making a property required
                                          // Additional configurations can be added here
            });


            #endregion

            #region MikrotikEndpoint
            modelBuilder.Entity<MikrotikEndpoint>(entity =>
            {
                entity.OwnsOne(e => e.PublicKey, pk =>
                {
                    pk.Property(p => p.EncodedKey)
                      .HasColumnName("PublicKey")
                      .HasColumnType("varchar");
                });

                entity.OwnsOne(e => e.PrivateKey, pk =>
                {
                    pk.Property(p => p.EncodedKey)
                      .HasColumnName("PrivateKey")
                      .HasColumnType("varchar");
                });
                entity.OwnsOne(e => e.AllowedAddress, pk =>
                {
                    pk.Property(p => p.Value)
                      .HasColumnName("AllowedAddress")
                      .HasColumnType("varchar");

                });



            });
            #endregion


            #region EndpointCloseToExpiry
            base.OnModelCreating(modelBuilder);

            // Configure the entity as keyless
            modelBuilder.Entity<EndpointCloseToExpiry>(entity =>
            {
                entity.HasNoKey();
                entity.ToFunction("Get_Endpoints_Close_To_Expire"); // Map to the PostgreSQL function
                entity.Ignore(e => e.AllowedAddress);
                modelBuilder.Entity<EndpointCloseToExpiry>()
                    .Property(e => e.AllowedAddress)
                    .HasConversion(
                        v => v.ToString(), // Convert IpAddress to string for storage
                        v => new IpAddress(v) // Convert string back to IpAddress on retrieval
                    );
            });
  
            #endregion
        }





        //public Expression<Func<T, object>> GetSortExpression<T>(string sortColumn)
        //{
        //    var param = Expression.Parameter(typeof(T), "entity");

        //    // Find the property by name on the specified type T
        //    var property = Expression.PropertyOrField(param, sortColumn);
        //    var convertedProperty = Expression.Convert(property, typeof(object));

        //    return Expression.Lambda<Func<T, object>>(convertedProperty, param);
        //}
    }
}

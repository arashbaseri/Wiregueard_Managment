
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

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region User


            //var emailConverter = new ValueConverter<Email, string>(
            //    email => email.ToString(), // convert Email to string for storage
            //    value => new Email(value)  // convert string back to Email on retrieval
            //);

            //modelBuilder.Entity<User>()
            //    .Property(user => user.Email)
            //    .HasConversion(emailConverter)
            //    .HasColumnType("varchar(100)"); // Define the SQL data type and length

            //var phoneConverter = new ValueConverter<Phone, string>(
            //    phone => phone.ToString(), // convert Email to string for storage
            //    value => new Phone(value)  // convert string back to Email on retrieval
            //);


            //modelBuilder.Entity<User>()
            //    .Property(user => user.Phone)
            //    .HasConversion(phoneConverter)
            //    .HasColumnType("varchar(15)"); // Define the SQL data type and length

            //modelBuilder.Ignore<Login>();

            //modelBuilder.Ignore<Email>();
            //modelBuilder.Ignore<Phone>();

            //modelBuilder.Ignore<UserReadDto>();
            //modelBuilder.Ignore<UserCreateDto>();
            //modelBuilder.Ignore<UserUpdateDto>();

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





        public Expression<Func<T, object>> GetSortExpression<T>(string sortColumn)
        {
            var param = Expression.Parameter(typeof(T), "entity");

            // Find the property by name on the specified type T
            var property = Expression.PropertyOrField(param, sortColumn);
            var convertedProperty = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(convertedProperty, param);
        }
    }
}

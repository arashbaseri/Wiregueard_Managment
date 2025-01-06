﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WSM.Infrastructure.DatabaseContext;

#nullable disable

namespace WSM.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250103011635_addFunc_Get_Endpoints_Close_To_Expire")]
    partial class addFunc_Get_Endpoints_Close_To_Expire
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WSM.Domain.Entities.EndpointCloseToExpiry", b =>
                {
                    b.Property<int?>("DaysToRenew")
                        .HasColumnType("integer");

                    b.Property<bool>("Disabled")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("MikrotikInterface")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("RenewDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("TelegramId")
                        .HasColumnType("bigint");

                    b.ToTable("EndpointCloseToExpiry");

                    b.ToFunction("Get_Endpoints_Close_To_Expire");
                });

            modelBuilder.Entity("WSM.Domain.Entities.EndpointUsage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<long>("BytesIn")
                        .HasColumnType("bigint");

                    b.Property<long>("BytesOut")
                        .HasColumnType("bigint");

                    b.Property<long?>("BytesTotal")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("MikrotikEndpointId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MikrotikServerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long?>("PacketsIn")
                        .HasColumnType("bigint");

                    b.Property<long?>("PacketsOut")
                        .HasColumnType("bigint");

                    b.Property<long?>("PacketsTotal")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("EndpointUsages");
                });

            modelBuilder.Entity("WSM.Domain.Entities.MikrotikCHR", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("WWWPort")
                        .HasColumnType("integer");

                    b.Property<int?>("WinboxPort")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("MikrotikCHRs");
                });

            modelBuilder.Entity("WSM.Domain.Entities.MikrotikEndpoint", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("DaysToRenew")
                        .HasColumnType("integer");

                    b.Property<bool>("Disabled")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("MikrotikInterface")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("MikrotikServerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("RenewDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("MikrotikEndpoints");
                });

            modelBuilder.Entity("WSM.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("TelegramId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WSM.Domain.Entities.MikrotikEndpoint", b =>
                {
                    b.OwnsOne("WSM.Domain.ValueObjects.Base64EncodedKey", "PrivateKey", b1 =>
                        {
                            b1.Property<Guid>("MikrotikEndpointId")
                                .HasColumnType("uuid");

                            b1.Property<string>("EncodedKey")
                                .IsRequired()
                                .HasColumnType("varchar")
                                .HasColumnName("PrivateKey");

                            b1.HasKey("MikrotikEndpointId");

                            b1.ToTable("MikrotikEndpoints");

                            b1.WithOwner()
                                .HasForeignKey("MikrotikEndpointId");
                        });

                    b.OwnsOne("WSM.Domain.ValueObjects.Base64EncodedKey", "PublicKey", b1 =>
                        {
                            b1.Property<Guid>("MikrotikEndpointId")
                                .HasColumnType("uuid");

                            b1.Property<string>("EncodedKey")
                                .IsRequired()
                                .HasColumnType("varchar")
                                .HasColumnName("PublicKey");

                            b1.HasKey("MikrotikEndpointId");

                            b1.ToTable("MikrotikEndpoints");

                            b1.WithOwner()
                                .HasForeignKey("MikrotikEndpointId");
                        });

                    b.OwnsOne("WSM.Domain.ValueObjects.IpAddress", "AllowedAddress", b1 =>
                        {
                            b1.Property<Guid>("MikrotikEndpointId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .HasColumnType("varchar")
                                .HasColumnName("AllowedAddress");

                            b1.HasKey("MikrotikEndpointId");

                            b1.ToTable("MikrotikEndpoints");

                            b1.WithOwner()
                                .HasForeignKey("MikrotikEndpointId");
                        });

                    b.Navigation("AllowedAddress")
                        .IsRequired();

                    b.Navigation("PrivateKey");

                    b.Navigation("PublicKey");
                });
#pragma warning restore 612, 618
        }
    }
}

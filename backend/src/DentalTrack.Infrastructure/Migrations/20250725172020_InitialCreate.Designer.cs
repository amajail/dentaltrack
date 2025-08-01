﻿// <auto-generated />
using System;
using DentalTrack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DentalTrack.Infrastructure.Migrations
{
    [DbContext(typeof(DentalTrackDbContext))]
    [Migration("20250725172020_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DentalTrack.Domain.Entities.Analysis", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CompletedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("ConfidenceScore")
                        .HasColumnType("decimal(5,4)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ErrorMessage")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Findings")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<Guid>("PhotoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ProcessingTimeMs")
                        .HasColumnType("int");

                    b.Property<string>("Recommendations")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("Results")
                        .HasMaxLength(5000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid?>("TreatmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PhotoId");

                    b.HasIndex("Status");

                    b.HasIndex("TreatmentId");

                    b.HasIndex("Type");

                    b.ToTable("Analyses");
                });

            modelBuilder.Entity("DentalTrack.Domain.Entities.Patient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Allergies")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("EmergencyContact")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("EmergencyPhone")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Gender")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("MedicalHistory")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("FirstName", "LastName");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("DentalTrack.Domain.Entities.Photo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsProcessed")
                        .HasColumnType("bit");

                    b.Property<int>("Quality")
                        .HasColumnType("int");

                    b.Property<int?>("ToothNumber")
                        .HasColumnType("int");

                    b.Property<Guid>("TreatmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Quality");

                    b.HasIndex("TreatmentId");

                    b.HasIndex("Type");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("DentalTrack.Domain.Entities.Treatment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal?>("ActualCost")
                        .HasColumnType("decimal(10,2)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("EstimatedCost")
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("Notes")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.HasIndex("Status");

                    b.HasIndex("Type");

                    b.ToTable("Treatments");
                });

            modelBuilder.Entity("DentalTrack.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("GoogleId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLoginAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("GoogleId")
                        .IsUnique()
                        .HasFilter("[GoogleId] IS NOT NULL");

                    b.HasIndex("IsActive");

                    b.HasIndex("Role");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DentalTrack.Domain.Entities.Analysis", b =>
                {
                    b.HasOne("DentalTrack.Domain.Entities.Photo", "Photo")
                        .WithMany("Analyses")
                        .HasForeignKey("PhotoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DentalTrack.Domain.Entities.Treatment", null)
                        .WithMany("Analyses")
                        .HasForeignKey("TreatmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Photo");
                });

            modelBuilder.Entity("DentalTrack.Domain.Entities.Photo", b =>
                {
                    b.HasOne("DentalTrack.Domain.Entities.Treatment", "Treatment")
                        .WithMany("Photos")
                        .HasForeignKey("TreatmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("DentalTrack.Domain.ValueObjects.PhotoMetadata", "Metadata", b1 =>
                        {
                            b1.Property<Guid>("PhotoId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("CameraMake")
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<string>("CameraModel")
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<DateTime?>("DateTaken")
                                .HasColumnType("datetime2");

                            b1.Property<double?>("ExposureTime")
                                .HasColumnType("float");

                            b1.Property<double?>("FNumber")
                                .HasColumnType("float");

                            b1.Property<string>("Flash")
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<double?>("FocalLength")
                                .HasColumnType("float");

                            b1.Property<int>("Height")
                                .HasColumnType("int");

                            b1.Property<int?>("Iso")
                                .HasColumnType("int");

                            b1.Property<string>("Location")
                                .HasMaxLength(200)
                                .HasColumnType("nvarchar(200)");

                            b1.Property<int>("Width")
                                .HasColumnType("int");

                            b1.HasKey("PhotoId");

                            b1.ToTable("Photos");

                            b1.WithOwner()
                                .HasForeignKey("PhotoId");
                        });

                    b.Navigation("Metadata")
                        .IsRequired();

                    b.Navigation("Treatment");
                });

            modelBuilder.Entity("DentalTrack.Domain.Entities.Treatment", b =>
                {
                    b.HasOne("DentalTrack.Domain.Entities.Patient", "Patient")
                        .WithMany("Treatments")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("DentalTrack.Domain.Entities.Patient", b =>
                {
                    b.Navigation("Treatments");
                });

            modelBuilder.Entity("DentalTrack.Domain.Entities.Photo", b =>
                {
                    b.Navigation("Analyses");
                });

            modelBuilder.Entity("DentalTrack.Domain.Entities.Treatment", b =>
                {
                    b.Navigation("Analyses");

                    b.Navigation("Photos");
                });
#pragma warning restore 612, 618
        }
    }
}

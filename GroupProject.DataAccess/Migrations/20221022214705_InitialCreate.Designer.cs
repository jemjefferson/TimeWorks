﻿// <auto-generated />
using System;
using GroupProject.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GroupProject.DataAccess.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20221022214705_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("GroupProject.DataAccess.Break", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Comment")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("HourId")
                        .HasColumnType("int");

                    b.Property<bool>("Paid")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("HourId");

                    b.ToTable("Breaks");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Comment = "Comment 1",
                            EndTime = new DateTime(2022, 9, 5, 3, 30, 0, 0, DateTimeKind.Unspecified),
                            HourId = 2,
                            Paid = false,
                            StartTime = new DateTime(2022, 9, 5, 3, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2,
                            Comment = "Comment 2",
                            EndTime = new DateTime(2022, 9, 5, 3, 30, 0, 0, DateTimeKind.Unspecified),
                            HourId = 5,
                            Paid = false,
                            StartTime = new DateTime(2022, 9, 5, 3, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 3,
                            Comment = "Comment 3",
                            EndTime = new DateTime(2022, 9, 5, 3, 30, 0, 0, DateTimeKind.Unspecified),
                            HourId = 8,
                            Paid = true,
                            StartTime = new DateTime(2022, 9, 5, 3, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("GroupProject.DataAccess.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int>("Manager")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<string>("Zip")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "123 Street",
                            City = "Wausau",
                            FirstName = "Jacob",
                            LastName = "Mahner",
                            Manager = 1,
                            State = "WI",
                            Zip = "54401-0400"
                        },
                        new
                        {
                            Id = 2,
                            Address = "456 Street",
                            City = "Wausau",
                            FirstName = "Zach",
                            LastName = "Johnson",
                            Manager = 1,
                            State = "WI",
                            Zip = "54401-0400"
                        },
                        new
                        {
                            Id = 3,
                            Address = "789 Street",
                            City = "Wausau",
                            FirstName = "Jordan",
                            LastName = "Jefferson",
                            Manager = 2,
                            State = "WI",
                            Zip = "54401-0400"
                        });
                });

            modelBuilder.Entity("GroupProject.DataAccess.EmployeeJobCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("JobCodeId")
                        .HasColumnType("int");

                    b.Property<double>("PayRate")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("JobCodeId");

                    b.ToTable("EmployeeJobCodes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Active = true,
                            EmployeeId = 1,
                            JobCodeId = 1,
                            PayRate = 24.0
                        },
                        new
                        {
                            Id = 2,
                            Active = true,
                            EmployeeId = 2,
                            JobCodeId = 2,
                            PayRate = 23.5
                        },
                        new
                        {
                            Id = 3,
                            Active = true,
                            EmployeeId = 3,
                            JobCodeId = 3,
                            PayRate = 21.5
                        },
                        new
                        {
                            Id = 4,
                            Active = true,
                            EmployeeId = 3,
                            JobCodeId = 4,
                            PayRate = 20.0
                        });
                });

            modelBuilder.Entity("GroupProject.DataAccess.Hour", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("ApprovedBy")
                        .HasColumnType("int");

                    b.Property<string>("Comment")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<bool>("EmployeeApproved")
                        .HasColumnType("bit");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeJobCodeId")
                        .HasColumnType("int");

                    b.Property<int>("PayPeriodId")
                        .HasColumnType("int");

                    b.Property<double>("PayRate")
                        .HasColumnType("float");

                    b.Property<DateTime>("TimeEntered")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TimeIn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TimeOut")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("EmployeeJobCodeId");

                    b.HasIndex("PayPeriodId");

                    b.ToTable("Hours");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ApprovedBy = 1,
                            Comment = "Comment 1",
                            EmployeeApproved = true,
                            EmployeeId = 1,
                            EmployeeJobCodeId = 1,
                            PayPeriodId = 1,
                            PayRate = 24.0,
                            TimeEntered = new DateTime(2022, 9, 4, 9, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeIn = new DateTime(2022, 9, 4, 1, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeOut = new DateTime(2022, 9, 4, 9, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2,
                            ApprovedBy = 1,
                            Comment = "Comment 2",
                            EmployeeApproved = true,
                            EmployeeId = 1,
                            EmployeeJobCodeId = 1,
                            PayPeriodId = 1,
                            PayRate = 24.0,
                            TimeEntered = new DateTime(2022, 9, 5, 9, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeIn = new DateTime(2022, 9, 5, 1, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeOut = new DateTime(2022, 9, 5, 9, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 3,
                            ApprovedBy = 1,
                            Comment = "Comment 3",
                            EmployeeApproved = true,
                            EmployeeId = 1,
                            EmployeeJobCodeId = 1,
                            PayPeriodId = 1,
                            PayRate = 24.0,
                            TimeEntered = new DateTime(2022, 9, 6, 9, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeIn = new DateTime(2022, 9, 6, 1, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeOut = new DateTime(2022, 9, 6, 9, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 4,
                            ApprovedBy = 1,
                            Comment = "Comment 1",
                            EmployeeApproved = true,
                            EmployeeId = 2,
                            EmployeeJobCodeId = 2,
                            PayPeriodId = 1,
                            PayRate = 23.5,
                            TimeEntered = new DateTime(2022, 9, 4, 9, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeIn = new DateTime(2022, 9, 4, 1, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeOut = new DateTime(2022, 9, 4, 9, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 5,
                            ApprovedBy = 1,
                            Comment = "Comment 2",
                            EmployeeApproved = true,
                            EmployeeId = 2,
                            EmployeeJobCodeId = 2,
                            PayPeriodId = 1,
                            PayRate = 23.5,
                            TimeEntered = new DateTime(2022, 9, 5, 9, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeIn = new DateTime(2022, 9, 5, 1, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeOut = new DateTime(2022, 9, 5, 9, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 6,
                            ApprovedBy = 1,
                            Comment = "Comment 3",
                            EmployeeApproved = true,
                            EmployeeId = 2,
                            EmployeeJobCodeId = 2,
                            PayPeriodId = 1,
                            PayRate = 23.5,
                            TimeEntered = new DateTime(2022, 9, 6, 9, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeIn = new DateTime(2022, 9, 6, 1, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeOut = new DateTime(2022, 9, 6, 9, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 7,
                            ApprovedBy = 2,
                            Comment = "Comment 1",
                            EmployeeApproved = true,
                            EmployeeId = 3,
                            EmployeeJobCodeId = 3,
                            PayPeriodId = 1,
                            PayRate = 21.5,
                            TimeEntered = new DateTime(2022, 9, 4, 9, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeIn = new DateTime(2022, 9, 4, 1, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeOut = new DateTime(2022, 9, 4, 9, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 8,
                            ApprovedBy = 2,
                            Comment = "Comment 2",
                            EmployeeApproved = true,
                            EmployeeId = 3,
                            EmployeeJobCodeId = 3,
                            PayPeriodId = 1,
                            PayRate = 21.5,
                            TimeEntered = new DateTime(2022, 9, 5, 9, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeIn = new DateTime(2022, 9, 5, 1, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeOut = new DateTime(2022, 9, 5, 9, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 9,
                            ApprovedBy = 2,
                            Comment = "Comment 3",
                            EmployeeApproved = true,
                            EmployeeId = 3,
                            EmployeeJobCodeId = 4,
                            PayPeriodId = 1,
                            PayRate = 20.0,
                            TimeEntered = new DateTime(2022, 9, 6, 9, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeIn = new DateTime(2022, 9, 6, 1, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeOut = new DateTime(2022, 9, 6, 9, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("GroupProject.DataAccess.JobCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("JobTitle")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<double>("StartingPay")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("JobCodes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Department = "Information Technology",
                            JobTitle = "Timeworks Admin",
                            StartingPay = 23.5
                        },
                        new
                        {
                            Id = 2,
                            Department = "Information Technology",
                            JobTitle = "Timeworks Manager",
                            StartingPay = 22.5
                        },
                        new
                        {
                            Id = 3,
                            Department = "Information Technology",
                            JobTitle = "Timeworks Developer",
                            StartingPay = 21.5
                        },
                        new
                        {
                            Id = 4,
                            Department = "Information Technology",
                            JobTitle = "Application Support Senior",
                            StartingPay = 20.0
                        });
                });

            modelBuilder.Entity("GroupProject.DataAccess.Login", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Admin")
                        .HasColumnType("bit");

                    b.Property<int>("EmployeeID")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeID")
                        .IsUnique();

                    b.ToTable("Logins");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Admin = true,
                            EmployeeID = 1,
                            PasswordHash = "AQAAAAEAACcQAAAAEHQvShwMx9WV7o77eCrLDyweVJh+EWJtxBNYH72/qEm1WSi8SKm3uCBmMzzR6dkdaQ==",
                            UserName = "jacobmahner1"
                        },
                        new
                        {
                            Id = 2,
                            Admin = false,
                            EmployeeID = 2,
                            PasswordHash = "AQAAAAEAACcQAAAAEDjKdRio1JIvInP+fSSt3L6ZzCN7xLJr+Oe8eRc0Yvh2FvW6utOsoI4qa/st5Cr1TQ==",
                            UserName = "zachjohnson1"
                        },
                        new
                        {
                            Id = 3,
                            Admin = false,
                            EmployeeID = 3,
                            PasswordHash = "AQAAAAEAACcQAAAAEIZINxTPRI5W7NZ5Duo2Gffy1hnOUzWf7XU2u+F1MaRh4Y9tBAXdKI98rwe7ae0ZRw==",
                            UserName = "jordanjefferson1"
                        });
                });

            modelBuilder.Entity("GroupProject.DataAccess.PayPeriod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("End")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("PayPeriods");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            End = new DateTime(2022, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Start = new DateTime(2022, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2,
                            End = new DateTime(2022, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Start = new DateTime(2022, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 3,
                            End = new DateTime(2022, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Start = new DateTime(2022, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("GroupProject.DataAccess.Break", b =>
                {
                    b.HasOne("GroupProject.DataAccess.Hour", "Hour")
                        .WithMany("Breaks")
                        .HasForeignKey("HourId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Hour");
                });

            modelBuilder.Entity("GroupProject.DataAccess.EmployeeJobCode", b =>
                {
                    b.HasOne("GroupProject.DataAccess.Employee", "Employee")
                        .WithMany("EmployeeJobCodes")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("GroupProject.DataAccess.JobCode", "JobCode")
                        .WithMany("EmployeeJobCodes")
                        .HasForeignKey("JobCodeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("JobCode");
                });

            modelBuilder.Entity("GroupProject.DataAccess.Hour", b =>
                {
                    b.HasOne("GroupProject.DataAccess.Employee", "Employee")
                        .WithMany("Hours")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("GroupProject.DataAccess.EmployeeJobCode", "EmployeeJobCode")
                        .WithMany("Hours")
                        .HasForeignKey("EmployeeJobCodeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("GroupProject.DataAccess.PayPeriod", "PayPeriod")
                        .WithMany("Hours")
                        .HasForeignKey("PayPeriodId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("EmployeeJobCode");

                    b.Navigation("PayPeriod");
                });

            modelBuilder.Entity("GroupProject.DataAccess.Login", b =>
                {
                    b.HasOne("GroupProject.DataAccess.Employee", "Employee")
                        .WithOne("Login")
                        .HasForeignKey("GroupProject.DataAccess.Login", "EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("GroupProject.DataAccess.Employee", b =>
                {
                    b.Navigation("EmployeeJobCodes");

                    b.Navigation("Hours");

                    b.Navigation("Login")
                        .IsRequired();
                });

            modelBuilder.Entity("GroupProject.DataAccess.EmployeeJobCode", b =>
                {
                    b.Navigation("Hours");
                });

            modelBuilder.Entity("GroupProject.DataAccess.Hour", b =>
                {
                    b.Navigation("Breaks");
                });

            modelBuilder.Entity("GroupProject.DataAccess.JobCode", b =>
                {
                    b.Navigation("EmployeeJobCodes");
                });

            modelBuilder.Entity("GroupProject.DataAccess.PayPeriod", b =>
                {
                    b.Navigation("Hours");
                });
#pragma warning restore 612, 618
        }
    }
}

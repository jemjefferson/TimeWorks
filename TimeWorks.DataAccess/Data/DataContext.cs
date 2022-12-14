using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TimeWorks.DataAccess;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    { }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<Break> Breaks { get; set; }

    public DbSet<EmployeeJobCode> EmployeeJobCodes { get; set; }

    public DbSet<Hour> Hours { get; set; }

    public DbSet<JobCode> JobCodes { get; set; }

    public DbSet<Login> Logins { get; set; }

    public DbSet<PayPeriod> PayPeriods { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        PasswordHasher<Login> passwordHasher = new PasswordHasher<Login>();
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Employee>().HasMany(e => e.Hours).WithOne(h => h.Employee).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Employee>().HasMany(e => e.EmployeeJobCodes).WithOne(e => e.Employee).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Employee>().HasOne(e => e.Login).WithOne(l => l.Employee).HasForeignKey<Login>(l => l.EmployeeID);
        modelBuilder.Entity<Hour>().HasMany(h => h.Breaks).WithOne(b => b.Hour).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<PayPeriod>().HasMany(p => p.Hours).WithOne(h => h.PayPeriod).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<JobCode>().HasMany(j => j.EmployeeJobCodes).WithOne(e => e.JobCode).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<EmployeeJobCode>().HasMany(e => e.Hours).WithOne(h => h.EmployeeJobCode).OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, FirstName = "Jacob", LastName = "Mahner", Address = "123 Street", City = "Wausau", State = "WI", Zip = "54401-0400", Manager = 1 },
            new Employee { Id = 2, FirstName = "Zach", LastName = "Johnson", Address = "456 Street", City = "Wausau", State = "WI", Zip = "54401-0400", Manager = 1 },
            new Employee { Id = 3, FirstName = "Jordan", LastName = "Jefferson", Address = "789 Street", City = "Wausau", State = "WI", Zip = "54401-0400", Manager = 2 }
        );
        modelBuilder.Entity<Login>().HasData(
            new Login { Id = 1, UserName = "jacobmahner1", EmployeeID = 1, Admin = true, PasswordHash = passwordHasher.HashPassword(new Login(), "timeworks123")},
            new Login { Id = 2, UserName = "zachjohnson1", EmployeeID = 2, Admin = false, PasswordHash = passwordHasher.HashPassword(new Login(), "timeworks456") },
            new Login { Id = 3, UserName = "jordanjefferson1", EmployeeID = 3, Admin = false, PasswordHash = passwordHasher.HashPassword(new Login(), "timeworks789") }
        );
        modelBuilder.Entity<JobCode>().HasData(
            new JobCode { Id = 1, JobTitle = "Timeworks Admin", StartingPay = 23.50, Department = "Information Technology" },
            new JobCode { Id = 2, JobTitle = "Timeworks Manager", StartingPay = 22.50, Department = "Information Technology" },
            new JobCode { Id = 3, JobTitle = "Timeworks Developer", StartingPay = 21.50, Department = "Information Technology" },
            new JobCode { Id = 4, JobTitle = "Application Support Senior", StartingPay = 20.00, Department = "Information Technology" }
        );
        modelBuilder.Entity<EmployeeJobCode>().HasData(
            new EmployeeJobCode { Id = 1, EmployeeId = 1, JobCodeId = 1, PayRate = 24.00, Active = true },
            new EmployeeJobCode { Id = 2, EmployeeId = 2, JobCodeId = 2, PayRate = 23.50, Active = true },
            new EmployeeJobCode { Id = 3, EmployeeId = 3, JobCodeId = 3, PayRate = 21.50, Active = true },
            new EmployeeJobCode { Id = 4, EmployeeId = 3, JobCodeId = 4, PayRate = 20.00, Active = true }
        );
        modelBuilder.Entity<PayPeriod>().HasData(
            new PayPeriod { Id = 1, Start = new DateTime(2022, 9, 4), End = new DateTime(2022, 9, 17) },
             new PayPeriod { Id = 2, Start = new DateTime(2022, 9, 18), End = new DateTime(2022, 10, 1) },
              new PayPeriod { Id = 3, Start = new DateTime(2022, 10, 2), End = new DateTime(2022, 10, 15) }
        );
        modelBuilder.Entity<Hour>().HasData(
            new Hour { Id = 1, TimeIn = new DateTime(2022, 9, 4, 1, 0, 0), TimeOut = new DateTime(2022, 9, 4, 9, 0, 0), EmployeeId = 1, TimeEntered = new DateTime(2022, 9, 4, 9, 0, 0), Comment = "Comment 1", EmployeeJobCodeId = 1, PayPeriodId = 1, ApprovedBy = 1, EmployeeApproved = true, PayRate = 24.00 },
            new Hour { Id = 2, TimeIn = new DateTime(2022, 9, 5, 1, 0, 0), TimeOut = new DateTime(2022, 9, 5, 9, 0, 0), EmployeeId = 1, TimeEntered = new DateTime(2022, 9, 5, 9, 0, 0), Comment = "Comment 2", EmployeeJobCodeId = 1, PayPeriodId = 1, ApprovedBy = 1, EmployeeApproved = true, PayRate = 24.00 },
            new Hour { Id = 3, TimeIn = new DateTime(2022, 9, 6, 1, 0, 0), TimeOut = new DateTime(2022, 9, 6, 9, 0, 0), EmployeeId = 1, TimeEntered = new DateTime(2022, 9, 6, 9, 0, 0), Comment = "Comment 3", EmployeeJobCodeId = 1, PayPeriodId = 1, ApprovedBy = 1, EmployeeApproved = true, PayRate = 24.00 },
            new Hour { Id = 4, TimeIn = new DateTime(2022, 9, 4, 1, 0, 0), TimeOut = new DateTime(2022, 9, 4, 9, 0, 0), EmployeeId = 2, TimeEntered = new DateTime(2022, 9, 4, 9, 0, 0), Comment = "Comment 1", EmployeeJobCodeId = 2, PayPeriodId = 1, ApprovedBy = 1, EmployeeApproved = true, PayRate = 23.50 },
            new Hour { Id = 5, TimeIn = new DateTime(2022, 9, 5, 1, 0, 0), TimeOut = new DateTime(2022, 9, 5, 9, 0, 0), EmployeeId = 2, TimeEntered = new DateTime(2022, 9, 5, 9, 0, 0), Comment = "Comment 2", EmployeeJobCodeId = 2, PayPeriodId = 1, ApprovedBy = 1, EmployeeApproved = true, PayRate = 23.50 },
            new Hour { Id = 6, TimeIn = new DateTime(2022, 9, 6, 1, 0, 0), TimeOut = new DateTime(2022, 9, 6, 9, 0, 0), EmployeeId = 2, TimeEntered = new DateTime(2022, 9, 6, 9, 0, 0), Comment = "Comment 3", EmployeeJobCodeId = 2, PayPeriodId = 1, ApprovedBy = 1, EmployeeApproved = true, PayRate = 23.50 },
            new Hour { Id = 7, TimeIn = new DateTime(2022, 9, 4, 1, 0, 0), TimeOut = new DateTime(2022, 9, 4, 9, 0, 0), EmployeeId = 3, TimeEntered = new DateTime(2022, 9, 4, 9, 0, 0), Comment = "Comment 1", EmployeeJobCodeId = 3, PayPeriodId = 1, ApprovedBy = 2, EmployeeApproved = true, PayRate = 21.50 },
            new Hour { Id = 8, TimeIn = new DateTime(2022, 9, 5, 1, 0, 0), TimeOut = new DateTime(2022, 9, 5, 9, 0, 0), EmployeeId = 3, TimeEntered = new DateTime(2022, 9, 5, 9, 0, 0), Comment = "Comment 2", EmployeeJobCodeId = 3, PayPeriodId = 1, ApprovedBy = 2, EmployeeApproved = true, PayRate = 21.50 },
            new Hour { Id = 9, TimeIn = new DateTime(2022, 9, 6, 1, 0, 0), TimeOut = new DateTime(2022, 9, 6, 9, 0, 0), EmployeeId = 3, TimeEntered = new DateTime(2022, 9, 6, 9, 0, 0), Comment = "Comment 3", EmployeeJobCodeId = 4, PayPeriodId = 1, ApprovedBy = 2, EmployeeApproved = true, PayRate = 20.00 }
        );
        modelBuilder.Entity<Break>().HasData(
            new Break { Id = 1, StartTime = new DateTime(2022, 9, 5, 3, 0, 0), EndTime = new DateTime(2022, 9, 5, 3, 30, 0), HourId = 2, Paid = false, Comment = "Comment 1" },
            new Break { Id = 2, StartTime = new DateTime(2022, 9, 5, 3, 0, 0), EndTime = new DateTime(2022, 9, 5, 3, 30, 0), HourId = 5, Paid = false, Comment = "Comment 2" },
            new Break { Id = 3, StartTime = new DateTime(2022, 9, 5, 3, 0, 0), EndTime = new DateTime(2022, 9, 5, 3, 30, 0), HourId = 8, Paid = true, Comment = "Comment 3" }
        );
    }
}

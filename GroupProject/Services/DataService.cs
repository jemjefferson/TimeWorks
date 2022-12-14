using GroupProject.Models;
using GroupProject.DataAccess;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Plugins;

namespace GroupProject.Services
{
    public class DataService
    {
        private readonly DataContext _dataContext;

        public DataService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Employee AddEmployee(Employee employee)
        {
            employee.Login.Employee = employee;
            employee.Login.EmployeeID = employee.Id;
            _dataContext.Employees.Add(employee);
            PasswordHasher<Login> passwordHasher = new PasswordHasher<Login>();
            employee.Login.PasswordHash = passwordHasher.HashPassword(employee.Login, employee.Login.PasswordHash);
            _dataContext.Logins.Add(employee.Login);
            _dataContext.SaveChanges();
            return employee;
        }

        public JobCode AddJobCode(JobCode jobCode)
        {
            _dataContext.JobCodes.Add(jobCode);
            _dataContext.SaveChanges();
            return jobCode;
        }

        public JobCode EditJobCode(JobCode jobCode)
        {
            _dataContext.JobCodes.Update(jobCode);
            _dataContext.SaveChanges();
            return jobCode;
        }

        public EmployeeJobCode AddEmployeeJobCode(EmployeeJobCode e)
        {
            _dataContext.EmployeeJobCodes.Add(e);
            _dataContext.SaveChanges();
            return e;
        }

        public List<Employee> GetEmployees()
        {
            return _dataContext.Employees.AsNoTracking().ToList();
        }

        public void AddBreak(Break brk)
        {
            _dataContext.Breaks.Add(brk);
            _dataContext.SaveChanges();
        }

        public List<Break> GetBreaks()
        {
            return _dataContext.Breaks.ToList();
        }

        public void UpdateBreak(Break brk)
        {
            _dataContext.Entry(brk).State = EntityState.Modified;
            _dataContext.SaveChanges();
        }

        public void RemoveBreak(Break brk)
        {
            _dataContext.Breaks.Remove(brk);
            _dataContext.SaveChanges();
        }

        public List<EmployeeJobCode> GetEmployeeJobCodes()
        {
            return _dataContext.EmployeeJobCodes.ToList();
        }

        public EmployeeJobCode GetEmployeeJobCode(int id)
        {
            return _dataContext.EmployeeJobCodes.Include(j => j.JobCode).FirstOrDefault(x => x.Id == id);
        }

        public Hour AddHour(Hour hour)
        {
            _dataContext.Hours.Add(hour);
            _dataContext.SaveChanges();
            return hour;
        }

        public Hour UpdateHour(Hour hour)
        {
            _dataContext.Entry(hour).State = EntityState.Modified;
            _dataContext.SaveChanges();
            return hour;
        }

        public void RemoveHour(Hour hour)
        {
            foreach(var brk in _dataContext.Breaks.Where(b => b.HourId == hour.Id))
            {
                _dataContext.Breaks.Remove(brk);
            }
            _dataContext.SaveChanges();
            _dataContext.Hours.Remove(hour);
            _dataContext.SaveChanges();
        }

        public List<Hour> GetHours()
        {
            return _dataContext.Hours.ToList();
        }

        public Hour GetHour(int id)
        {
            return _dataContext.Hours.FirstOrDefault(h => h.Id == id);
        }

        public Break GetBreak(int id)
        {
            return _dataContext.Breaks.FirstOrDefault(b => b.Id == id);
        }

        public void ApproveHour(Hour hour)
        {
            _dataContext.Entry(hour).State = EntityState.Modified;
            _dataContext.SaveChanges();
        }

        public void EmployeeApproveHour(Hour hour)
        {
            _dataContext.Entry(hour).State = EntityState.Modified;
            _dataContext.SaveChanges();
        }

        public List<JobCode> GetJobCodes()
        {
            return _dataContext.JobCodes.AsNoTracking().ToList();
        }

        public List<Login> GetLogins()
        {
            return _dataContext.Logins.ToList();
        }

        public PayPeriod AddPayPeriod(PayPeriod payPeriod)
        {
            _dataContext.PayPeriods.Add(payPeriod);
            _dataContext.SaveChanges();
            return payPeriod;
        }

        public List<PayPeriod> GetPayPeriods()
        {
            return _dataContext.PayPeriods.ToList();
        }

        public PayPeriod GetPayPeriod(int id)
        {
            return _dataContext.PayPeriods.FirstOrDefault(p => p.Id == id);
        }

        public Employee GetEmployee(int id)
        {
            return _dataContext.Employees.Include(h => h.Hours).ThenInclude(b => b.Breaks).Include(e => e.EmployeeJobCodes).ThenInclude(j => j.JobCode).FirstOrDefault(x => x.Id == id);
        }

        public Login GetLogin(string username)
        {
            return _dataContext.Logins.FirstOrDefault(u => u.UserName == username);
        }

        public Login GetLogin(int eId)
        {
            return _dataContext.Logins.FirstOrDefault(u => u.EmployeeID == eId);
        }

        public List<EmployeeJobCode> GetEmployeeJobCodes(int empId)
        {
            List<EmployeeJobCode> result = new List<EmployeeJobCode>();
            foreach (EmployeeJobCode jobCode in _dataContext.EmployeeJobCodes)
            {
                if (jobCode.EmployeeId == empId)
                {
                    result.Add(jobCode);
                }
            }

            return result;
        }

        public void UpdateEmployee(Employee e)
        {
            _dataContext.Employees.Update(e);

            PasswordHasher<Login> passwordHasher = new PasswordHasher<Login>();

            // Needed to make a clone of the object as when trying to run the GetLogin() method, it caused the reference of the employee to update.
            Login eLogin = new Login();
            eLogin = e.Login;

            Login login = this.GetLogin(e.Id);
            login.UserName = eLogin.UserName;
            login.PasswordHash = passwordHasher.HashPassword(eLogin, eLogin.PasswordHash);
            login.Admin = eLogin.Admin;

            _dataContext.SaveChanges();
        }

        public void UpdatePassword(Login l)
        {
            _dataContext.Logins.Update(l);
            _dataContext.SaveChanges();
        }

        public JobCode GetJobCode(int id)
        {
            return _dataContext.JobCodes.FirstOrDefault(x => x.Id == id);
        }

        public void UpdateEmployeeJobCode(EmployeeJobCode e)
        {
            EmployeeJobCode ejc = new EmployeeJobCode();
            ejc = e;
            EmployeeJobCode jc = this.GetEmployeeJobCode(e.Id);

            jc.Active = ejc.Active;
            jc.PayRate = ejc.PayRate;
            jc.JobCodeId = ejc.JobCodeId;
            _dataContext.SaveChanges();
        }

        public Employee GetEmployeeFromString(string name)
        {
            return _dataContext.Employees.Include(e => e.EmployeeJobCodes).ThenInclude(h => h.Hours).FirstOrDefault(x => x.FirstName + ' ' + x.LastName == name);
        }

        public void AddHours(Hour hour)
        {
            Employee e = _dataContext.Employees.Include(jc => jc.EmployeeJobCodes).ThenInclude(h => h.Hours).FirstOrDefault(x => x.Id == hour.EmployeeId);
            EmployeeJobCode ejc = this.GetEmployeeJobCode(hour.EmployeeJobCodeId);

            hour.PayPeriodId = _dataContext.PayPeriods.FirstOrDefault(x => x.End > hour.TimeIn && x.Start < hour.TimeIn).Id;

            _dataContext.Hours.Add(hour);
            _dataContext.SaveChanges();
        }

        public void EndHours(int employeeId, string tz)
        {
            Employee e = _dataContext.Employees.Include(h => h.Hours).FirstOrDefault(x => x.Id == employeeId);
            EmployeeJobCode ejc = _dataContext.EmployeeJobCodes.FirstOrDefault(x => x.EmployeeId == employeeId && x.Active == true);
            Hour h = e.Hours.Last();

            Hour hour = this.GetHour(h.Id);
            hour.TimeOut = DateTime.UtcNow.AddMinutes(-int.Parse(tz));
            hour.TimeEntered = DateTime.UtcNow.AddMinutes(-int.Parse(tz));

            _dataContext.Hours.Update(hour);
            _dataContext.Employees.Update(e);
            _dataContext.EmployeeJobCodes.Update(ejc);
            _dataContext.SaveChanges();
        }

        public void UpdateHourComment(int hourId, string Comment)
        {
            Hour h = this.GetHour(hourId);
            h.Comment = Comment;
            _dataContext.SaveChanges();
        }

        public void UpdateBreakComment(int breakId, string Comment)
        {
            Break b = this.GetBreak(breakId);
            b.Comment = Comment;
            _dataContext.SaveChanges();
        }
    }
}

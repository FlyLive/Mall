using Mall.Data.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mall.Web.Data.Services
{
    public class AdminService : IDisposable
    {
        private MallDBContext _db;
        public AdminService()
        {
            _db = new MallDBContext();
        }
        //新增员工
        public bool AddEmployee(string account,string name,string password)
        {
            if (!ReName(account))
            {
                User user = new User
                {
                    Account = account,
                    NickName = name,
                    Password = password
                };

                _db.User.Add(user);
                _db.Employee.Add(
                    new Employee
                    {
                        UserId = user.UserId
                    }
                );
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        //是否重名
        public bool ReName(string account)
        {
            var ul = GetAllEmployee()
                .Where(employee => employee.User.Account == account)
                .SingleOrDefault();
            if (ul != null)
                return true;
            return false;
        }
        //登录
        public Employee Login(string account, string password)
        {
            Employee employee = GetEmployeeByAccount(account);
            if (employee != null)
            {
                if (employee.User.Password == password)
                    return employee;
            }
            return null;
        }
        //返回所有员工
        public List<Employee> GetAllEmployee()
        {
            return _db.Employee.ToList();
        }
        //通过员工账户返回员工
        public Employee GetEmployeeByAccount(string account)
        {
            Employee employee = GetAllEmployee()
                .Where(u => u.User.Account == account)
                .SingleOrDefault();
            return employee;
        }
        //通过员工Id返回员工
        public Employee GetEmployeeByUserId(int userId)
        {
            Employee user = _db.Employee
                .Where(u => u.UserId == userId)
                .SingleOrDefault();
            return user;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}

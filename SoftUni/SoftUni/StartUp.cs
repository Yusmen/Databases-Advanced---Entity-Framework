
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var softUniContext = new SoftUniContext())
            {
                Console.WriteLine(GetEmployee147(softUniContext));
            }


        }
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder str = new StringBuilder();
            var employee147 = context.Employees.FirstOrDefault(x => x.EmployeeId == 147);

            str.AppendLine($"{employee147.FirstName} {employee147.LastName} - {employee147.JobTitle}");

            var projects = employee147.EmployeesProjects
                .Select(x => new
                {
                    ProjectName = x.Project.Name

                })
                .OrderBy(x => x.ProjectName)
                .ToList();

            foreach (var project in projects)
            {

                str.AppendLine($"{project.ProjectName}");

            }


            return str.ToString();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {

            StringBuilder str = new StringBuilder();

            var address = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4

            };
            //context.Addresses.Add(address); --Entity Framework automatically adds new address

            var nakov = context.Employees.FirstOrDefault(x => x.LastName == "Nakov");
            nakov.Address = address;
            context.SaveChanges();

            var employees = context.Employees
                .Select(x => new
                {
                    AddressText = x.Address.AddressText,
                    AddressId = x.AddressId


                })
                .OrderByDescending(x => x.AddressId)
                .Take(10)
                .ToArray();

            foreach (var employee in employees)
            {
                str.AppendLine($"{employee.AddressText}");
            }


            return str.ToString();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {

            StringBuilder str = new StringBuilder();

            var employees = context.Employees
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    DepartmentName = x.Department.Name,
                    Salary = x.Salary

                })
                .Where(x => x.DepartmentName == "Research and Development")
                .OrderBy(x => x.Salary)
                .ThenByDescending(x => x.FirstName)
                .ToArray();


            foreach (var employee in employees)
            {
                str.AppendLine($"{employee.FirstName} {employee.LastName} from Research and Development - {employee.Salary:f2}");

            }

            return str.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder str = new StringBuilder();

            var employees = context.Employees
                .Select(x => new Employee
                {
                    FirstName = x.FirstName,
                    Salary = x.Salary

                })
                .Where(x => x.Salary > 50000)
                .OrderBy(x => x.FirstName);


            foreach (var employee in employees)
            {
                str.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");

            }

            return str.ToString().TrimEnd();
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(x => new
                {
                    x.FirstName,
                    x.MiddleName,
                    x.LastName,
                    x.JobTitle,
                    x.Salary,
                    x.EmployeeId
                })
                .OrderBy(x => x.EmployeeId);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var employee in employees)
            {
                stringBuilder.AppendLine($"{employee.FirstName} {employee.MiddleName} {employee.LastName} {employee.JobTitle} {employee.Salary:f2}");
            }

            return stringBuilder.ToString().TrimEnd();


        }
    }
}

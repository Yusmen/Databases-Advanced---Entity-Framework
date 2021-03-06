﻿
using Microsoft.EntityFrameworkCore;
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
                Console.WriteLine(DeleteProjectById(softUniContext));

            }


        }
        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var project = context.Projects
                .FirstOrDefault(x => x.ProjectId == 2);

            var employeeProjects = context.EmployeesProjects
                .Where(x => x.ProjectId == 2);

            context.EmployeesProjects.RemoveRange(employeeProjects);

            //foreach (var employeeProject in employeeProjects)
            //{
            //    context.EmployeesProjects.Remove(employeeProject);
            //}

            context.Projects.Remove(project);

            context.SaveChanges();

            var projects = context.Projects
                .Select(x=>new { Name=x.Name})
                .Take(10)
                .ToList();

            foreach (var proj in projects)
            {
                sb.AppendLine($"{proj.Name}");
            }


            return sb.ToString().TrimEnd();

        }
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {

            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(x => EF.Functions.Like(x.FirstName, "sa%"))
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    JobTitle = x.JobTitle,
                    Salary = x.Salary

                })
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - ({employee.Salary:f2})");
            }

            return sb.ToString();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departments = context.Departments
                .Where(x => x.Employees.Count > 5)
                .OrderBy(x => x.Employees.Count)
                .ThenBy(x => x.Name)
                .Select(x => new
                {
                    DepartmentName = x.Name,
                    ManagerFullName = x.Manager.FirstName + " " + x.Manager.LastName,
                    Employees = x.Employees.Select(e => new
                    {
                        EmployeeFullName = e.FirstName + " " + e.LastName,
                        JobTitle = e.JobTitle

                    })
                    .OrderBy(x => x.EmployeeFullName)
                    .ToList()

                })
                .ToList();

            foreach (var department in departments)
            {
                sb.AppendLine($"{department.DepartmentName} {department.ManagerFullName}");

                foreach (var employee in department.Employees)
                {
                    sb.AppendLine($"{employee.EmployeeFullName} {employee.JobTitle}");
                }

            }



            return sb.ToString();
        }
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder str = new StringBuilder();
            var employee147 = context.Employees.FirstOrDefault(x => x.EmployeeId == 147);

            str.AppendLine($"{employee147.FirstName} {employee147.LastName} - {employee147.JobTitle}");

            var projects = context.EmployeesProjects
                .Where(x => x.EmployeeId == 147)
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

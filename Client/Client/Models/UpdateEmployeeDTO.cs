﻿namespace Client.Models
{
    public class UpdateEmployeeDTO
    {
        public string Id { get; set; }
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public int JobId { get; set; }

        public string JobTitle { get; set; }

        public double BaseSalary { get; set; }

        public string Department { get; set; }

        public Manager Manager { get; set; }

        public string EndDate { get; set; }

        public double Salary { get; set; }

        public string EmploymentType { get; set; }
    }
}

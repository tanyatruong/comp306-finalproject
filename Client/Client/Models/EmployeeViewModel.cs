﻿namespace Client.Models
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public JobViewModel Job { get; set; }
        public string HireDate { get; set; }
        public string EndDate { get; set; }
        public double Salary { get; set; }
        public string EmploymentType { get; set; }
    }
}



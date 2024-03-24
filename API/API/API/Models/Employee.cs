using Amazon.DynamoDBv2.DataModel;


namespace API.Models
{
	[DynamoDBTable("Employees")]
	public class Employee
	{
		[DynamoDBHashKey("employeeId")]
		public int EmployeeId { get; set; }

		[DynamoDBProperty("firstName")]
		public string FirstName { get; set; }

		[DynamoDBProperty("lastName")]
		public string LastName { get; set; }

		[DynamoDBProperty("email")]
		public string Email { get; set; }

		[DynamoDBProperty("phoneNumber")]
		public string PhoneNumber { get; set; }

		[DynamoDBProperty("address")]
		public string Address { get; set; }

		[DynamoDBProperty("job")]
		public Job Job { get; set; }

		[DynamoDBProperty("hireDate")]
		public string HireDate { get; set; }

		[DynamoDBProperty("endDate")]
		public string EndDate { get; set; }

		[DynamoDBProperty("salary")]
		public double Salary { get; set; }

		[DynamoDBProperty("employmentType")]
		public string EmploymentType { get; set; }



	}
}

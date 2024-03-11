using Amazon.DynamoDBv2.DataModel;


namespace API.Models
{
	[DynamoDBTable("Jobs")]
	public class Job
	{
		[DynamoDBHashKey("jobId")]
		public int JobId { get; set; }

		[DynamoDBProperty("jobTitle")]
		public string JobTitle { get; set; }

		[DynamoDBProperty("baseSalary")]
		public double BaseSalary { get; set; }

		[DynamoDBProperty("department")]
		public string Department { get; set; }

		[DynamoDBProperty("manager")]
		public Manager Manager { get; set; }
	}
}

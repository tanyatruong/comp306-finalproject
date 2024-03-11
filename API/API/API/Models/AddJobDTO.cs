using Amazon.DynamoDBv2.DataModel;


namespace API.Models
{
	public class AddJobDTO
	{
		public string JobTitle { get; set; }

		public double BaseSalary { get; set; }

		public string Department { get; set; }

		public Manager Manager { get; set; }
	}
}

namespace Client.Models
{
    public class JobViewModel
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public double BaseSalary { get; set; }
        public string Department { get; set; }
        public ManagerViewModel Manager { get; set; }
    }
}

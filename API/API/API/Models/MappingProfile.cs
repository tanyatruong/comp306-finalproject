using Amazon;
using AutoMapper;
using Profile = AutoMapper.Profile;

namespace API.Models
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<EmployeeDTO, Employee>();
			CreateMap<Employee, EmployeeDTO>();
			CreateMap<AddEmployeeDTO, Employee>();
			CreateMap<Employee, AddEmployeeDTO>();
			CreateMap<UpdateEmployeeDTO, Employee>();
			CreateMap<Employee, UpdateEmployeeDTO>();


			CreateMap<Job, JobDTO>();
			CreateMap<JobDTO, Job>();
			CreateMap<AddJobDTO, Job>();
			CreateMap<Job, AddJobDTO>();
			CreateMap<UpdateJobDTO, Job>();
			CreateMap<Job, UpdateJobDTO>();
		}
	}
}

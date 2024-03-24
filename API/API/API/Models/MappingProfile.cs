using Amazon;
using AutoMapper;
using Profile = AutoMapper.Profile;

namespace API.Models
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<EmployeeDTO, Employee>().ForMember(dest => dest.Job, act => act.MapFrom(src => new Job
			{
				JobId = src.JobId,
				JobTitle = src.JobTitle,
				BaseSalary = src.BaseSalary,
				Department = src.Department,
				Manager = src.Manager
			}));
			CreateMap<Employee, EmployeeDTO>().ForMember(dest => dest.JobId, act => act.MapFrom(src => src.Job.JobId)).ForMember(dest => dest.JobTitle, act => act.MapFrom(src => src.Job.JobTitle)).ForMember(dest => dest.BaseSalary, act => act.MapFrom(src => src.Job.BaseSalary)).ForMember(dest => dest.Department, act => act.MapFrom(src => src.Job.Department)).ForMember(dest => dest.Manager, act => act.MapFrom(src => src.Job.Manager));
			CreateMap<AddEmployeeDTO, EmployeeDTO>();
			CreateMap<EmployeeDTO, AddEmployeeDTO>();
			CreateMap<UpdateEmployeeDTO, Employee>().ForMember(dest => dest.Job, act => act.MapFrom(src => new Job
			{
				JobId = src.JobId,
				JobTitle = src.JobTitle,
				BaseSalary = src.BaseSalary,
				Department = src.Department,
				Manager = src.Manager
			}));
			CreateMap<EmployeeDTO, UpdateEmployeeDTO>();


			CreateMap<Job, JobDTO>();
			CreateMap<JobDTO, Job>();
			CreateMap<AddJobDTO, Job>();
			CreateMap<Job, AddJobDTO>();
			CreateMap<UpdateJobDTO, Job>();
			CreateMap<Job, UpdateJobDTO>();
		}
	}
}

using AutoMapper;
using Demo.BLL.DTOS.DepartmentModule;
using Demo.BLL.DTOS.EmployeeDTOS;
using Demo.DAL.Models;
namespace Demo.BLL.Mapping
{
    public class ProfileMapping : Profile


    {
        public ProfileMapping()
        {

            DepartmentMapping();
            EmployeeMapping();

        }

        private void DepartmentMapping()
        {
            // Department → DepartmentDto
            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.DateOfCreation,
                           opt => opt.MapFrom(src =>
                               src.CreatedOn.HasValue
                                   ? DateOnly.FromDateTime(src.CreatedOn.Value)
                                   : default));
            // Department → DepartmentDetailsDto
            CreateMap<Department, DepartmentDetailsDto>()
                .ForMember(dest => dest.CreatedOn,
                           opt => opt.MapFrom(src =>
                               src.CreatedOn.HasValue
                                   ? DateOnly.FromDateTime(src.CreatedOn.Value)
                                   : default))
                .ForMember(dest => dest.ModifiedOn,
                           opt => opt.MapFrom(src =>
                               src.ModifiedOn.HasValue
                                   ? DateOnly.FromDateTime(src.ModifiedOn.Value)
                                   : default));

            // CreateDepartmentDto → Department
            CreateMap<CreateDepartmentDto, Department>()
                .ForMember(dest => dest.CreatedOn,
                           opt => opt.MapFrom(src =>
                               src.DateOfCreation.ToDateTime(new TimeOnly())));

            // UpdateDepartmentDto → Department
            CreateMap<UpdateDepartmentDto, Department>()
                .ForMember(dest => dest.CreatedOn,
                           opt => opt.MapFrom(src =>
                               src.DateOfCreation.ToDateTime(new TimeOnly())));
        }

        private void EmployeeMapping()
        {
            CreateMap<Employee, EmployeeDto>().
                ForMember(dest => dest.Gender, options => options.MapFrom(src => src.Gender))
                .ForMember(dest => dest.EmployeeTypes, options => options.MapFrom(src => src.EmployeeType))
                             .ForMember(dest => dest.Department,
                       opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null));
            CreateMap<Employee, EmployeeDetailsDTO>()
                                    .ForMember(dest => dest.PhotoName,
                                        opt => opt.MapFrom(src => src.PhotoName))
                                    .ForMember(dest => dest.Gender,
                                        opt => opt.MapFrom(src => src.Gender))
                                    .ForMember(dest => dest.EmployeeTypes,
                                        opt => opt.MapFrom(src => src.EmployeeType))
                                    .ForMember(dest => dest.HiringDate,
                                        opt => opt.MapFrom(src => DateOnly.FromDateTime(src.HiringDate)))
                                    .ForMember(dest => dest.Department,
                                        opt => opt.MapFrom(src =>
                                            src.Department != null ? src.Department.Name : null
                                        ));

            CreateMap<CreateEmployeeDTO, Employee>()
                .ForMember(dest => dest.HiringDate, options => options.MapFrom(src => src.HiringDate.ToDateTime(TimeOnly.MinValue)));
            CreateMap<UpdateEmployeeDto, Employee>()
                .ForMember(dest => dest.HiringDate, options => options.MapFrom(src => src.HiringDate.ToDateTime(TimeOnly.MinValue)));

        }
    }
}

namespace Demo.BLL.Services.classes
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #region Get All Departments

        public IEnumerable<DepartmentDto> GetAllDepartments(string? DepartmentSearchName = null, bool withTracking = false)
        {
            var repo = _unitOfWork.Repository<Department>();
            IEnumerable<Department> departments;

            if (!string.IsNullOrWhiteSpace(DepartmentSearchName))
            {
                departments = repo.GetALL(
                    d => EF.Functions.Like(d.Name, $"%{DepartmentSearchName}%"),
                    withTracking
                );
            }
            else
            {
                departments = repo.GetAll(withTracking);
            }

            var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
            return departmentDtos;
        }
        #endregion
        #region Get By Id
        public DepartmentDetailsDto? GetDepartmentById(int id)
        {
            var department = _unitOfWork.Repository<Department>().GetById(id);
            return _mapper.Map<DepartmentDetailsDto>(department);
        }
        #endregion
        #region Add Or Create 
        public int AddDepartment(CreateDepartmentDto departmentDto)
        {
            var department = _mapper.Map<Department>(departmentDto);
            _unitOfWork.Repository<Department>().Add(department);
            return _unitOfWork.SaveChanges();
        }

        #endregion
        #region Update 
        public int UpdateDepartment(UpdateDepartmentDto departmentDto)
        {
            var department = _mapper.Map<Department>(departmentDto);
            _unitOfWork.Repository<Department>().Update(department);
            return _unitOfWork.SaveChanges();

        }
        #endregion
        #region Remove
        public bool DeleteDepartment(int id)
        {
            var Repo = _unitOfWork.Repository<Department>();

            var department = Repo.GetById(id);
            if (department is null)
                return false;

            Repo.Remove(department);
            return _unitOfWork.SaveChanges() > 0;
        }
        #endregion

    }
}

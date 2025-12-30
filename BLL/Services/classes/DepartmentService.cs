
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
        public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync(string? DepartmentSearchName = null, bool withTracking = false)
        {
            var repo = _unitOfWork.Repository<Department>();
            IEnumerable<Department> departments;

            if (!string.IsNullOrWhiteSpace(DepartmentSearchName))
            {
                // Using the Async version of GetALL (predicate based)
                departments = await repo.GetAllAsync(
                    d => EF.Functions.Like(d.Name, $"%{DepartmentSearchName}%"),
                    withTracking
                );
            }
            else
            {
                // Using the Async version of GetAll
                departments = await repo.GetAllAsync(withTracking);
            }

            return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
        }
        #endregion

        #region Get By Id
        public async Task<DepartmentDetailsDto?> GetDepartmentByIdAsync(int id)
        {
            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(id);
            return _mapper.Map<DepartmentDetailsDto>(department);
        }
        #endregion

        #region Add Or Create 
        public async Task<int> AddDepartmentAsync(CreateDepartmentDto departmentDto)
        {
            var department = _mapper.Map<Department>(departmentDto);

            // Note: AddAsync is used here
            await _unitOfWork.Repository<Department>().AddAsync(department);

            return await _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region Update 
        public async Task<int> UpdateDepartmentAsync(UpdateDepartmentDto departmentDto)
        {
            var department = _mapper.Map<Department>(departmentDto);

            // Update in EF is a local state change, but we save asynchronously
            _unitOfWork.Repository<Department>().Update(department);

            return await _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region Remove
        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var repo = _unitOfWork.Repository<Department>();

            var department = await repo.GetByIdAsync(id);
            if (department is null)
                return false;

            repo.Remove(department);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        #endregion
    }
}
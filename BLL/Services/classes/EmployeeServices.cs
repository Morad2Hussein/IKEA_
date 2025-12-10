namespace Demo.BLL.Services.classes
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EmployeeServices( IMapper mapper, IUnitOfWork unitOfWork )
        {
            
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        #region Get All Data
        public IEnumerable<EmployeeDto> GetAllEmployee(string? EmployeeSearchName, bool withTracking = false)
        {
            var Repo = _unitOfWork.Repository<Employee>();
            IEnumerable<Employee> employees;
            //me == > mohaned , ahmed
            if (!String.IsNullOrWhiteSpace(EmployeeSearchName))
                employees = _unitOfWork.Repository<Employee>().GetALL(e => e.Name.ToLower().Contains(EmployeeSearchName.ToLower()));
            else
                employees = Repo.GetAll(withTracking);
            var employeesDto = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDto>>(employees);

            return employeesDto;
        } 
        #endregion
        #region GetBy ID
        public EmployeeDetailsDTO? GetEmployeeById(int id)
        {
            if (id <= 0)
                return null;
            var employee = _unitOfWork.Repository<Employee>().GetById(id);
            var employeeDto = _mapper.Map<Employee, EmployeeDetailsDTO>(employee!);
            return employeeDto;
        } 
        #endregion
        #region Create
        public int CreateEmployee(CreateEmployeeDTO createEmployee)
        {
            var employee = _mapper.Map<CreateEmployeeDTO, Employee>(createEmployee);
            // Source: createEmployee  Destination: employee  
            _unitOfWork.Repository<Employee>().Add(employee);
            return _unitOfWork.SaveChanges();
        } 
        #endregion
        #region Edit Or Update
        public int UpdateEmployee(UpdateEmployeeDto updateEmployee)
        {
            var employee = _mapper.Map<UpdateEmployeeDto, Employee>(updateEmployee);
            // Source: updateEmployee  Destination: employee  
            _unitOfWork.Repository<Employee>().Update(employee);
            return _unitOfWork.SaveChanges();
        }

        #endregion
        #region Delete
        public bool DeleteEmployee(int id)
        {
            var Repo = _unitOfWork.Repository<Employee>();

            var employee = Repo.GetById(id);
            if (employee == null)
                return false;
            else
            {
                employee.IsDeleted = true;
                Repo.Update(employee);
                return true;
            }
        } 
        #endregion


    }
}

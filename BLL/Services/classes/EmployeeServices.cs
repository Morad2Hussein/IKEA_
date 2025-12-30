using BLL.Services.AttachmentServices;
using Demo.BLL.DTOS.EmployeeDTOS;
using Demo.BLL.Services.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.BLL.Services.classes
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentServices _attachmentServices;

        public EmployeeServices(IMapper mapper, IUnitOfWork unitOfWork, IAttachmentServices attachmentServices)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _attachmentServices = attachmentServices;
        }

        #region Get All Data
        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeeAsync(string? EmployeeSearchName, bool withTracking = false)
        {
            var repo = _unitOfWork.Repository<Employee>();
            IEnumerable<Employee> employees;

            if (!string.IsNullOrWhiteSpace(EmployeeSearchName))
            {
                // Using the Async version of GetALL
                employees = await repo.GetAllAsync(e => e.Name.ToLower().Contains(EmployeeSearchName.ToLower()), withTracking);
            }
            else
            {
                // Using the Async version of GetAll
                employees = await repo.GetAllAsync(withTracking);
            }

            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }
        #endregion

        #region GetBy ID
        public async Task<EmployeeDetailsDTO?> GetEmployeeByIdAsync(int id)
        {
            if (id <= 0) return null;

            // Using FindAsync/GetByIdAsync
            var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(id);

            return _mapper.Map<EmployeeDetailsDTO>(employee!);
        }
        #endregion

        #region Create
        public async Task<int> CreateEmployeeAsync(CreateEmployeeDTO createEmployee)
        {
            var employee = _mapper.Map<Employee>(createEmployee);

            if (createEmployee.Photo is not null)
            {
                // Await the asynchronous file upload
                string? photoName = await _attachmentServices.UploadedAsync(createEmployee.Photo, "images");
                employee.PhotoName = photoName;
            }

            // Using AddAsync
            await _unitOfWork.Repository<Employee>().AddAsync(employee);

            // Using SaveChangesAsync
            return await _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region Edit Or Update
        public async Task<int> UpdateEmployeeAsync(UpdateEmployeeDto updateEmployee)
        {
            var employee = _mapper.Map<Employee>(updateEmployee);

            if (updateEmployee.Photo is not null)
            {
                // If you have a logic to delete the old photo, await it here first
                // await _attachmentServices.DeleteAsync("images", oldPhotoName);

                string? photoName = await _attachmentServices.UploadedAsync(updateEmployee.Photo, "images");
                employee.PhotoName = photoName;
            }

            // Update is usually synchronous in EF, but the save is async
            _unitOfWork.Repository<Employee>().Update(employee);

            return await _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region Delete
        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var repo = _unitOfWork.Repository<Employee>();
            var employee = await repo.GetByIdAsync(id);

            if (employee == null)
                return false;

            employee.IsDeleted = true;
            repo.Update(employee);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
        #endregion
    }
}
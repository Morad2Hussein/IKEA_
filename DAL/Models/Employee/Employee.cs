using Demo.BLL.DTOS.EmployeeDTOS.Common;

namespace Demo.DAL.Models
{
    public class Employee :BaseEntity
    {
        #region  Properties
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public Gender Gender { get; set; }
        public DateTime HiringDate { get; set; }
        public string? PhotoName { get; set; }
        #endregion
        #region Relationship
        public virtual Department? Department { get; set; }
        public  int? DepartmentId { get; set; }
        #endregion

    }
}

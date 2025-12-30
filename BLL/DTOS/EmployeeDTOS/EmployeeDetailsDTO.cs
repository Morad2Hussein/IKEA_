
namespace Demo.BLL.DTOS.EmployeeDTOS
{
    public class EmployeeDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public string? Email { get; set; }
        public int Age { get; set; }
        public decimal Salary { get; set; }
        public string? EmployeeTypes { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateOnly HiringDate { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? Gender { get; set; }
        public int? DepartmentId { get; set; }
        public string? Department { get; set; }
        public string? PhotoName { get; set; }
    }
}

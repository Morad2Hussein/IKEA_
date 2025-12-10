

using System.ComponentModel.DataAnnotations;

namespace Demo.BLL.DTOS.EmployeeDTOS
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [EmailAddress]
        public string? Email { get; set; }
        [Display (Name = "IS Active")]
        public bool IsActive { get; set; }

        public int Age { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [Display(Name ="Employee Types ")]
        public string EmployeeTypes { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;

        public string? Department { get; set; }   


    }
}

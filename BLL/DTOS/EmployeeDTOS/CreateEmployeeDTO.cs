
using Demo.BLL.DTOS.EmployeeDTOS.Common;
using System.ComponentModel.DataAnnotations;

namespace Demo.BLL.DTOS.EmployeeDTOS
{
    public class CreateEmployeeDTO
    {
        #region  properties
        #region Name
        [Required(ErrorMessage = "The Name Is Required")]
        [MaxLength(50, ErrorMessage = "The Max Length is 50 characters.")]
        [MinLength(3, ErrorMessage = "The Min Length is 3 characters.")]
        public string Name { get; set; } = null!;

        #endregion
        #region Age
        [Required(ErrorMessage ="The Age is Required")]
        [Range(22, 35)]
        public int Age { get; set; }
        #endregion
        #region Addrees
        [RegularExpression(
     @"^\d{1,3}-[A-Za-z]{3,20}-[A-Za-z]{3,20}-[A-Za-z]{3,20}$",
     ErrorMessage = "Address must be like 123-Street-City-Country")]
        public string? Address { get; set; }

        #endregion
        [EmailAddress]
        // Name - Email - Address  - PhoneNumber - Salary - IsActive - EmployeeType - Gender - HiringDate  - DepartmentId
        public string? Email { get; set; }
        [Display(Name = " Phone Number")]
        [Phone]
        public string? PhoneNumber { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [Display(Name = "Employee Types ")]
        public EmployeeType EmployeeType { get; set; }
        public Gender Gender { get; set; }
        [Display(Name = "Hiring Date")]
        public DateOnly HiringDate { get; set; }
        #endregion
        [Display(Name ="Department")]
        public  int? DepartmentId { get; set; }


    }
}

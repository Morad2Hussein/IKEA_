using System.ComponentModel.DataAnnotations;

namespace Demo.BLL.DTOS.DepartmentModule
{
    public class CreateDepartmentDto
    {
        [Required (ErrorMessage ="Name Is Required  please Enter Your Name ")]
        public string Name { get; set; } = string.Empty;
        [Required (ErrorMessage ="Code Is Required  please Enter Your Code ")]
        public string Code { get; set; } = string.Empty;

        public string? Description { get; set; } 
        
        public DateOnly DateOfCreation { get; set; }

    }
}

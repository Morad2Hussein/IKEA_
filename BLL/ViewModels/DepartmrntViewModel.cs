

namespace Demo.BLL.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } =  string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } 
        public DateOnly CreatedOn { get; set; }
    }
}

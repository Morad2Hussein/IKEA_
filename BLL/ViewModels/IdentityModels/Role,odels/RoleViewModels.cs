

namespace BLL.ViewModels.IdentityModels.Role_odels
{
    public class RoleViewModels
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public List<UserRoleViewModels> users  { get; set; } = new List<UserRoleViewModels>();
        public RoleViewModels() { 
         Id = Guid.NewGuid().ToString();
        }
    }
}

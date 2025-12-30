
using System.ComponentModel.DataAnnotations;

namespace BLL.ViewModels.IdentityModels
{
    public class ForgotPasswordViewModel
    {
        [Required (ErrorMessage ="Email Can Not Be Empty")]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}

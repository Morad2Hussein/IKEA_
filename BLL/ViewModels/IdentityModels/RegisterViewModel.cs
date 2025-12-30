using System.ComponentModel.DataAnnotations;

namespace BLL.ViewModels.IdentityModels
{
    public class RegisterViewModel
    {
       
        [Required(ErrorMessage = "Username can not be null")]
        [MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Firstname can not be null")]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "Lastname can not be null")]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
        public bool IsAgreed { get; set; }

    }
}

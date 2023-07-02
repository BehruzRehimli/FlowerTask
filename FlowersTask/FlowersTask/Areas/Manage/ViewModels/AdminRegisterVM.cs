using System.ComponentModel.DataAnnotations;

namespace FlowersTask.Areas.Manage.ViewModels
{
    public class AdminRegisterVM
    {
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(30)]
        public string Username { get; set; }
        [Required]
        [MaxLength(80)]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [Required]
        [MaxLength(15)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [MaxLength(15)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [Required]
        public List<string> RolesIds { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace FlowersTask.Areas.Manage.ViewModels
{
    public class AdminLoginVM
    {

        [Required]
        [MaxLength(30)]
        public string UserName { get; set; }
        [Required]
        [StringLength(15)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowersTask.Models
{
    public class AppUser:IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }
        [NotMapped]
        public List<string> RolesIds { get; set; }=new List<string>();

    }
}

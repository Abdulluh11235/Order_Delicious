using System.ComponentModel.DataAnnotations;

namespace Application;

public class AddRoleModel
{
   [Required] 
   public string UserId { get; set; } = null!;
   [Required] 
   public string RoleName { get; set; } = null!;
}
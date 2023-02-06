using System.ComponentModel.DataAnnotations;

namespace empmgmt.Models
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        
        [Required]
        [StringLength(4)]
        public string CaptchaCode { get; set; }
    }
}

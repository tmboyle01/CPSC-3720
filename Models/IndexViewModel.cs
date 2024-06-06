using System.ComponentModel.DataAnnotations;

namespace TigerTix.Models
{
    public class IndexViewModel
    {
        [Required(ErrorMessage = "CUID is required")]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }


    }
}


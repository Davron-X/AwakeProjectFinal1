using System.ComponentModel.DataAnnotations;

namespace Awake_Models
{
    public class ApplicationType
    {
        [Key]
        public int Id { get; set; }
        [Display(Name="Название")]
        [Required]
        [Length(2,80)]
        public string Name { get; set; }
    }
}

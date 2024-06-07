using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Awake_Models
{
    public class Category
    {
        public int Id { get; set; }
        [DisplayName("Название")]
        [Required]
        [Length(2,80)]
        public string? Name { get; set; }
        [DisplayName("Порядок отображения")]
        [Required]
        [Range(minimum:1,int.MaxValue)]
        public int DisplayOrder { get; set; }
    }
}

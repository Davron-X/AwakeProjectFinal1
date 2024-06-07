using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Awake_Models
{
    public class Product
    {
        public Product()
        {
            TempQuantity = 1;
        }
        public int Id { get; set; }
        [Required]
        [DisplayName("Название")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Описание")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required]
        [DisplayName("Короткое описание")]
        [DataType(DataType.MultilineText)]
        public string ShortDesc { get; set; }
        public string? Image { get; set; }
        [Required]
        [Range(1,double.MaxValue-1)]
        [DisplayName("Цена")]
        public double Price { get; set; }
        [DisplayName("Категория продукта")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        [DisplayName("Тип продукта")]
        public int ApplicationTypeId { get; set; }
        [ForeignKey("ApplicationTypeId")]
        public ApplicationType? ApplicationType { get; set; }
        [NotMapped]
        [Range(1,1000)]
        public int TempQuantity { get; set; }
    }
  
}

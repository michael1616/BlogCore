using System.ComponentModel.DataAnnotations;

namespace BlogCore.Models
{
    public class Category
    {
        [Key]
        public int IdCategory { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [Display(Name = "Visualization Order")]
        public int? Order { get; set; }
    }
}

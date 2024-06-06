using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlogCore.Models
{
    public class Article
    {
        [Key]
        public int IdArticle { get; set; }

        [Required]
        [Display(Name = "Article Name")]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Creation Date")]
        public string CreationDate { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Slider
    {
        [Key]
        public int IdSlider { get; set; }

        [Required]
        public string Name { get; set; }

        public bool State { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MintWebRazor_Temp.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Category Name")]
        public string Name { get; set; }

        [DisplayName("Justice Order")]
        [Range(1, 100, ErrorMessage = "Justice Order must between 1-100 please.")]
        public int DisplayOrder { get; set; }
    }
}

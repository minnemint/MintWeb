using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mint.Models
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
        [Range(1, 100,ErrorMessage ="Justice Order must between 1-100 please.")]
        public int DisplayOrder { get; set; }
    }
}

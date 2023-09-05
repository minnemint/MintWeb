using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mint.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(150, ErrorMessage = "Title can't be longer than 150 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author name is required.")]
        [MaxLength(100, ErrorMessage = "Author name can't be longer than 100 characters.")]
        public string Author { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "Description can't be longer than 1000 characters.")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value greater than 0")]
        public int StockCount { get; set; }

        [MaxLength(150)]
        public string Publisher { get; set; }

        [MaxLength(500)]
        public string CoverImageUrl { get; set; }

        // Assuming categories are defined in another table and are related to products.
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        // For reviews and ratings
        public virtual ICollection<Review> Reviews { get; set; }

        // For tracking any offers or discounts on the book
        public virtual ICollection<Offer> Offers { get; set; }
    }

    public class Review
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }

        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
    }

    public class Offer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(0.0, 100.0)]
        public double DiscountPercentage { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Product.Application.Dtos
{
    public record CreateProductDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
    }

    public record CreateProductResponseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public record ProductsResponse : CreateProductResponseDto
    {

    }
    public record UpdateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}

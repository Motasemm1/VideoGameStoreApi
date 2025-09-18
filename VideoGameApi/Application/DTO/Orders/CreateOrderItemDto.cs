using System.ComponentModel.DataAnnotations;

namespace VideoGameApi.Application.DTO.Orders
{
    public class CreateOrderItemDto

    {
        [Required]
        public int VideoGameId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be positive.")]
        public int Quantity { get; set; } = 1;
        }
}

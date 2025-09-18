using System.ComponentModel.DataAnnotations;

namespace VideoGameApi.Application.DTO.Orders
{
    public class CreateOrderDto
    {
        [Required]
        public List<CreateOrderItemDto> Items { get; set; }
    }

}

using Microsoft.AspNetCore.Mvc;
using Tangy_Business.Repository.IRepository;
using Tangy_Models;

namespace TangyWeb_API.Controllers;

[Route("api/[controller]/[action]")] // example of another approach using actions.
// in a real application you would stick to one or the other method
[ApiController]
public class OrderController : ControllerBase
{
	private readonly IOrderRepository _orderRepository;

	public OrderController(IOrderRepository orderRepository)
	{
		_orderRepository = orderRepository;
	}

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		return Ok(await _orderRepository.GetAll());
	}
	
	[HttpGet("{orderHeaderId:int}")]
	public async Task<IActionResult> Get(int? orderHeaderId)
	{
		if (orderHeaderId == null || orderHeaderId == 0)
		{
			return BadRequest(new ErrorModelDTO()
			{
				ErrorMessage = "Invalid Id",
				StatusCode = StatusCodes.Status400BadRequest
			});
		}

		var orderHeader = await _orderRepository.Get(orderHeaderId.Value); //we use .Value because int? productId is nullable.
		if (orderHeader == null)
		{
			return BadRequest(new ErrorModelDTO()
			{
				ErrorMessage = "Product not found",
				StatusCode = StatusCodes.Status404NotFound
			});
		}
		return Ok(orderHeader);
	}
}
﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tangy_Business.Repository.IRepository;
using Tangy_Common;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_DataAccess.ViewModels;
using Tangy_Models;

namespace Tangy_Business.Repository;

public class OrderRepository : IOrderRepository
{
	private readonly ApplicationDbContext _db;
	private readonly IMapper _mapper;

	public OrderRepository(ApplicationDbContext db, IMapper mapper)
	{
		_db = db;
		_mapper = mapper;
	}

	public async Task<OrderDTO> Get(int id)
	{
		var order = new Order
		{
			OrderHeader = await _db.OrderHeaders.FirstOrDefaultAsync(header => header.Id == id),
			OrderDetails = _db.OrderDetails.Where(detail => detail.OrderHeaderId == id)
		};

		return _mapper.Map<Order, OrderDTO>(order);
	}

	public async Task<IEnumerable<OrderDTO>> GetAll(string? userId = null, string? status = null)
	{
		List<Order> OrderFromDb = new List<Order>();
		IEnumerable<OrderHeader> orderHeaders = _db.OrderHeaders;
		IEnumerable<OrderDetail> orderDetails = _db.OrderDetails;

		foreach (var orderHeader in orderHeaders)
		{
			var order = new Order
			{
				OrderHeader = orderHeader,
				OrderDetails = _db.OrderDetails.Where(detail => detail.OrderHeaderId == orderHeader.Id)
			};
			OrderFromDb.Add(order);
		}
		// todo do some filtering

		return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(OrderFromDb);
	}

	public async Task<OrderDTO> Create(OrderDTO orderDTO)
	{
		try
		{
			var order = _mapper.Map<OrderDTO, Order>(orderDTO);
			_db.OrderHeaders.Add(order.OrderHeader);
			await _db.SaveChangesAsync();
			foreach (var orderOrderDetail in order.OrderDetails)
			{
				orderOrderDetail.OrderHeaderId = order.OrderHeader.Id;
				// _db.OrderDetails.Add(orderOrderDetail); we could add one by one in the loop
			}

			_db.OrderDetails.AddRange(order.OrderDetails); // or all together outside
			await _db.SaveChangesAsync();

			return new OrderDTO
			{
				OrderHeader = _mapper.Map<OrderHeader, OrderHeaderDTO>(order.OrderHeader),
				OrderDetails = _mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailDTO>>(order.OrderDetails)
					.ToList()
			};
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}

	public async Task<int> Delete(int id)
	{
		var objHeader = await _db.OrderHeaders.FirstOrDefaultAsync(header => header.Id == id);
		if (objHeader == null) return 0;
		var objDetail = _db.OrderDetails.Where(detail => detail.OrderHeaderId == id);
		_db.OrderDetails.RemoveRange(objDetail);
		_db.OrderHeaders.Remove(objHeader);
		return _db.SaveChanges();
	}

	public async Task<OrderHeaderDTO> UpdateHeader(OrderHeaderDTO headerDTO)
	{
		if (headerDTO != null)
		{
			var orderHeader = _mapper.Map<OrderHeaderDTO, OrderHeader>(headerDTO);
			_db.OrderHeaders.Update(orderHeader);
			await _db.SaveChangesAsync();
			return _mapper.Map<OrderHeader, OrderHeaderDTO>(orderHeader);
		}

		return new OrderHeaderDTO();
	}

	public async Task<OrderHeaderDTO> MarkPaymentSuccessful(int id)
	{
		var data = await _db.OrderHeaders.FindAsync(id);
		if (data == null)
		{
			return new OrderHeaderDTO();
		}

		if (data.Status == StaticDetails.StatusPending)
		{
			data.Status = StaticDetails.StatusConfirmed;
			await _db.SaveChangesAsync();
			return _mapper.Map<OrderHeader, OrderHeaderDTO>(data);
		}

		return new OrderHeaderDTO();
	}

	public async Task<bool> UpdateOrderStatus(int orderId, string status)
	{
		var data = await _db.OrderHeaders.FindAsync(orderId);
		if (data == null)
		{
			return false;
		}

		data.Status = status;
		if (status == StaticDetails.StatusShipped)
		{
			data.ShippingDate = DateTime.Now;
		}
		await _db.SaveChangesAsync();
		return true;
	}
}
﻿using System.ComponentModel.DataAnnotations;

namespace Tangy_Models;

public class OrderHeaderDTO
{
	[Key] public int Id { get; set; }

	[Required] public string UserId { get; set; }
	//todo add navigation property

	[Required]
	[Display(Name = "Order Total")]
	public double OrderTotal { get; set; }

	[Required] public DateTime OrderDate { get; set; }

	[Required]
	[Display(Name = "Shipping Date")]
	public DateTime ShippingDate { get; set; }

	public string Status { get; set; }

	// stripe payment
	public string? SessionId { get; set; }
	public string? PaymentIntentId { get; set; }


	[Required] [Display(Name = "Name")] public string Name { get; set; }

	[Required]
	[Display(Name = "Phone Number")]
	public string PhoneNumber { get; set; }

	[Required]
	[Display(Name = "Street Address")]
	public string StreetAddress { get; set; }

	[Required] public string State { get; set; }
	[Required] public string City { get; set; }

	[Required]
	[Display(Name = "Postal Code")]
	public string PostalCode { get; set; }

	public string Email { get; set; }
}
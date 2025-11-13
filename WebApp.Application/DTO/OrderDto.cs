using System;
using System.Collections.Generic;

namespace WebApp.Application;

public class OrderDto
{
    public int BuyerId  { get; set; }
    public int CarId { get; set; }
    public List<int> ServiceIds { get; set; } = new();
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Ожидание";
}
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApp.models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    
    public int CarId { get; set; }
    public Car? Cars { get; set; }
    
    public List<Service>? Services { get; set; } = new();
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Ожидание";
}
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApp.models;

public class Service
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Opisanie {get; set;}
    public int Price {get; set;}
    public List<Order>? Orders { get; set; } = new();
}
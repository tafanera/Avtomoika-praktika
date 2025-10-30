using Microsoft.EntityFrameworkCore;

namespace WebApp.models;
public class Car
{
    public int Id { get; set; }
    public string? Marka { get; set; }
    public string? Model { get; set; }
    public string? Number { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}
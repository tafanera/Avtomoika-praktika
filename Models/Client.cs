using Microsoft.EntityFrameworkCore;

namespace WebApp.models;
public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
}
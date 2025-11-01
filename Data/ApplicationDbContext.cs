using Microsoft.EntityFrameworkCore;
using server.Models;  // This imports your User model

namespace server.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    
    public DbSet<User> Users { get; set; }
}
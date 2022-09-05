using Microsoft.EntityFrameworkCore;
using WorkHelpApi.Models.Entities;
using System.Linq;

namespace WorkHelpApi.Core;

public class MySqlDbContext : DbContext
{
	private readonly DbContextOptions<MySqlDbContext> _options;

	public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
	{
		_options = options;
	}

	public DbSet<UserEntity> Users { get; set; }
	public DbSet<WorkEntity> Work { get; set; }
	public DbSet<RefferalEntity> Refferals { get; set; }
	public DbSet<PromocodeEntity> Promocodes { get; set; }
}

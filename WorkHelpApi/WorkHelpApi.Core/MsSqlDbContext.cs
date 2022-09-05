using Microsoft.EntityFrameworkCore;
using WorkHelpApi.Models.Entities;
using System.Linq;

namespace WorkHelpApi.Core;

public class MsSqlDbContext : DbContext
{
	private readonly DbContextOptions<MsSqlDbContext> _options;

	public MsSqlDbContext(DbContextOptions<MsSqlDbContext> options) : base(options)
	{
		_options = options;
	}

	public DbSet<UserEntity> Users { get; set; }
	public DbSet<WorkEntity> Work { get; set; }
	public DbSet<RefferalEntity> Refferals { get; set; }
	public DbSet<PromocodeEntity> Promocodes { get; set; }
}

/* using Microsoft.EntityFrameworkCore; */
/* using System.Configuration; */

/* using WorkHelpApi.Models.Entities; */

/* namespace WorkHelpApi.Core; */

/* public class MsSqlDbContext : DbContext */
/* { */
/* 	protected readonly IConfiguration _configuration; */

/* 	public MsSqlDbContext(IConfiguration configuration) */
/* 	{ */
/* 		_configuration = configuration; */
/* 	} */

/* 	protected override void OnConfiguring(DbContextOptionsBuilder options) */
/* 	{ */
/* 		options.UseSqlServer(_configuration.GetConnectionString("MsSqlApiConnection")); */
/* 	} */

/* 	public DbSet<UserEntity> Users { get; set; } */
/* 	public DbSet<WorkEntity> Work { get; set; } */
/* 	public DbSet<RefferalEntity> Refferals { get; set; } */
/* 	public DbSet<PromocodeEntity> Promocodes { get; set; } */
/* } */



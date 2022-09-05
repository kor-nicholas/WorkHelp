using WorkHelpApi.Core;

using WorkHelpApi.Abstractions;
using WorkHelpApi.Abstractions.Services;
using WorkHelpApi.Abstractions.Repositories;

using WorkHelpApi.Services;
using WorkHelpApi.Services.Services;
using WorkHelpApi.Services.Repositories;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI
builder.Services.AddTransient<IWorkService, WorkService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ICommonService, CommonService>();
builder.Services.AddTransient<IRefferalService, RefferalService>();
builder.Services.AddTransient<IPromocodeService, PromocodeService>();

builder.Services.AddTransient<IWorkRepository, WorkRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ICommonRepository, CommonRepository>();
builder.Services.AddTransient<IRefferalRepository, RefferalRepository>();
builder.Services.AddTransient<IPromocodeRepository, PromocodeRepository>();

// DB
/* builder.Services.AddDbContext<MySqlDbContext>(options => */ 
		/* options.UseMySql(builder.Configuration.GetConnectionString("MySqlApiConnection"), new MySqlServerVersion(new Version(8, 0, 27)))); */

builder.Services.AddDbContext<MsSqlDbContext>(options => 
		options.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlApiConnection")));

/* builder.Services.AddDbContext<MsSqlDbContext>(options => */ 
/* 		options.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlApiConnection2"))); */

// Registration/Autorization
/* builder.Services.AddDefaultIdentity<IdentityUser>(opt => */ 
/* 		opt.Password.RequireDigit = true, */
/* 		opt.Password.RequireLength = 8, */
/* 		opt.Password.RequireUppercase = true, */
/* 		opt.Lockout.MaxFailedAccessAttempts = 4, */
/* 		opt.User.RequireUniqueEmail = true, */
/* 		opt.SignIn.RequireConfirmedEmail = false) */
/* 	.AddEntityFrameworkStores<MySqlDbContext>(); */

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

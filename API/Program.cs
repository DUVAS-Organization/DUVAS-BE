using System.Text;
using API.Service;
using DUVAS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Repositories.IRepository;
using Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add STMP settings
            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
            builder.Services.AddTransient<EmailService>();
            builder.Services.AddSingleton<OtpService>();
            builder.Services.AddSingleton<JwtService>();
            builder.Services.AddSingleton<TokenDictionaryService>();
            builder.Services.AddScoped<TokenExchangeService>();
            // Add jwt filter
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
                    };
                });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("Landlord", policy => policy.RequireRole("Landlord"));
                options.AddPolicy("Service", policy => policy.RequireRole("Service"));
                options.AddPolicy("User", policy => policy.RequireRole("User"));
            });
            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add database context
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DBString")));

            //// Add repositories
            builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();
            builder.Services.AddScoped<ICategoryRoomRepository, CategoryRoomRepository>();
            builder.Services.AddScoped<ICategoryServiceRepository, CategoryServiceRepository>();
            builder.Services.AddScoped<IContractRepository, ContractRepository>();
            builder.Services.AddScoped<IOwnerLicenseRepository, OwnerLicenseRepository>();
            builder.Services.AddScoped<IRentalListRepository, RentalListRepository>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<IRoomLicenseRepository, RoomLicenseRepository>();
            builder.Services.AddScoped<IServiceFeedbackRepository, ServiceFeedbackRepository>();
            builder.Services.AddScoped<IServiceLicenseRepository, ServiceLicenseRepository>();
            builder.Services.AddScoped<IServicePostRepository, ServicePostRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserFeedbackRepository, UserFeedbackRepository>();

            // Add CORS policy for React app
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowReactApp");
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

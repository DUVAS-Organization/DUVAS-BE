using System.Text;
using API.Service;
using DUVAS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Repositories.IRepository;
using Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using API.Controllers;
using System.Text.Json.Serialization;
using Repository;
using Microsoft.AspNetCore.SignalR;
using API;
using DataAccess;
using Microsoft.OpenApi.Models;
using Repositories.Repositories;
using API.Hubs;
using API.Services;
using Utilities;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Diagnostics; // Thêm cho UseExceptionHandler
using Microsoft.Extensions.Logging; // Thêm cho ILogger

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            EncryptionHelper.Initialize(builder.Configuration);

            // Add STMP settings
            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
            builder.Services.AddTransient<EmailService>();
            builder.Services.AddSingleton<OtpService>();
            builder.Services.AddSingleton<JwtService>();
            builder.Services.AddSingleton<TokenDictionaryService>();
            builder.Services.AddScoped<TokenExchangeService>();

            // Add JWT authentication
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
                    };

                    // Cấu hình SignalR để sử dụng JWT
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;

                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/savedPostHub") || path.StartsWithSegments("/chathub")))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            // Add authorization policies
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("Landlord", policy => policy.RequireRole("Landlord"));
                options.AddPolicy("Service", policy => policy.RequireRole("Service"));
                options.AddPolicy("User", policy => policy.RequireRole("User"));
            });

            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSingleton<AiService>();
            builder.Services.AddSwaggerGen(options =>
            {
                options.OperationFilter<FileUploadOperationFilter>();

                // Thêm Bearer Authorization header vào Swagger UI
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter your Bearer token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // Add database context
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DBString")));

            // Thêm cấu hình Cloudinary 
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
            builder.Services.AddSingleton(serviceProvider =>
            {
                var config = serviceProvider.GetRequiredService<IOptions<CloudinarySettings>>().Value;
                if (string.IsNullOrEmpty(config.CloudName) || string.IsNullOrEmpty(config.ApiKey) || string.IsNullOrEmpty(config.ApiSecret))
                    throw new InvalidOperationException("Cloudinary configuration is missing or invalid.");

                var account = new Account(config.CloudName, config.ApiKey, config.ApiSecret);
                return new Cloudinary(account);
            });

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<SpeechToTextService>();
            // Add repositories
            builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();
            builder.Services.AddScoped<ICategoryRoomRepository, CategoryRoomRepository>();
            builder.Services.AddScoped<ICategoryServiceRepository, CategoryServiceRepository>();
            builder.Services.AddScoped<IContractRepository, ContractRepository>();
            builder.Services.AddScoped<ILandlordLicenseRepository, LandlordLicenseRepository>();
            builder.Services.AddScoped<IRentalListRepository, RentalListRepository>();
            builder.Services.AddScoped<IRentalServiceListRepository, RentalServiceListRepository>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<IRoomLicenseRepository, RoomLicenseRepository>();
            builder.Services.AddScoped<IServiceFeedbackRepository, ServiceFeedbackRepository>();
            builder.Services.AddScoped<IServiceLicenseRepository, ServiceLicenseRepository>();
            builder.Services.AddScoped<IServicePostRepository, ServicePostRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<IWithdrawRequestRepository, WithdrawRequestRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserFeedbackRepository, UserFeedbackRepository>();
            builder.Services.AddScoped<ICategoryPriorityPackageRoomRepository, CategoryPriorityPackageRoomRepository>();
            builder.Services.AddScoped<ICategoryPriorityPackageServicePostRepository, CategoryPriorityPackageServicePostRepository>();
            builder.Services.AddScoped<IPriorityPackageRoomRepository, PriorityPackageRoomRepository>();
            builder.Services.AddScoped<IPriorityPackageServicePostRepository, PriorityPackageServicePostRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<UserDAO>();
            builder.Services.AddScoped<UserFeedbackDAO>();
            builder.Services.AddHttpClient<FPTAIService>();
            builder.Services.AddScoped<CloudinaryService>();
            builder.Services.AddScoped<IInsiderTradingRepository, InsiderTradingRepository>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<FPTAIService>();
            // Add SignalR
            builder.Services.AddSignalR();
            builder.Services.AddHostedService<CheckExpiredContractsService>();
            builder.Services.AddScoped<IAuthorizationContractRepository, AuthorizationContractRepository>();
            builder.Services.AddScoped<PdfService>();
            builder.Services.Configure<AzureImageServiceOptions>(
            builder.Configuration.GetSection("AzureImageService"));
            builder.Services.AddSingleton<AzureImageService>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<AzureImageServiceOptions>>().Value;
                return new AzureImageService(options.Endpoint, options.ApiKey);
            });
            // Add Hangfire
            builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(builder.Configuration.GetConnectionString("DBString"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Thêm Hangfire server
            builder.Services.AddHangfireServer();

            // Add CORS policy for React app
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins("https://blue-field-0c1caa000.6.azurestaticapps.net")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials(); // Quan trọng cho SignalR
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "swagger";
            });
            app.UseHttpsRedirection();
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                        logger.LogError(error.Error, "Unhandled exception occurred");
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Internal Server Error");
                    }
                });
            });
            // Sử dụng CORS trước các middleware khác
            app.UseCors("AllowReactApp");
            app.UseRouting();
            app.UseAuthentication(); 
            app.UseAuthorization();

            app.UseHangfireDashboard(); 

            // Map SignalR Hubs
            app.MapHub<SavedPostHub>("/savedPostHub");
            app.MapHub<ChatHub>("/chathub");

            app.MapControllers();

            app.Run();
        }
    }
}
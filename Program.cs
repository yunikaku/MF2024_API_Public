
using MF2024_API.Models;
using MF2024_API.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using Microsoft.Graph.Core;

using Microsoft.Kiota.Abstractions.Authentication;
using MF2024_API.Service;
using MF2024API.Service;
using System.Text.Json.Serialization;
using System.Security.Claims;
using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Security.Cryptography.X509Certificates;

namespace MF2024_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // SQL Serverを使用する場合
            builder.Services.AddDbContext<Mf2024apiDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // MySQLを使用する場合
            //builder.Services.AddDbContext<Mf2024apiDbContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(option =>
            {
                //循環参照を無視する
                option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                option.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<Mf2024apiDbContext>().AddDefaultTokenProviders();

            //HTTP3（QUIC）実装
            //builder.WebHost.ConfigureKestrel(
            //    (context, option) =>
            //    option.Listen(IPAddress.Any, 5001, listenOptions =>
            //    {
            //        listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
            //        var ssl = LoadCertificate();
            //        listenOptions.UseHttps(ssl);
            //    }));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen
                    (
                        option =>
                        {
                            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                            {
                                In = ParameterLocation.Header,
                                Description = "Please enter a valid token",
                                Name = "Authorization",
                                Type = SecuritySchemeType.Http,
                                BearerFormat = "JWT",
                                Scheme = "Bearer"
                            });
                            option.AddSecurityRequirement(new OpenApiSecurityRequirement
                            {
                                    {
                                        new OpenApiSecurityScheme
                                        {
                                            Reference = new OpenApiReference
                                            {
                                                Type=ReferenceType.SecurityScheme,
                                                Id="Bearer"
                                            }
                                        },
                                        new string[]{}
                                    }
                            });
                        }
                    );


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowJsApp", policy =>
                {
                    policy.WithOrigins("https://192.168.2.100:5173") // 許可するフロントエンドのオリジンを指定
                    .AllowAnyHeader()                    // 任意のヘッダーを許可
                    .AllowAnyMethod();                   // 任意のHTTPメソッドを許可
                });
            });



            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                options.DefaultChallengeScheme =
                options.DefaultForbidScheme =
                options.DefaultScheme =
                options.DefaultSignInScheme =
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = ClaimTypes.Role,
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
                    )
                };
            });

            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<AESInterfaces, AESService>();
            builder.Services.AddScoped<MailInterfase, MailService>();
            builder.Services.AddScoped<DiscordInterfase, DiscordService>();


            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowJsApp");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();


            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    //自動でマイグレーションを実行
                    var context = services.GetRequiredService<Mf2024apiDbContext>();
                    context.Database.Migrate();

                    //シードデータを追加

                    SeetData.SeetDataLoad(services).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }

                app.Run();
            }

        }
        static X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2("cert.pfx", "password"); // 証明書のパスとパスワード
        }
    }

}

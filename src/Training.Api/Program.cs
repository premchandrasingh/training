
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace Training.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Initializes a new instance of the WebApplicationBuilder class with preconfigured defaults.
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var assembly = Assembly.GetEntryAssembly();
                var xmlFile = $"{assembly.GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (Path.Exists(xmlPath))
                    options.IncludeXmlComments(xmlPath);
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySQL(builder.Configuration.GetConnectionString("my_conn"));
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:7184";
                    options.Audience = Constants.JwtAudiance;
                    var sharedKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.JwtSecretKey));

                    var tokenValidationParameters = new TokenValidationParameters
                    {
                        // Ensure the token hasn't expired:
                        RequireExpirationTime = true,
                        RequireSignedTokens = true,
                        // Ensure the token audience matches our audience value (default true):
                        ValidateAudience = true,
                        ValidAudience = Constants.JwtAudiance,
                        // Ensure the token was issued by a trusted authorization server (default true):
                        ValidateIssuer = true,
                        ValidIssuer = Constants.JwtIssuer,
                        ValidateIssuerSigningKey = true,
                        // Specify the key used to sign the token:
                        IssuerSigningKey = sharedKey,
                        //IssuerSigningKeys = issuerSigningKeys,
                        ValidateLifetime = true,
                        // Clock skew compensates for server time drift.
                        // We recommend 5 minutes or less:
                        ClockSkew = TimeSpan.FromMinutes(2),
                    };

                    options.TokenValidationParameters = tokenValidationParameters;
                    options.TokenValidationParameters.NameClaimType = ClaimTypes.NameIdentifier;
                    options.TokenValidationParameters.RoleClaimType = ClaimTypes.Role; // ClaimTypes.Role is default therefore not mandatory here
                });


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
        }
    }
}

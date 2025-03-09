using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Training.Api.Entity;
using Training.Api.Model;

namespace Training.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly AppDbContext _appDbContext;

        public AccountsController(ILogger<AccountsController> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        [HttpPost("token")]
        public JsonResult GenerateToken([FromBody] LoginBindingModel model)
        {
            var emp = _appDbContext.Employees.Where(x => x.FirstName == model.UserName && x.Password == model.Password).FirstOrDefault();
            if (emp == null)
            {
                return new JsonResult(new { Message = "Invalid username or password" })
                {
                    StatusCode = 401
                };
            }

            var token = GenerateJwtToken(emp);
            var refreshToken = GenerateJwtToken(emp, isRefreshToken: true);
            return new JsonResult(new { Token = token, Refresh_Token = refreshToken })
            {
                StatusCode = 200
            };

        }


        private string GenerateJwtToken(Employee employee, bool isRefreshToken = false)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.JwtSecretKey));
            // HmacSha256 meaning HMAC + SHA265. HMAC means Hash-based Message Authentication Code, 
            // HS256 stands for HMAC with SHA256 - used in symetric encryption
            // RS256 stands for RSA with SHA256 - used in asymetric encryption
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = CreateJwtClaims(employee, isRefreshToken);

            DateTime? expiry = null;
            if (isRefreshToken)
            {
                // Refresh token claims
                expiry = DateTime.UtcNow.AddMinutes(60 + 20);
            }
            else
            {
                expiry = DateTime.UtcNow.AddMinutes(60);
            }


            var token = new JwtSecurityToken(Constants.JwtIssuer,
              Constants.JwtAudiance,
              claims,
              expires: expiry, // 8 hrs is complete office hrs,
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private List<Claim> CreateJwtClaims(Employee app, bool isRefreshToken = false)
        {
            var claims = new List<Claim>();

            if (isRefreshToken)
            {
                // Refresh token claims
                new Claim(JwtRegisteredClaimNames.Sub, app.EmployeeId.ToString());
            }
            else
            {
                claims.AddRange(new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, app.EmployeeId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, app.EmployeeId.ToString()),
                    new Claim(JwtRegisteredClaimNames.GivenName, $"{app.FirstName} {app.LastName}"),
                    new Claim("roles", app.Role),
                });
            }

            return claims;
        }


    }
}

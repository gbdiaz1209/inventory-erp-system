using Inventory.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Inventory.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous] // Este controlador DEBE ser público para que puedan iniciar sesión
    public class AuthController : ControllerBase
    {
       
        private readonly Inventory.WebAPI.Configurations.JwtSettings _jwtSettings;

        // Inyectamos JwtSettings usando IOptions (Patrón de Opciones que configuramos en el Program.cs)
        public AuthController(IOptions<Inventory.WebAPI.Configurations.JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            // 1. SIMULACIÓN DE VALIDACIÓN (Más adelante lo conectarás a tu base de datos)
            // Por ahora, usaremos un usuario de prueba para validar que todo funcione
            if (loginDto.Username == "admin" && loginDto.Password == "123456")
            {
                // 2. CREAR LOS CLAIMS (Datos del usuario que viajarán dentro del Token)
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Name, loginDto.Username),
                    new Claim(ClaimTypes.Role, "Administrador") // Esto servirá para restringir pantallas en Angular
                };

                // 3. GENERAR LA LLAVE CRIPTOGRÁFICA
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // 4. CREAR EL CUERPO DEL JWT
                var token = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                    signingCredentials: creds
                );

                // 5. ESCRIBIR EL TOKEN COMO STRING
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                // Devuelve 200 OK junto con el pasaporte digital
                return Ok(new { Token = tokenString, Expiration = token.ValidTo });
            }

            // Si las credenciales están mal, devuelve 401 sin dar detalles (por seguridad)
            return Unauthorized(new { Message = "Credenciales incorrectas" });
        }
    }
}
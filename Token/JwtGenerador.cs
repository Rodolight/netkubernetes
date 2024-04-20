using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NetKubernetes.Models;

namespace NetKubernetes.Token;

public class JwtGenerador : IJwtGenerador
{
       readonly IConfiguration configuration;
        
        public JwtGenerador(IConfiguration configuration)
         {
          //  _mySettings = options.Value;
          this.configuration = configuration;
        }
        
    public string CrearToken(Usuario usuario)
    {

        var claims = new List<Claim>{
            new(JwtRegisteredClaimNames.NameId, usuario.UserName!),
            new("userId", usuario.Id), 
            new("Email", usuario.Email!)
        };

         //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi_Palabra_Secreta_Debe_ser_mas larga para que se pueda generar bien el token."));
         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
         var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

         var tokenDescripcion = new SecurityTokenDescriptor{
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(30),
            SigningCredentials = credenciales
         };

         var tokenHandler = new JwtSecurityTokenHandler();
         var token = tokenHandler.CreateToken(tokenDescripcion);
         return tokenHandler.WriteToken(token);
    }

   

}

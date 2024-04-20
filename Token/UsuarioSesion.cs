using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace NetKubernetes.Token;

public class UsuarioSesion : IUsuarioSesion
{

     private readonly IHttpContextAccessor _httpContextAccessor;

     public UsuarioSesion(IHttpContextAccessor httpContextAccessor)
     {
        _httpContextAccessor = httpContextAccessor;
     }
    public string ObtenerUsuarioSession()
    {
        var userName = _httpContextAccessor.HttpContext!.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        return userName!;
    }
}

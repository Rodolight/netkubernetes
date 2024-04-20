using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetKubernetes.Dtos.UsuarioDtos;
using NetKubernetes.Middleware;
using NetKubernetes.Models;
using NetKubernetes.Models.Data;
using NetKubernetes.Token;

namespace NetKubernetes.Data.Usuarios;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly IJwtGenerador _jwtGenerador;
    private readonly AppDbContext _context;
    private readonly IUsuarioSesion _usuarioSesion;
 
 public UsuarioRepository(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador,
                           AppDbContext context, IUsuarioSesion usuarioSesion)
 {
    _userManager = userManager;
    _signInManager = signInManager;
    _jwtGenerador = jwtGenerador;
    _context = context;
    _usuarioSesion = usuarioSesion;
 }

  private UsuarioResponseDto TransformerUserToUserDto(Usuario usuario){
    return new UsuarioResponseDto {
        Id = usuario.Id,
        Nombre = usuario.Nombre,
        Apellidos = usuario.Apellidos,
        Telefono = usuario.Telefono,
        Email = usuario.Email,
        UserName = usuario.UserName,
        Token = _jwtGenerador.CrearToken(usuario)
    };
  }
    public async Task<UsuarioResponseDto> GetUsuario()
    {
        var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSession()) ?? throw new MiddlewareException(HttpStatusCode.Unauthorized,
                                          new {mensaje = "El usuario del token no existe en la base de datos"} ); 
        return TransformerUserToUserDto(usuario!);
    }

    public async Task<UsuarioResponseDto> Login(UsuarioLoginRequestDtos request)
    {
        var usuario = await _userManager.FindByEmailAsync(request.Email!) ?? throw new MiddlewareException(HttpStatusCode.Unauthorized,
                                          new {mensaje = "El email del usuario no existe en la base de datos"} );

       var resultado = await _signInManager.CheckPasswordSignInAsync(usuario!, request.Password!,false);

       if(resultado.Succeeded)  return TransformerUserToUserDto(usuario!);

       throw new MiddlewareException(HttpStatusCode.Unauthorized, new {mesaje="Las credenciales son incorrectas."});

    }

    public async Task<UsuarioResponseDto> RegistrarUsuario(UsuarioRegistroRequestDtos request)
    {
        var existeEmail = await _context.Users.Where(x => x.Email == request.Email ).AnyAsync();
        if (existeEmail) {
           throw new MiddlewareException(HttpStatusCode.BadRequest, new {mensaje="El email del usuario ya existe en la base de datos."});
        }

         var existeUserName = await _context.Users.Where(x => x.UserName == request.UserName ).AnyAsync();
        if (existeUserName) {
           throw new MiddlewareException(HttpStatusCode.BadRequest, new {mensaje="El UserName del usuario ya existe en la base de datos."});
        }


        var usuario = new Usuario{
            Nombre = request.Nombre,
            Apellidos = request.Apellidos,
            Telefono = request.Telefono,
            Email = request.Email,
            UserName = request.UserName
        };

        var resultados = await _userManager.CreateAsync(usuario!, request.Password!);
      
       if(resultados.Succeeded)
         return TransformerUserToUserDto(usuario);

        throw new Exception("No se pudo registrar el usuario."); 
       
    }

  
}
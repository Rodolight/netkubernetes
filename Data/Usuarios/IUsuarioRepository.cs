using NetKubernetes.Dtos.UsuarioDtos;

namespace NetKubernetes.Data.Usuarios;

public interface IUsuarioRepository
{   
    Task<UsuarioResponseDto> GetUsuario();

    Task<UsuarioResponseDto> Login(UsuarioLoginRequestDtos request);
    
    Task<UsuarioResponseDto> RegistrarUsuario(UsuarioRegistroRequestDtos request);
}
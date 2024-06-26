using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetKubernetes.Data.Usuarios;
using NetKubernetes.Dtos.UsuarioDtos;

namespace NetKubernetes.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsuarioController: ControllerBase
{
    private readonly IUsuarioRepository _repository;

    public UsuarioController(IUsuarioRepository repository)
    {
        _repository = repository;
    }
     
     [AllowAnonymous]
     [HttpPost("login")]
     public async Task<ActionResult<UsuarioResponseDto>> Login ([FromBody] UsuarioLoginRequestDtos request) {
        return await _repository.Login(request);
    }

     [AllowAnonymous]
     [HttpPost("registrar")]
     public async Task<ActionResult<UsuarioResponseDto>> Registrar ([FromBody] UsuarioRegistroRequestDtos request) {
        return await _repository.RegistrarUsuario(request);
    }

      
     [HttpGet]
     public async Task<ActionResult<UsuarioResponseDto>> DevolverUsuario () {
        return await _repository.GetUsuario();
    }
}
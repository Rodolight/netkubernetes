using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetKubernetes.Middleware;
using NetKubernetes.Models;
using NetKubernetes.Models.Data;
using NetKubernetes.Token;

namespace NetKubernetes.Data.Inmuebles;

public class InmuebleRepository : IInmuebleRepository
{
    private readonly AppDbContext _context;
    private readonly IUsuarioSesion _usuarioSesion;
    private readonly UserManager<Usuario> _userManager;
    public InmuebleRepository(AppDbContext context, IUsuarioSesion usuarioSesion, UserManager<Usuario> userManager)
    {

        _context = context;
        _usuarioSesion = usuarioSesion;
        _userManager = userManager;
    }
    public async Task CreateInmueble(Inmueble inmueble)
    {
        var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSession()) ??
         throw new MiddlewareException(HttpStatusCode.BadRequest, new {mensaje="El usuario no es válido para insertar Inmuebles."});
        
        if(inmueble is null){
             throw new MiddlewareException(HttpStatusCode.BadRequest, new {mensaje="Los datos del inmueble son icorrectos."});
        }
        
        inmueble.FechaCreacion = DateTime.UtcNow;
        inmueble.UsuarioId = Guid.Parse(usuario!.Id);

        await _context.Inmuebles!.AddAsync(inmueble);
       
    }

    public async Task DeleteInmueble(int Id)
    {
        var inmueble = await _context.Inmuebles!.FirstOrDefaultAsync(x => x.Id == Id);
         _context.Inmuebles!.Remove(inmueble!);
    }

    public async Task<IEnumerable<Inmueble>> GetAllInmuebles()
    {
        return await _context.Inmuebles!.ToListAsync();
    }

    public async Task<Inmueble> GetInmuebleById(int Id)
    {
        var resultado = await _context.Inmuebles!.FirstOrDefaultAsync(X => X.Id == Id);
         return resultado!;
    }

    

    public async Task<bool> SaveChanges()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }
}

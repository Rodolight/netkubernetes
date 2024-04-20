using Microsoft.AspNetCore.Identity;
using NetKubernetes.Models;
using NetKubernetes.Models.Data;

namespace NetKubernetes.Data;

public class LoadDatabase {
    public static async Task InsertarData(AppDbContext context, UserManager<Usuario> userManager){
        if(!userManager.Users.Any()){
            var usuario = new Usuario{
                Nombre="Rodolfo",
                Apellidos = "De Pena",
                Email="rde_pena@gmail.com",
                UserName= "rodolight",
                Telefono="555-555-4444"
            };

            await userManager.CreateAsync(usuario, "Password123$");  
        }

        if(!context.Inmuebles!.Any()){
           context.Inmuebles!.AddRange(
            new Inmueble{
                Nombre = "Casa de Playa",
                Direccion = "Av. El Sol 32",
                Precio = 300969,
                FechaCreacion = DateTime.UtcNow
            },
             new Inmueble{
                Nombre = "Casa de Invierno",
                Direccion = "Av. La Roca 101",
                Precio = 3500M,
                FechaCreacion = DateTime.UtcNow
            }
           );

           context.SaveChanges();
        }
    }
}
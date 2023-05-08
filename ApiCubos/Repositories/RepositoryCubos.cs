using ApiCubos.Context;
using ApiCubos.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCubos.Repositories
{
    public class RepositoryCubos
    {
        private DataContext context;

        public RepositoryCubos(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<Cubo>> GetCubosAsync()
        {
            return await this.context.Cubos.ToListAsync();
        }

        public async Task<List<Cubo>> GetCubosMarcaAsync(string marca)
        {
            return await this.context.Cubos.Where(x => x.Marca == marca).ToListAsync();
        }

        private int GetMaxIdCubo()
        {
            if (this.context.Cubos.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Cubos.Max(x => x.IdCubo) + 1;
            }
        }

        public async Task NuevoCuboAsync(string nombre, string marca, string imagen, int precio)
        {
            Cubo cubo = new Cubo();
            cubo.IdCubo = GetMaxIdCubo();
            cubo.Nombre = nombre;
            cubo.Marca = marca;
            cubo.Imagen = imagen;
            cubo.Precio = precio;
            this.context.Cubos.Add(cubo);
            await this.context.SaveChangesAsync();
        }

        public async Task<Usuario> GetUsuarioAsync(string username)
        {
            return await this.context.Usuarios.FirstOrDefaultAsync(x => x.Email == username);
        }

        public async Task<List<Compra>> GetComprasUsuarioAsync(int idusuario)
        {
            return await this.context.Compras.Where(x => x.IdUsuario == idusuario).ToListAsync();
        }

        private int GetMaxIdCompra()
        {
            if (this.context.Compras.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Compras.Max(x => x.IdPedido) + 1;
            }
        }

        public async Task NuevaCompraAsync(int idcubo, int idusuario)
        {
            Compra compra = new Compra();
            compra.IdPedido = GetMaxIdCompra();
            compra.IdCubo = idcubo;
            compra.IdUsuario = idusuario;
            compra.Fecha = DateTime.Now;
            this.context.Compras.Add(compra);
            await this.context.SaveChangesAsync();
        }

        private int GetMaxIdUsuario()
        {
            if (this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Usuarios.Max(x => x.IdUsuario) + 1;
            }
        }

        public async Task NuevoUsuarioAsync(string nombre, string email, string pass, string imagen)
        {
            Usuario usuario = new Usuario();
            usuario.IdUsuario = GetMaxIdUsuario();
            usuario.Nombre = nombre;
            usuario.Email = email;
            usuario.Pass = pass;
            usuario.Imagen = imagen;
            this.context.Usuarios.Add(usuario);
            await this.context.SaveChangesAsync();
        }

        public async Task<Usuario> LoginAsync(string email, string pass)
        {
            return await this.context.Usuarios.FirstOrDefaultAsync(x => x.Email == email && x.Pass == pass);
        }
    }
}

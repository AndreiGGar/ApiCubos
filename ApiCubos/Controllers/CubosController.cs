using ApiCubos.Models;
using ApiCubos.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ApiCubos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CubosController : ControllerBase
    {
        private RepositoryCubos repo;

        public CubosController(RepositoryCubos repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cubo>>> GetCubos()
        {
            return await this.repo.GetCubosAsync();
        }

        [HttpGet("{marca}")]
        public async Task<ActionResult<List<Cubo>>> GetCubosMarca(string marca)
        {
            return await this.repo.GetCubosMarcaAsync(marca);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> NuevoUsuario(Usuario usuario)
        {
            await this.repo.NuevoUsuarioAsync(usuario.Nombre, usuario.Email, usuario.Pass, usuario.Imagen);
            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> NuevoCubo(Cubo cubo)
        {
            await this.repo.NuevoCuboAsync(cubo.Nombre, cubo.Marca, cubo.Imagen);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Usuario>> PerfilUsuario()
        {
            Claim claim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserData");
            string jsonEmpleado = claim.Value;
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(jsonEmpleado);
            return usuario;
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Compra>>> ComprasUsuario()
        {
            string jsonEmpleado = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserData").Value;
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(jsonEmpleado);
            List<Compra> compras = await this.repo.GetComprasUsuarioAsync(usuario.IdUsuario);
            return compras;
        }

        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> NuevaCompra(Compra compra)
        {
            await this.repo.NuevaCompraAsync(compra.IdCubo, compra.IdUsuario);
            return Ok();
        }
    }
}

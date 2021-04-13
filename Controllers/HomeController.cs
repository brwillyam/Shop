using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Modelos;

namespace Shop.Controllers
{
    [Route("v1")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<dynamic>> Get([FromServices]DataContext context)
        {
            var employee = new Usuario {Id = 1, NomeUsuario = "Robin", Senha = "robin", Role = "employee"};
            var manager = new Usuario {Id = 1, NomeUsuario = "Batman", Senha = "batman", Role = "manager"};
            var categoria = new Categorias {Id =1, Titulo = "Informatica"};
            var produto = new Produto {Id =1, Categorias = categoria, Titulo = "Mouse", Preco = 199, Descricao = "Mouse gamer"};
            context.Usuario.Add(employee);
            context.Usuario.Add(manager);
            context.Categorias.Add(categoria);
            context.Produto.Add(produto);
            await context.SaveChangesAsync();

            return Ok(new {
                message = "Dados configurados"
            });
        }
    }
}
using System.ComponentModel.Design.Serialization;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Modelos;
using System.Threading.Tasks;
using System.Collections.Generic;
using Shop.Data;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;
using Shop.Services;

namespace Shop.Controllers
{
    [Route("produtos")]
    public class ProdutoController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Produto>>> Get([FromServices] DataContext context)
        {
            var produtos = await context.Produto.Include(x => x.Categorias).AsNoTracking().ToListAsync();
            return Ok(produtos);
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Produto>>> GetById(int id, [FromServices] DataContext context)
        {
           var produtos = await context.Produto.Include(x => x.Categorias).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return Ok(produtos); 
        }
        [HttpGet]
        [Route("categorias/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Produto>>> GetByCategorias(int id, [FromServices] DataContext context)
        {
           var produtos = await context.Produto.Include(x => x.Categorias).AsNoTracking().Where(x => x.CategoriasId == id).ToListAsync();
            return Ok(produtos); 
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Produto>>> Post([FromBody] Produto modelos,
                                                               [FromServices] DataContext context)
         {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            try
            {
               context.Produto.Add(modelos);
                await context.SaveChangesAsync();
                return Ok(modelos); 
            }
            catch (System.Exception)
            {
                return BadRequest(new{message = "Não foi possivel criar o produto."});
                
            }
            
        }  
    }
}
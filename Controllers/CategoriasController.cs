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
using Microsoft.AspNetCore.ResponseCompression;
namespace Shop.Controllers
{
    //Endpoint => URL 
    //https://localhost:5001/categorias
    [Route("v1/categorias")]
    public class CategoriasController : ControllerBase
    {
        // https://localhost:5001/categorias
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]        
        public async Task<ActionResult<List<Categorias>>> Get([FromServices] DataContext context)
        {
            var categorias = await context.Categorias.AsNoTracking().ToListAsync();
                return Ok(categorias);
        }  

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Categorias>> GetById(int id, 
                                                             [FromServices] DataContext context
                                                             )
        {
            var categoria = await context.Categorias.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                return Ok(categoria);
        }
        [HttpPost]
        [Route("")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Categorias>>> Post([FromBody] Categorias modelos,
                                                               [FromServices] DataContext context)
         {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            try
            {
               context.Categorias.Add(modelos);
                await context.SaveChangesAsync();
                return Ok(modelos); 
            }
            catch (System.Exception)
            {
                return BadRequest(new{message = "Não foi possivel criar a categoria."});
                
            }
            
        }  
        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public  async Task<ActionResult<List<Categorias>>>  Put(int id,[FromBody]Categorias modelos,
                                                                  [FromServices] DataContext context)
        {
            //Verifica se o ID informado é o mesmo do modelo
            if(id != modelos.Id)
               return NotFound(new { message = "Categoria nao encontrada."});

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }   
            try
            {
                context.Entry<Categorias>(modelos).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(modelos);

            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este registro ja foi atualizado."});
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel atualizar a categoria"});

            }

        }  
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public  async Task<ActionResult<List<Categorias>>>  Delete(int id,
                                                                    [FromServices]DataContext context
                                                                    )
        {
            var categoria = await context.Categorias.FirstOrDefaultAsync(x => x.Id == id);
            if (categoria == null)
            {
                return NotFound(new { message = "Categoria nao encontrada"});
            }
            try
            {
                context.Categorias.Remove(categoria);
                await context.SaveChangesAsync();
                return Ok(new { message = "Categoria removida."});
            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Não foi possivel remover a categoria"});
            }
            
        } 
    }
}
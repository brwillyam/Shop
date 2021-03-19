using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Modelos;
using System.Threading.Tasks;
using System.Collections.Generic;
using Shop.Data;

namespace Shop.Controllers
{
    //Endpoint => URL 
    //https://localhost:5001/categorias
    [Route("categorias")]
    public class CategoriasController : ControllerBase
    {
        // https://localhost:5001/categorias
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Categorias>>> Get(){
            return new List<Categorias>();
        }  
         [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Categorias>> GetById(int id){
            return new Categorias();
        } 
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<List<Categorias>>> Post([FromBody] Categorias modelos,
                                                               [FromServices] DataContext context)
         {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            context.Categorias.Add(modelos);
            await context.SaveChangesAsync();
            return Ok(modelos);
        }  
        [HttpPut]
        [Route("{id:int}")]
        public  async Task<ActionResult<List<Categorias>>>  Put(int id,[FromBody]Categorias modelos)
        {
            //Verifica se o ID informado é o mesmo do modelo
            if(id != modelos.Id)
               return NotFound(new { message = "Categoria nao encontrada."});

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }   

            return Ok(modelos);

        }  
         [HttpDelete]
        [Route("id:int")]
        public  async Task<ActionResult<List<Categorias>>>  Delete(){
            return Ok();
        } 
    }
}
using System.Reflection;
using System.Xml.Linq;
using System.Diagnostics.Contracts;
using System.IO;
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
using Shop.Services;
using Microsoft.AspNetCore.Authorization;

namespace Shop.Controllers
{
    [Route("usuario")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<Usuario>>> Get([FromServices] DataContext context)
        {
            var usuarios = await context.Usuario.AsNoTracking().ToListAsync();
            return usuarios;
        }
        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Usuario>>> Post([FromBody] Usuario modelos,
                                                               [FromServices] DataContext context)
         {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            try
            {
                modelos.Role = "employee";

                context.Usuario.Add(modelos);
                await context.SaveChangesAsync();

                modelos.Senha = "";
                return Ok(modelos); 
            }
            catch (System.Exception)
            {
                return BadRequest(new{message = "Não foi possivel criar a categoria."});
                
            }
            
        }
        [HttpPost] 
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] Usuario modelos,
                                                               [FromServices] DataContext context)
        {
            var usuario = await context.Usuario.AsNoTracking()
            .Where(x => x.NomeUsuario == modelos.NomeUsuario && x.Senha == modelos.Senha)
            .FirstOrDefaultAsync();

            if (usuario == null)
            return NotFound(new{message = "Ususario ou senha invalidos"});

            var token = TokenService.GenerateToken(usuario);

            usuario.Senha = "";
            return new
            {
                usuario = usuario,
                token = token
            };
        }
        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public  async Task<ActionResult<Usuario>>  Put(int id,[FromBody]Usuario modelos,
                                                                  [FromServices] DataContext context)
        {
            //Verifica se o ID informado é o mesmo do modelo
            if(id != modelos.Id)
               return NotFound(new { message = "Usuario nao encontrado."});

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }   
            try
            {
                context.Entry(modelos).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(modelos);

            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este registro ja foi atualizado."});
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel atualizar o usuario"});

            }

        }
         [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles ="manager")]
        public  async Task<ActionResult<List<Usuario>>>  Delete(int id,
                                                                    [FromServices]DataContext context
                                                                    )
        {
            var usuario = await context.Usuario.FirstOrDefaultAsync(x => x.Id == id);
            if (usuario == null)
            {
                return NotFound(new { message = "usuario nao encontrado"});
            }
            try
            {
                context.Usuario.Remove(usuario);
                await context.SaveChangesAsync();
                return Ok(new { message = "usuario removido."});
            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Não foi possivel remover o usuario"});
            }
            
        } 
        
    }
}
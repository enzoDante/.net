using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIRedeSo.Models;
using APIRedeSo.Services;

namespace APIRedeSo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuario.ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }
            usuario.Senha = PasswordService.HashPassword(usuario.Senha);
            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            // Verifica se o email já está em uso
            var usuarioExistente = await _context.Usuario
                .FirstOrDefaultAsync(u => u.Email == usuario.Email);

            if (usuarioExistente != null)
            {
                return Conflict("Email já está em uso.");
            }

            usuario.Senha = PasswordService.HashPassword(usuario.Senha);
            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // POST: api/Usuarios/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Usuario loginData)
        {
            if (string.IsNullOrWhiteSpace(loginData.Email) && string.IsNullOrWhiteSpace(loginData.Nome))
            {
                return BadRequest("Nome ou Email é obrigatório.");
            }

            var user = await _context.Usuario.FirstOrDefaultAsync(u => (u.Email == loginData.Email || u.Nome == loginData.Nome));

            if (user == null || !PasswordService.VerifyPassword(loginData.Senha, user.Senha))
            {
                return Unauthorized("Invalid email or password.");
            }

            // Simples retorno de login; pode ser adaptado para JWT
            return Ok(new { message = "Login successful" });
        }
        // GET: api/Usuarios/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<object>>> SearchUsersByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("O parâmetro de pesquisa é obrigatório.");
            }

            // Realiza a busca no banco de dados por nomes que contenham o termo pesquisado
            var users = await _context.Usuario
                .Where(u => u.Nome.Contains(name))
                .Select(u => new { u.Id, u.Nome })
                .ToListAsync();

            return Ok(users);
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.Id == id);
        }
    }
}

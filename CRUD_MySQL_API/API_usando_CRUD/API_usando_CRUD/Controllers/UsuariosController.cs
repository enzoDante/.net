using CRUD_MySQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_usando_CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private CRUD_class _crud;

        public UsuariosController()
        {
            // Substitua sua string de conexão
            string connectionString = "server=localhost;database=nome_do_banco;user=root;password=sua_senha";
            _crud = new CRUD_class("localhost", "geral", "root", "admin");
        }
        // GET: api/usuarios
        [HttpGet]
        public IActionResult GetUsuarios()
        {
            string query = "SELECT * FROM usuarios";
            List<Dictionary<string, object>> result = _crud.ExecuteSelect(query);
            return Ok(result);
        }

        // GET: api/usuarios/{id}
        [HttpGet("{id}")]
        public IActionResult GetUsuario(int id)
        {
            string query = $"SELECT * FROM usuarios WHERE id = {id}";
            var result = _crud.ExecuteSelect(query);
            if (result.Count == 0)
            {
                return NotFound();
            }
            return Ok(result[0]);
        }

        // POST: api/usuarios
        [HttpPost]
        public IActionResult CreateUsuario([FromBody] UsuarioDto usuario)
        {
            List<string> columns = new List<string> { "nome", "email" };
            List<object> values = new List<object> { usuario.Nome, usuario.Email };
            _crud.Create("usuarios", columns, values);
            return Ok("Usuário criado com sucesso.");
        }

        // PUT: api/usuarios/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateUsuario(int id, [FromBody] UsuarioDto usuario)
        {
            List<string> columns = new List<string> { "nome", "email" };
            List<object> values = new List<object> { usuario.Nome, usuario.Email };
            _crud.Update("usuarios", columns, values, "id", id);
            return Ok("Usuário atualizado com sucesso.");
        }

        // DELETE: api/usuarios/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUsuario(int id)
        {
            string query = $"DELETE FROM usuarios WHERE id = {id}";
            _crud.Delete(query);
            return Ok("Usuário deletado com sucesso.");
        }

    }
    // DTO (Data Transfer Object) para receber o usuário
    public class UsuarioDto
    {
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}

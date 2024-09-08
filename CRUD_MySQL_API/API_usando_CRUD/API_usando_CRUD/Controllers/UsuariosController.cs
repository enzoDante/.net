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
            //string connectionString = "server=localhost;database=nome_do_banco;user=root;password=sua_senha";
            _crud = new CRUD_class("localhost", "geral", "root", "");
        }
        // GET: api/usuarios
        [HttpGet]
        public IActionResult GetUsuarios([FromQuery] UsuarioDto usuario)
        {
            string query = $"SELECT * FROM {usuario.Tabela}";
            List<Dictionary<string, object>> result = _crud.Select(query);
            return Ok(result);
        }

        // GET: api/usuarios/{id}
        [HttpGet("{id}")]
        public IActionResult GetUsuario(int id, [FromQuery] UsuarioDto usuario)
        {
            string query = $"SELECT * FROM {usuario.Tabela} WHERE {usuario.Colum} = {id}";
            var result = _crud.Select(query);
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
            //List<string> columns = new List<string> { "nome", "email" };
            //List<object> values = new List<object> { usuario.Nome, usuario.Email };
            _crud.Create(usuario.Tabela, usuario.Columns, usuario.Values);
            return Ok("Usuário criado com sucesso.");
        }

        // PUT: api/usuarios/{id}
        [HttpPut]
        public IActionResult UpdateUsuario([FromBody] UsuarioDto usuario)
        {
            // Verifica se o objeto recebido é válido
            if (usuario == null || string.IsNullOrEmpty(usuario.Tabela) || string.IsNullOrEmpty(usuario.Colum))
            {
                return BadRequest("Dados inválidos.");
            }
            //List<string> columns = new List<string> { "nome", "email" };
            //List<object> values = new List<object> { usuario.Nome, usuario.Email };
            _crud.Update(usuario.Tabela, usuario.Columns, usuario.Values, usuario.Colum, usuario.Id);
            return Ok("Usuário atualizado com sucesso.");
        }

        // DELETE: api/usuarios/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUsuario(int id, [FromQuery] UsuarioDto usuario)
        {
            //string query = $"DELETE FROM usuarios WHERE id = {id}";
            _crud.Delete(usuario.Tabela, usuario.Colum, id); //usuario.value id
            return Ok("Usuário deletado com sucesso.");
        }

    }
    // DTO (Data Transfer Object) para receber o usuário
    //criar um DTO para cada rota caso seja necessário (rotas que precisam de outros valores para n ficar confuso)
    public class UsuarioDto
    {
        public object? Id { get; set; } //? declara o item como anulável nullable
        public string? Tabela { get; set; }
        public List<string>? Columns { get; set; }
        public List<object>? Values { get; set; }
        public string? Colum { get; set; }
        public object? Value { get; set; }
    }
}

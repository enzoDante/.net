using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using RedeSocialAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RedeSocialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly string _connectionString;
        public UsuarioController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // GET: api/<UsuarioController>
        [HttpGet]
        public ActionResult<IEnumerable<Usuario>> GetUsuarios()
        {
            var usuarios = new List<Usuario>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM usuario", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuarios.Add(new Usuario { 
                            Id = reader.GetInt32("id") ,
                            Name = reader.GetString("nome") ,
                            Email = reader.GetString("email") ,
                            Senha = reader.GetString("senha") ,
                            CreatedAt = reader.GetDateTime("data_criacao"),
                            IsAdmin = reader.GetInt32("is_admin") ,
                        });
                    }
                }
            }
            return Ok(usuarios);
        }

        // GET api/<UsuarioController>/5
        [HttpGet("{id}")]
        public ActionResult<Usuario> GetUsuario(int id)
        {
            Usuario usuario = null;
            using(var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM usuario WHERE id = @id", connection);
                command.Parameters.AddWithValue("@id", id);
                using(var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("nome"),
                            Email = reader.GetString("email"),
                            Senha = reader.GetString("senha"),
                            CreatedAt = reader.GetDateTime("data_criacao") ,
                            IsAdmin = reader.GetInt32("is_admin")
                        };
                    }
                }
            }
            if(usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        // POST api/<UsuarioController>
        [HttpPost]
        public ActionResult<Usuario> CreateUsuario([FromBody] Usuario usuario)
        {
            using(var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("INSERT INTO usuario (nome, email, senha, is_admin) VALUES (@nome, @email, @senha, @is_admin)", connection);
                command.Parameters.AddWithValue("@nome", usuario.Name);
                command.Parameters.AddWithValue("@email", usuario.Email);
                command.Parameters.AddWithValue("@senha", usuario.Senha);
                command.Parameters.AddWithValue("@is_admin", usuario.IsAdmin);
                command.ExecuteNonQuery();
            }
            return CreatedAtAction(nameof(GetUsuario), new {id = usuario.Id}, usuario);
        }

        // PUT api/<UsuarioController>/5
        [HttpPut("{id}")]
        public IActionResult UpdateUsuariot(int id, [FromBody] Usuario usuario)
        {
            using(var connections = new MySqlConnection(_connectionString))
            {
                connections.Open();
                var command = new MySqlCommand("UPDATE usuario SET nome=@nome, email=@email, senha=@senha, is_admin=@is_admin where id=@id", connections);
                command.Parameters.AddWithValue("@nome", usuario.Name);
                command.Parameters.AddWithValue("@email", usuario.Email);
                command.Parameters.AddWithValue("@senha", usuario.Senha);
                command.Parameters.AddWithValue("@is_admin", usuario.IsAdmin);
                command.Parameters.AddWithValue("@id", id);
                var rowsAffected = command.ExecuteNonQuery();
                if(rowsAffected == 0)
                {
                    return NotFound();
                }
            }
            return NoContent();
        }

        // DELETE api/<UsuarioController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUsuario(int id)
        {
            using(var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("DELETE FROM usuario WHERE id=@id", connection);
                command.Parameters.AddWithValue("@id", id);
                var rowAffected = command.ExecuteNonQuery();
                if(rowAffected == 0)
                {
                    return NotFound();
                }
            }
            return NoContent();
        }
    }
}

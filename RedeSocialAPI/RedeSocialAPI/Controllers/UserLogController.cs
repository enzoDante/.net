using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using RedeSocialAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RedeSocialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLogController : ControllerBase
    {
        private readonly string _connectionString;
        public UserLogController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // POST: api/usuarios/register
        [HttpPost("register")]
        public ActionResult<Usuario> Register([FromBody] Usuario usuario)
        {
            // Hash da senha antes de armazenar
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("INSERT INTO usuario (nome, email, senha, is_admin) VALUES (@nome, @email, @senha, @is_admin)", connection);
                command.Parameters.AddWithValue("@nome", usuario.Name);
                command.Parameters.AddWithValue("@email", usuario.Email);
                command.Parameters.AddWithValue("@senha", usuario.Senha);
                command.Parameters.AddWithValue("@is_admin", usuario.IsAdmin);
                command.ExecuteNonQuery();
            }

            return CreatedAtAction(nameof(UsuarioController.GetUsuario), "Usuario", new { id = usuario.Id }, usuario);
        }
        // POST: api/usuarios/login
        [HttpPost("login")]
        public ActionResult<Usuario> Login([FromBody] LoginModel login)
        {
            Usuario usuario = null;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM usuario WHERE email = @email", connection);
                command.Parameters.AddWithValue("@email", login.Email);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("nome"),
                            Email = reader.GetString("email"),
                            Senha = reader.GetString("senha") // Não retorne a senha!
                        };
                    }
                }
            }

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(login.Password, usuario.Senha))
            {
                return Unauthorized(); // Senha incorreta ou usuário não encontrado
            }

            return Ok(usuario); // Retorne informações do usuário ou token de autenticação
        }

    }
}

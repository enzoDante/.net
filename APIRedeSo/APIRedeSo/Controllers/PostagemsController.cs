using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIRedeSo.Models;

namespace APIRedeSo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostagemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostagemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Postagems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Postagem>>> GetPostagens()
        {
            return await _context.Postagens.ToListAsync();
        }

        // GET: api/Postagems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Postagem>> GetPostagem(int id)
        {
            var postagem = await _context.Postagens
                .Include(p => p.Fotos)
                .Include(p => p.Colaboradores)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (postagem == null)
            {
                return NotFound();
            }

            return postagem;
        }

        // PUT: api/Postagems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePostagem(int id, Postagem postagemAtualizada)
        {
            var postagem = await _context.Postagens.FindAsync(id);
            if (postagem == null)
            {
                return NotFound();
            }

            postagem.Titulo = postagemAtualizada.Titulo;
            postagem.Descricao = postagemAtualizada.Descricao;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Postagems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Postagem>> CreatePostagem(PostagemCreateDTO postagemDTO)
        {
            postagemDTO.Data_criacao = DateTime.Now;
            var postagem = new Postagem
            {
                Usuario_id = postagemDTO.Usuario_id,
                Titulo = postagemDTO.Titulo,
                Descricao = postagemDTO.Descricao,
                Data_criacao = postagemDTO.Data_criacao,
                Curtidas = postagemDTO.Curtidas
            };
            //postagem.Data_criacao = DateTime.Now;
            //postagem.Curtidas = 0;
            _context.Postagens.Add(postagem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPostagem), new { id = postagem.Id }, postagem);
        }

        // DELETE: api/Postagems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostagem(int id)
        {
            var postagem = await _context.Postagens.FindAsync(id);
            if (postagem == null)
            {
                return NotFound();
            }

            _context.Postagens.Remove(postagem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostagemExists(int id)
        {
            return _context.Postagens.Any(e => e.Id == id);
        }



        [HttpPost("{postagemId}/fotos")]
        public async Task<IActionResult> AddFoto(int postagemId, List<Fotos_postagem> foto)
        {
            var postagem = await _context.Postagens.FindAsync(postagemId);
            if (postagem == null)
            {
                return NotFound();
            }
            //associa o ID da postagem a cada foto e adiciona ao banco
            foreach (var item in foto)
            {
                item.Postagem_id = postagemId;
                _context.Fotos.Add(item);
                
            }
            await _context.SaveChangesAsync();

            return Ok(foto);
        }
        [HttpPost("{postagemId}/colaboradores")]
        public async Task<IActionResult> AddColaborador(int postagemId, List<Postagem_usuario_relacionado> colaborador)
        {
            var postagem = await _context.Postagens.FindAsync(postagemId);
            if (postagem == null)
            {
                return Unauthorized("Apenas o dono da postagem pode adicionar colaboradores.");
            }
            foreach (var item in colaborador)
            {
                item.Postagem_id = postagemId;
                _context.Colaboradores.Add(item);
                
            }
            await _context.SaveChangesAsync();

            return Ok(colaborador);
        }
    }
}

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
        public async Task<ActionResult<IEnumerable<PostagemComFotosDTO>>> GetPostagens()
        {
            var postagens = await _context.Postagem
                .Include(p => p.Fotos)
                .Include(p => p.Colaboradores)
                .Select(p => new PostagemComFotosDTO
                {
                    Id = p.Id,
                    Usuario_id = p.Usuario_id,
                    Titulo = p.Titulo,
                    Descricao = p.Descricao,
                    Data_criacao = p.Data_criacao,
                    Curtidas = p.Curtidas,
                    Fotos = p.Fotos.Select(f => new FotosDTO
                    {
                        Url_foto = f.Url_foto
                    }).ToList(),
                    Colaboradores = p.Colaboradores.Select(c => new ColaboradoresDTO
                    {
                        Usuario_id = c.Usuario_id
                    }).ToList()
                })
                .ToListAsync();

            return Ok(postagens);
        }

        // GET: api/Postagems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostagemComFotosDTO>> GetPostagem(int id)
        {
            var postagem = await _context.Postagem
                .Include(p => p.Fotos)
                .Include(p => p.Colaboradores)
                .Select(p => new PostagemComFotosDTO
                {
                    Id = p.Id,
                    Usuario_id = p.Usuario_id,
                    Titulo = p.Titulo,
                    Descricao = p.Descricao,
                    Data_criacao = p.Data_criacao,
                    Curtidas = p.Curtidas,
                    Fotos = p.Fotos.Select(f => new FotosDTO
                    {
                        Url_foto = f.Url_foto
                    }).ToList(),
                    Colaboradores = p.Colaboradores.Select(c => new ColaboradoresDTO
                    {
                        Usuario_id = c.Usuario_id
                    }).ToList()

                }).FirstOrDefaultAsync(p => p.Id == id);

            if (postagem == null)
            {
                return NotFound();
            }

            return postagem;
        }

        // PUT: api/Postagems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePostagem(int id, PostagemUpdateDTO updateDto)
        {
            var postagem = await _context.Postagem.FindAsync(id);
            //baseado no id da postagem, buscar também todos os elementos da tabela de fotos daquela postagem (postagem_id)
            if (postagem == null)
            {
                return NotFound();
            }
            //postagem.Id = id
            postagem.Titulo = updateDto.Titulo;
            postagem.Descricao = updateDto.Descricao;
            // Atualiza as fotos
            // Você pode querer fazer uma lógica mais complexa para gerenciar a atualização das fotos
            // Aqui, por exemplo, vamos limpar as fotos existentes e adicionar as novas
            var fotosExistentes = await _context.Fotos_postagem.Where(f => f.Postagem_id == id).ToListAsync();
            _context.Fotos_postagem.RemoveRange(fotosExistentes); // Remove as fotos antigas

            if (updateDto.Fotos != null && updateDto.Fotos.Any())
            {
                foreach (var fotoDto in updateDto.Fotos)
                {
                    if(fotoDto.Url_foto != "")
                    {
                        var novaFoto = new Fotos_postagem
                        {
                            Postagem_id = id,
                            Url_foto = fotoDto.Url_foto
                        };
                        _context.Fotos_postagem.Add(novaFoto);
                    }
                }
            }
            //atualizar colaboradores
            var colabExistentes = await _context.Postagem_usuario_relacionado.Where(f => f.Postagem_id == id).ToListAsync();
            _context.Postagem_usuario_relacionado.RemoveRange(colabExistentes);//remove os colaboradores
            if(updateDto.Colaboradores != null && updateDto.Colaboradores.Any())
            {
                foreach (var colabDto in updateDto.Colaboradores)
                {
                    if(colabDto.Usuario_id != 0)
                    {
                        var novosC = new Postagem_usuario_relacionado
                        {
                            Postagem_id = id,
                            Usuario_id = colabDto.Usuario_id
                        };
                        _context.Postagem_usuario_relacionado.Add(novosC);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return NoContent(); // Retorna 204 No Content após a atualização
        }

        // POST: api/Postagems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Postagem>> CreatePostagem(PostagemComFotosDTO postagemDto)
        {
            postagemDto.Data_criacao = DateTime.Now;
            var postagem = new Postagem
            {
                Usuario_id = postagemDto.Usuario_id,
                Titulo = postagemDto.Titulo,
                Descricao = postagemDto.Descricao,
                Data_criacao = postagemDto.Data_criacao,
                Curtidas = postagemDto.Curtidas
            };

            // Adiciona a postagem ao contexto e salva
            _context.Postagem.Add(postagem);
            await _context.SaveChangesAsync();

            // Captura o ID da nova postagem
            int novoPostagemId = postagem.Id;

            // Se houver fotos, adiciona-as com o ID da nova postagem
            if (postagemDto.Fotos != null && postagemDto.Fotos.Count > 0)
            {
                foreach (var fotoDto in postagemDto.Fotos)
                {
                    if(fotoDto.Url_foto != "")
                    {
                        var foto = new Fotos_postagem
                        {
                            Postagem_id = novoPostagemId,
                            Url_foto = fotoDto.Url_foto
                        };
                        _context.Fotos_postagem.Add(foto);
                    }
                }
                await _context.SaveChangesAsync();
                
            }
            if(postagemDto.Colaboradores != null && postagemDto.Colaboradores.Count > 0)
            {
                foreach (var colaborador in postagemDto.Colaboradores)
                {
                    if(colaborador.Usuario_id != 0)
                    {
                        // Verifica se o usuário existe
                        var usuarioExiste = await _context.Usuario.AnyAsync(u => u.Id == colaborador.Usuario_id);
                        if (!usuarioExiste)
                        {
                            return BadRequest($"O usuário com ID {colaborador.Usuario_id} não existe.");
                        }

                        var colab = new Postagem_usuario_relacionado
                        {
                            Postagem_id=novoPostagemId,
                            Usuario_id = colaborador.Usuario_id
                        };
                        _context.Postagem_usuario_relacionado.Add(colab);
                    }
                }
                await _context.SaveChangesAsync();
            }

            // Retorna a postagem com o ID gerado
            return CreatedAtAction(nameof(GetPostagem), new { id = novoPostagemId }, postagem);
        }

        // DELETE: api/Postagems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostagem(int id)
        {
            var postagem = await _context.Postagem.FindAsync(id);
            if (postagem == null)
            {
                return NotFound();
            }

            _context.Postagem.Remove(postagem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostagemExists(int id)
        {
            return _context.Postagem.Any(e => e.Id == id);
        }
    }
}

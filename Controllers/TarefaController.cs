using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;
using System;
using System.Linq;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // Busca a tarefa pelo ID no banco de dados
            var tarefa = _context.Tarefas.Find(id);

            // Verifica se a tarefa existe
            if (tarefa == null)
                return NotFound(); // Retorna 404 se a tarefa não foi encontrada

            return Ok(tarefa); // Retorna a tarefa com status 200
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            // Busca todas as tarefas no banco
            var tarefas = _context.Tarefas.ToList();
            return Ok(tarefas); // Retorna a lista de tarefas
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // Busca tarefas com o título correspondente
            var tarefas = _context.Tarefas
                .Where(t => t.Titulo.Contains(titulo))
                .ToList();
            return Ok(tarefas);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas
                .Where(x => x.Data.Date == data.Date)
                .ToList(); // Adicionado ToList() para listar as tarefas da data especificada
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefa = _context.Tarefas
                .Where(x => x.Status == status)
                .ToList(); // Adicionado ToList() para listar as tarefas com o status especificado
            return Ok(tarefa);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Adiciona a tarefa ao contexto e salva as alterações
            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();

            // Retorna a tarefa recém-criada com o status 201
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Atualiza os campos da tarefaBanco com os dados da tarefa recebida
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            // Atualiza a tarefa no contexto e salva as mudanças
            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();

            return Ok(tarefaBanco); // Retorna a tarefa atualizada
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            // Remove a tarefa do contexto e salva as mudanças
            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();

            return NoContent(); // Retorna 204 (sem conteúdo) após a exclusão
        }
    }
}

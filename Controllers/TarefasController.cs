using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskAPI.Context;
using TaskAPI.Models;

namespace TaskAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TarefasController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<Tarefa>> Get()
        {
            var tarefas =_context.Tarefas.ToList();
            
            if(tarefas is null)
            {
                return NotFound("Sem tarefas registradas...");
            }
            return tarefas;
        }


        [HttpGet("{id:int}")]
        public ActionResult<Tarefa> Get(int id)
        {
            var tarefa = _context.Tarefas.FirstOrDefault(t=> t.TarefaId == id);

            if (tarefa is null)
            {
                return NotFound("Tarefa não encontrada...");
            }
            return tarefa;
        }



    }
}

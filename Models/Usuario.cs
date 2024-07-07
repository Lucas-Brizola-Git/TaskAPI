using System.Collections.ObjectModel;

namespace TaskAPI.Models
{
    public class Usuario
    {
        public Usuario()
        {
            Tarefas = new Collection<Tarefa>();
        }

        public int UsuarioId { get; set; }
        public string? Name { get; set; }
        public string? Senha { get; set; }
        public string? Email { get; set; }

        public ICollection<Tarefa> Tarefas { get; set; }
    }
}

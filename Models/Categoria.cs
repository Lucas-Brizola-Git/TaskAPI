using System.Collections.ObjectModel;

namespace TaskAPI.Models;

public class Categoria
{
    public Categoria() 
    {
        Tarefas = new Collection<Tarefa>();
    }
    public int CategoriaId { get; set; } //Colocando Id no atributo, por convenção o EF entende como chave primaria
    public string? Nome { get; set; }

    public ICollection<Tarefa> Tarefas { get; set; }
}

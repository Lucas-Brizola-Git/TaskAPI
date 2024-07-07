namespace TaskAPI.Models
{
    public class Tarefa
    {
        public int TarefaId { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public string? DataCriacao { get; set; }
        public string? DataConclusao { get; set; }
        public int Status { get; set; }
        
        public int  CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }
}

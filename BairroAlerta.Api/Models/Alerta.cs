namespace BairroAlerta.Api.Models
{
    public class Alerta
    {
        public int Id { get; set; }
        public string? Tipo { get; set; }
        public string? Descricao { get; set; }
        public string? Usuario { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.Now;
    }
}
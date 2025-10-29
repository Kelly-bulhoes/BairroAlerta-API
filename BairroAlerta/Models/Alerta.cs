
namespace BairroAlerta.Models
{
    public class Alerta
    {
        
    
        public int Id { get; set; }
                public string Tipo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;
        
        
        public string Usuario { get; set; } = string.Empty;
        
       
        public DateTime CriadoEm { get; set; } = DateTime.Now;
    }
}

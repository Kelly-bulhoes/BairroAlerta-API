using System;
using System.ComponentModel.DataAnnotations;

namespace BairroAlerta.Api.Models
{
    public class Alerta
    {
        [Key]
        public int Id { get; set; }

        // MUDANÇA: Adicionado 'required' para eliminar o warning CS8618
        [Required(ErrorMessage = "O campo Tipo é obrigatório.")]
        public required string Tipo { get; set; }

        // MUDANÇA: Adicionado 'required'
        [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
        public required string Descricao { get; set; }

        // MUDANÇA: Adicionado 'required'
        [Required(ErrorMessage = "O campo Usuário é obrigatório.")]
        public required string Usuario { get; set; }

        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}
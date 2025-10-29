using BairroAlerta.Api.Data;      
using BairroAlerta.Api.Models;
using Microsoft.EntityFrameworkCore; 
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; // ESSENCIAL para usar o .Where

namespace BairroAlerta.Api.Services
{
    public class AlertasService : IAlertasService
    {
        private readonly AlertaContext _context;

        public AlertasService(AlertaContext context)
        {
            _context = context;
        }

        public async Task<List<Alerta>> GetAllAsync()
        {
            return await _context.Alertas.ToListAsync();
        }

        // MÉTODO FALTANTE: ESSA IMPLEMENTAÇÃO DEVE ESTAR AQUI!
        public async Task<List<Alerta>> GetByTipoAsync(string tipo)
        {
            return await _context.Alertas
                .Where(a => a.Tipo.ToLower() == tipo.ToLower())
                .ToListAsync();
        }

        public async Task<Alerta> AddAsync(Alerta alerta)
        {
            _context.Alertas.Add(alerta);
            await _context.SaveChangesAsync();
            return alerta;
        }
    }
}
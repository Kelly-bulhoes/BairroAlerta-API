using BairroAlerta.Api.Models; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BairroAlerta.Api.Services
{
    public interface IAlertasService
    {
        // 1. CONTRATO OBRIGATÓRIO (GetAllAsync)
        Task<List<Alerta>> GetAllAsync();

        // 2. CONTRATO OBRIGATÓRIO (GetByTipoAsync)
        Task<List<Alerta>> GetByTipoAsync(string tipo); 

        // 3. CONTRATO OBRIGATÓRIO (AddAsync)
        Task<Alerta> AddAsync(Alerta alerta);
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BairroAlerta.Api.Data;
using BairroAlerta.Api.Models;
using BairroAlerta.Api.Hubs;
using BairroAlerta.Api.Services; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BairroAlerta.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertasController : ControllerBase
    {
        private readonly IAlertasService _alertasService; 
        private readonly IHubContext<AlertaHub> _hubContext;

        public AlertasController(IAlertasService alertasService, IHubContext<AlertaHub> hubContext)
        {
            _alertasService = alertasService;
            _hubContext = hubContext;
        }
        
        // MÉTODO GET 1: Busca todos os alertas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alerta>>> GetAlertas()
        {
            var alertas = await _alertasService.GetAllAsync(); 
            return Ok(alertas);
        }

        // MÉTODO GET 2: Busca alertas filtrados por tipo
        [HttpGet("{tipo}")] // Rota final será: /api/Alertas/Incêndio
        public async Task<ActionResult<IEnumerable<Alerta>>> GetAlertasByTipo(string tipo)
        {
            if (string.IsNullOrEmpty(tipo))
            {
                // Se a URL fosse /api/Alertas/ (sem tipo), a rota GetAlertas já cobriria
                return BadRequest("O parâmetro de tipo é obrigatório para esta rota.");
            }

            var alertas = await _alertasService.GetByTipoAsync(tipo);
            
            if (alertas == null || alertas.Count == 0)
            {
                // Melhor prática: retorna 404 Not Found se o recurso filtrado não existir
                return NotFound($"Nenhum alerta encontrado para o tipo: {tipo}"); 
            }
            
            return Ok(alertas);
        }

        // MÉTODO POST: Cria um novo alerta
        [HttpPost]
        public async Task<IActionResult> CreateAlerta([FromBody] Alerta alerta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var novoAlerta = await _alertasService.AddAsync(alerta);

            await _hubContext.Clients.All
                .SendAsync("ReceberAlerta", novoAlerta);

            return CreatedAtAction(
                nameof(GetAlertas), 
                new { id = novoAlerta.Id }, novoAlerta);
        }
    }
}
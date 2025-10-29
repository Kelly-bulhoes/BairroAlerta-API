using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BairroAlerta.Api.Data;
using BairroAlerta.Api.Models;
using BairroAlerta.Api.Hubs;

namespace BairroAlerta.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertasController : ControllerBase
    {
        private readonly AlertaContext _context;
        private readonly IHubContext<AlertaHub> _hubContext;

        public AlertasController(
            AlertaContext context,
            IHubContext<AlertaHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet]
        public IActionResult GetAlertas()
        {
            var alertas = _context.Alertas.ToList();
            return Ok(alertas);

        }

        [HttpPost]
        public async Task<IActionResult> CreateAlerta([FromBody] Alerta alerta)
        {
            _context.Alertas.Add(alerta);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All
                .SendAsync("ReceberAlerta", alerta);

            return CreatedAtAction(
                nameof(GetAlertas), 
                new { id = alerta.Id }, alerta);
        }
    }
}


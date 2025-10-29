using Microsoft.AspNetCore.SignalR;
using BairroAlerta.Api.Models;

namespace BairroAlerta.Api.Hubs
{
    public class AlertaHub : Hub
    {
        public async Task NovoAlerta(Alerta alerta)
        {
            await Clients.All.SendAsync("ReceberAlerta", alerta);
        }
    }
}


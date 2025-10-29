using Microsoft.AspNetCore.SignalR.Client;
using BairroAlerta.Models;
using System.Net.Http.Json;

namespace BairroAlerta.Services
{
    public class AlertaService
    {
        private HubConnection? _connection;
        private readonly HttpClient _httpClient;
        private const string ServerUrl = "http://10.0.2.2:5065";

        public AlertaService()
        {
            _httpClient = new HttpClient();
        }

        public async Task ConectarAsync()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl($"{ServerUrl}/alertaHub")
                .WithAutomaticReconnect()
                .Build();

            _connection.On<Alerta>("ReceberAlerta", alerta => 
            {
                OnNovoAlerta?.Invoke(alerta);
            });

            await _connection.StartAsync();
        }

        public async Task EnviarAlertaAsync(Alerta alerta)
        {
            await _httpClient.PostAsJsonAsync(
                $"{ServerUrl}/api/alertas", alerta);
        }

        public async Task<List<Alerta>> ObterTodosAletasAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"{ServerUrl}/api/alertas");
                var json = await response.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<List<Alerta>>(json) ?? new List<Alerta>();
            }
            catch
            {
                return new List<Alerta>();
            }
        }

        public event Action<Alerta>? OnNovoAlerta;
    }
}
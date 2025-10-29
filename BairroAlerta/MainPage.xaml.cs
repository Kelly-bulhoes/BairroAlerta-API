using BairroAlerta.Services;
using BairroAlerta.Models; 

namespace BairroAlerta;

public partial class MainPage : ContentPage
{
    private AlertaService _alertaService;
    
    private List<Alerta> _alertas = new();
    public MainPage()
    {
        InitializeComponent();
        _alertaService = new AlertaService();
        CarregarAlertas();
    }

    private async void CarregarAlertas()
    {
        try
        {
            await _alertaService.ConectarAsync();
            
            _alertaService.OnNovoAlerta += (alerta) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    _alertas.Insert(0, alerta); 
                    AtualizarLista();
                });
            };

            await CarregarAletasDoServidor();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Falha ao conectar: {ex.Message}", "OK");
        }
    }

    private async Task CarregarAletasDoServidor()
    {
        try
        {
            var alertas = await _alertaService.ObterTodosAletasAsync();
            _alertas.AddRange(alertas);
            AtualizarLista();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Falha ao carregar: {ex.Message}", "OK");
        }
    }

    private void AtualizarLista()
    {
        var textos = _alertas.Select(a => 
            $"[{a.Tipo}] {a.CriadoEm:dd/MM HH:mm}\n{a.Descricao}\nPor: {a.Usuario}"
        ).ToList();
        
        AlertasCollectionView.ItemsSource = new List<string>(textos);
    }

    private async void OnNovoAlertaClicked(object sender, EventArgs e)
    {
        var tipo = await this.DisplayPrompt("Novo Alerta", "Tipo (Segurança, Animal, Clima...):");        if (string.IsNullOrEmpty(tipo)) return;

        var descricao = await this.DisplayPrompt("Novo Alerta", "Descrição:");
        if (string.IsNullOrEmpty(descricao)) return;

        var usuario = await this.DisplayPrompt("Novo Alerta", "Seu nome:");
        if (string.IsNullOrEmpty(usuario)) return;

        var novoAlerta = new Alerta
        {
            Tipo = tipo,
            Descricao = descricao,
            Usuario = usuario
        };

        try
        {
            await _alertaService.EnviarAlertaAsync(novoAlerta);
            await DisplayAlert("Sucesso", "Alerta criado!", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Falha ao enviar: {ex.Message}", "OK");
        }
    }

    private async Task<string?> DisplayPrompt(string v1, string v2)
    {
        throw new NotImplementedException();
    }
}
using System.Text;
using System.Text.Json;
using PlaylistsApi.Config;

namespace PlaylistsMaui;

public partial class CadastroPage : ContentPage
{
    public CadastroPage()
    {
        InitializeComponent();
    }

    private async void OnCreateAccountButtonClicked(object sender, EventArgs e)
    {
        string nome = NomeEntry.Text;
        string email = EmailEntry.Text;
        string senha = PasswordEntry.Text;

        try
        {
            HttpClient client = new();
            var apiUrl = ApiConfig.Instance.GetCriadorUrl();
            var jsonData = JsonSerializer.Serialize(new { nome = nome, email = email, senha = senha });
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Sucesso", "Cadastro realizado com sucesso!", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Erro", "Não foi possível realizar o cadastro.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao realizar o cadastro: {ex.Message}", "OK");
        }
    }
}

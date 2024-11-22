using System.Text;
using System.Text.Json;
using PlaylistsApi.Config;
using PlaylistsApi.Models.DTO;
using PlaylistsMaui;

namespace PlaylistsApi
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            var loginData = new
            {
                Email = username,
                Senha = password
            };

            string jsonData = JsonSerializer.Serialize(loginData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpClient client = new();
            try
            {
                var apiUrl = ApiConfig.Instance.GetLoginUrl();

                var response = await client.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(responseContent);

                    if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                    {
                        Preferences.Set("JwtToken", loginResponse.Token);
                        Preferences.Set("NomeCriador", loginResponse.Nome);

                        await DisplayAlert("Sucesso", loginResponse.Message, "OK");
                        await Navigation.PushAsync(new ConteudosPage());
                    }
                }
                else
                {
                    await DisplayAlert("Erro", "Credenciais inválidas.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao tentar fazer login: {ex.Message}", "OK");
            }
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CadastroPage());
        }

        private async void OnButtonPressed(object sender, EventArgs e)
        {
            await LoginButton.ScaleTo(0.95, 50, Easing.CubicIn);
        }

        private async void OnButtonReleased(object sender, EventArgs e)
        {
            await LoginButton.ScaleTo(1.0, 50, Easing.CubicOut);
        }
    }
}

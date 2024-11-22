using System.Net.Http.Headers;
using System.Text.Json;
using PlaylistsApi.Models;
using PlaylistsApi.Config;
using System.Text;

namespace PlaylistsMaui
{
    public partial class ConteudosPage : ContentPage
    {
        public ConteudosPage()
        {
            InitializeComponent();
            LoadConteudos();
            SetCriadorNome();
        }

        private void SetCriadorNome()
        {
            if (Preferences.ContainsKey("NomeCriador"))
            {
                CriadorLabel.Text = Preferences.Get("NomeCriador", "Criador");
            }
        }

        private async void LoadConteudos()
        {
            try
            {
                string token = Preferences.Get("JwtToken", string.Empty);

                if (string.IsNullOrEmpty(token))
                {
                    await DisplayAlert("Erro", "Token não encontrado. Faça login novamente.", "OK");
                    return;
                }

                HttpClient client = new();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var apiUrl = ApiConfig.Instance.GetConteudosUrl();

                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var conteudos = JsonSerializer.Deserialize<List<Conteudo>>(responseContent, JsonOptions.Default);

                    ConteudosCollectionView.ItemsSource = conteudos;
                }
                else
                {
                    await DisplayAlert("Erro", "Não foi possível carregar os conteúdos.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao carregar os conteúdos: {ex.Message}", "OK");
            }
        }

        private async void OnEditButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button?.BindingContext is not Conteudo conteudo)
                return;

            string novoTitulo = await DisplayPromptAsync("Editar Conteúdo", "Novo título:", initialValue: conteudo.Titulo);
            string novoTipo = await DisplayPromptAsync("Editar Conteúdo", "Novo tipo:", initialValue: conteudo.Tipo);

            if (string.IsNullOrEmpty(novoTitulo) || string.IsNullOrEmpty(novoTipo))
            {
                await DisplayAlert("Erro", "Título e Tipo não podem estar vazios.", "OK");
                return;
            }

            var conteudoUpdate = new
            {
                Titulo = novoTitulo,
                Tipo = novoTipo
            };

            try
            {
                string token = Preferences.Get("JwtToken", string.Empty);
                HttpClient client = new();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                var apiUrl = ApiConfig.Instance.GetConteudosUrl(conteudo.Id);
                var jsonData = JsonSerializer.Serialize(conteudoUpdate);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    LoadConteudos();
                }
                else
                {
                    await DisplayAlert("Erro", "Erro ao atualizar o conteúdo.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao atualizar o conteúdo: {ex.Message}", "OK");
            }
        }

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button?.BindingContext is not Conteudo conteudo)
                return;

            bool confirm = await DisplayAlert("Excluir Conteúdo", $"Tem certeza que deseja excluir '{conteudo.Titulo}'?", "Sim", "Não");

            if (!confirm)
                return;

            try
            {
                string token = Preferences.Get("JwtToken", string.Empty);
                HttpClient client = new();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var apiUrl = ApiConfig.Instance.GetConteudosUrl(conteudo.Id);
                var response = await client.DeleteAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    LoadConteudos();
                }
                else
                {
                    await DisplayAlert("Erro", "Erro ao excluir o conteúdo.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao excluir o conteúdo: {ex.Message}", "OK");
            }
        }

        private async void OnAddButtonClicked(object sender, EventArgs e)
        {
            string titulo = await DisplayPromptAsync("Novo Conteúdo", "Digite o título do conteúdo:");
            if (string.IsNullOrWhiteSpace(titulo)) return;

            string tipo = await DisplayPromptAsync("Novo Conteúdo", "Digite o tipo do conteúdo:");
            if (string.IsNullOrWhiteSpace(tipo)) return;

            var novoConteudo = new { Titulo = titulo, Tipo = tipo };
            string token = Preferences.Get("JwtToken", string.Empty);

            if (string.IsNullOrEmpty(token))
            {
                await DisplayAlert("Erro", "Token não encontrado. Faça login novamente.", "OK");
                return;
            }

            HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var apiUrl = ApiConfig.Instance.GetConteudosUrl();

            var jsonData = JsonSerializer.Serialize(novoConteudo);
            var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                LoadConteudos();
            }
            else
            {
                await DisplayAlert("Erro", "Não foi possível criar o conteúdo.", "OK");
            }
        }
    }
}

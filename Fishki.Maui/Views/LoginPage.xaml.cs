using Fishki.Maui.Utils;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Windows.Input;
using Fishki.Maui.Models;

namespace Fishki.Maui.Views;

public partial class LoginPage : ContentPage
{
    public string Email { get; set; }
    public string Password { get; set; }
	public ICommand LoginButtonCommand => new Command(LoginHandler);
	public ICommand RegisterButtonCommand => new Command(() => Shell.Current.GoToAsync(nameof(RegisterPage)));

	public static FishkiApiService apiService;

    public LoginPage()
	{
		apiService = new FishkiApiService();
		
		InitializeComponent();

		BindingContext = this;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

		var token = await SecureStorage.GetAsync("auth_token");
		if (string.IsNullOrEmpty(token))
			return;

		try
		{
			var verificationResponse = await apiService.VerifyToken(token);
			if (verificationResponse != null && verificationResponse.IsSuccessStatusCode)
			{
				await Shell.Current.GoToAsync(nameof(FishkiSetsPage));
			}

            SecureStorage.Remove("auth_token");
        }
		catch (Exception ex)
		{
			SecureStorage.Remove("auth_token");
			await Shell.Current.GoToAsync($"{nameof(ErrorPage)}?msg={ex.Message}");
		}
    }

    private async void LoginHandler()
	{
		try
		{
			var json = new JsonObject();
			json.Add("email", Email);
			json.Add("password", Password);

			var requestJson = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
			var apiResponse = await apiService.LoginUser(requestJson);

			if (apiResponse != null && apiResponse.IsSuccessStatusCode)
			{
				var stringResponse = await apiResponse.Content.ReadAsStringAsync();
				var jsonResponse = JsonSerializer.Deserialize<LoginResponse>(stringResponse);

				await SecureStorage.SetAsync("auth_token", jsonResponse.Token);
				apiService.SetBearerToken(jsonResponse.Token);

				await Shell.Current.GoToAsync(nameof(FishkiSetsPage));
			}

			else throw new Exception("Nie uda³o siê zalogowaæ");
		}
		catch (Exception ex)
		{
			SecureStorage.Remove("auth_token");
            await Shell.Current.GoToAsync($"{nameof(ErrorPage)}?msg={ex.Message}");
        }
	}
}
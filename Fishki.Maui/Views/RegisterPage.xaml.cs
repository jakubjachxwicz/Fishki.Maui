using Fishki.Maui.Utils;
using System.Text;
using System.Text.Json.Nodes;
using System.Windows.Input;

namespace Fishki.Maui.Views;

public partial class RegisterPage : ContentPage
{
    private readonly FishkiApiService apiService = LoginPage.apiService;
	
	public string Username { get; set; }
    public string Email { get; set; }
	public string Password { get; set; }
    public string RepeatedPassword { get; set; }
	public ICommand RegisterButtonCommand => new Command(RegisterHandler);
	public ICommand LoginButtonCommand => new Command(() => Shell.Current.GoToAsync(".."));

    public RegisterPage()
	{
		InitializeComponent();

		BindingContext = this;
	}

	private async void RegisterHandler()
	{
		var json = new JsonObject();
		json.Add("username", Username);
		json.Add("email", Email);
		json.Add("password", Password);

		var requestJson = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
		var apiResponse = await apiService.RegisterUser(requestJson);

		if (apiResponse != null && apiResponse.IsSuccessStatusCode)
		{
			await DisplayAlert("Komunikat", "U¿tkownik zosta³ dodany", "OK");
			await Shell.Current.GoToAsync("..");
		}
	}
}
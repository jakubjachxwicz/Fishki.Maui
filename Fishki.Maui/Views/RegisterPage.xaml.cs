using Fishki.Maui.Utils;
using System.Text;
using System.Text.Json.Nodes;
using System.Windows.Input;

namespace Fishki.Maui.Views;

public partial class RegisterPage : ContentPage
{
    private readonly FishkiApiService apiService = LoginPage.apiService;

    public bool IsUsernameValid { get; set; }
    public bool IsEmailValid { get; set; }

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
		if (!IsUsernameValid)
        {
            await DisplayAlert("Komunikat", "Nazwa u�ytkownika powinna zawiera� od 1 do 20 znak�w", "OK");
            return;
        }

        if (!IsEmailValid)
        {
            await DisplayAlert("Komunikat", "Niepoprawny adres email", "OK");
            return;
        }

        if (!StaticMethods.IsPasswordValid(Password))
        {
            await DisplayAlert("Komunikat", "Has�o musi zawiera�:\n8 - 20 znak�w\nWielk� liter� oraz cyfr�", "OK");
            return;
        }

        if (Password != RepeatedPassword)
        {
            await DisplayAlert("Komunikat", "Has�a s� r�ne", "OK");
            return;
        }

        try
		{
            var json = new JsonObject();
            json.Add("username", Username);
            json.Add("email", Email);
            json.Add("password", Password);

            var requestJson = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            var apiResponse = await apiService.RegisterUser(requestJson);

            if (apiResponse != null && apiResponse.IsSuccessStatusCode)
            {
                await DisplayAlert("Komunikat", "U�tkownik zosta� dodany", "OK");
                await Shell.Current.GoToAsync("..");
            }

            else throw new Exception("Rejestracja nie powiod�a si�");
        }
        catch (Exception ex)
        {
            await Shell.Current.GoToAsync($"{nameof(ErrorPage)}?msg={ex.Message}");
        }
	}
}
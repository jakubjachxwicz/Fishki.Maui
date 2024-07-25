using Fishki.Maui.Models;
using Fishki.Maui.Utils;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Input;

namespace Fishki.Maui.Views;

public partial class SettingsPage : ContentPage, INotifyPropertyChanged
{
    private readonly FishkiApiService _apiService = LoginPage.apiService;
    public event PropertyChangedEventHandler PropertyChanged;

    private string _username;
    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged(nameof(Username));
        }
    }
    private string _email;
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged(nameof(Email));
        }
    }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string RepeatedNewPassword { get; set; }

    public bool IsUsernameValid { get; set; }
    public bool IsEmailValid { get; set; }

    public ICommand ReturnButtonCommand => new Command(() => Shell.Current.GoToAsync(".."));
	public ICommand LogoutButtonCommand => new Command(LogoutHandler);
	public ICommand UpdateUsernameCommand => new Command(UpdateUsernameHandler);
	public ICommand UpdateEmailCommand => new Command(UpdateEmailHandler);
	public ICommand UpdatePasswordCommand => new Command(UpdatePasswordHandler);
    public ICommand DeleteAccountCommand => new Command(DeleteAccountHandler);


    public SettingsPage()
	{
        InitializeComponent();

        BindingContext = this;

        RetrieveUserData();
    }


    private async void LogoutHandler()
	{
        bool answer = await DisplayAlert("Uwaga", "Czy chcesz si� wylogowa�?", "Tak", "Nie");
        if (!answer)
            return;

        SecureStorage.Remove("auth_token");
		await Shell.Current.GoToAsync(nameof(LoginPage));
	}

    private async void RetrieveUserData()
    {
        try
        {
            var response = await _apiService.GetUserData();
            if (response != null && response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonSerializer.Deserialize<UserData>(stringResponse);

                if (jsonResponse == null)
                    throw new Exception("Nie uda�o si� za�adowa� stronty");

                Username = jsonResponse.Username;
                Email = jsonResponse.Email;
            }

            else throw new Exception("Nie uda�o si� za�adowa� stronty");
        }
        catch (Exception ex)
        {
            await Shell.Current.GoToAsync($"{nameof(ErrorPage)}?msg={ex.Message}");
        }
    }

    private async void UpdateUsernameHandler()
    {
        try
        {
            if (!IsUsernameValid)
            {
                await DisplayAlert("B��d", "Niepoprawna nazwa u�ytkownika", "OK");
                return;
            }

            var json = new JsonObject();
            json.Add("username", Username);
            var requestSring = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

            var response = await _apiService.UpdateUsername(requestSring);

            if (response != null && response.IsSuccessStatusCode)
            {
                await DisplayAlert("Komunikat", "Nazwa u�ytkownika zaktualizowana", "OK");
                return;
            }

            await DisplayAlert("Komunikat", "Nie uda�o si� zaktulaizowa� informacji o u�ytkowniku", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.GoToAsync($"{nameof(ErrorPage)}?msg={ex.Message}");
        }
    }

    private async void UpdateEmailHandler()
    {
        try
        {
            if (!IsEmailValid)
            {
                await DisplayAlert("B��d", "Niepoprawny email", "OK");
                return;
            }

            var json = new JsonObject();
            json.Add("email", Email);
            var requestSring = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

            var response = await _apiService.UpdateEmail(requestSring);

            if (response != null && response.IsSuccessStatusCode)
            {
                await DisplayAlert("Komunikat", "Email zaktualizowany", "OK");
                return;
            }

            await DisplayAlert("Komunikat", "Nie uda�o si� zaktulaizowa� informacji o u�ytkowniku", "OK");
        }
        catch (Exception ex )
        {
            await Shell.Current.GoToAsync($"{nameof(ErrorPage)}?msg={ex.Message}");
        }
    }

    private async void UpdatePasswordHandler()
    {
        if (string.IsNullOrEmpty(OldPassword))
        {
            await DisplayAlert("Komunikat", "Pole ze starym has�em nie mo�e by� puste", "OK");
            return;
        }

        if (!StaticMethods.IsPasswordValid(NewPassword))
        {
            await DisplayAlert("Komunikat", "Nowe has�o musi zawiera�:\n8 - 20 znak�w\nWielk� liter� oraz cyfr�", "OK");
            return;
        }

        if (NewPassword != RepeatedNewPassword)
        {
            await DisplayAlert("Komunikat", "Nowe has�a r�ni� si�", "OK");
            return;
        }

        try
        {
            var json = new JsonObject();
            json.Add("old_password", OldPassword);
            json.Add("new_password", NewPassword);

            var stringRequest = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            var response = await _apiService.UpdatePassword(stringRequest);

            if (response != null && response.IsSuccessStatusCode)
            {
                await DisplayAlert("Komunikat", "Has�o zmienione pomy�lnie", "OK");

                OldPassword = string.Empty;
                NewPassword = string.Empty;
                RepeatedNewPassword = string.Empty;

                OnPropertyChanged(nameof(OldPassword));
                OnPropertyChanged(nameof(NewPassword));
                OnPropertyChanged(nameof(RepeatedNewPassword));

                return;
            }

            await DisplayAlert("Komunikat", "Nie uda�o si� zmieni� has�a", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.GoToAsync($"{nameof(ErrorPage)}?msg={ex.Message}");
        }
    }

    private async void DeleteAccountHandler()
    {
        bool answer = await DisplayAlert("Uwaga", "Czy napewno chcesz trwale usun�� to konto?\nWraz z nim stracisz wszystkie zestawy Fishek", "Tak", "Nie");
        if (!answer)
            return;

        try
        {
            var response = await _apiService.DeleteUser();
            if (response != null && response.IsSuccessStatusCode)
            {
                await DisplayAlert("Komunikat", "Usuni�to u�ytkownika pomy�lnie", "OK");
                SecureStorage.Remove("auth_token");
                await Shell.Current.GoToAsync(nameof(LoginPage));
            }

            else await DisplayAlert("Komunikat", "Nie uda�o si� usun�� u�ytkownika", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.GoToAsync($"{nameof(ErrorPage)}?msg={ex.Message}");
        }
    }


    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
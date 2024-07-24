using System.Windows.Input;

namespace Fishki.Maui.Views;

public partial class SettingsPage : ContentPage
{
	public ICommand ReturnButtonCommand => new Command(() => Shell.Current.GoToAsync(".."));
	public ICommand LogoutButtonCommand => new Command(LogoutHandler);


    public SettingsPage()
	{
		InitializeComponent();

		BindingContext = this;
	}


	private async void LogoutHandler()
	{
        bool answer = await DisplayAlert("Uwaga", "Czy chcesz siê wylogowaæ?", "Tak", "Nie");
        if (!answer)
            return;

        SecureStorage.Remove("auth_token");
		await Shell.Current.GoToAsync(nameof(LoginPage));
	}
}
using System.Windows.Input;

namespace Fishki.Maui.Views;

public partial class SettingsPage : ContentPage
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string RepeatedNewPassword { get; set; }

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
	}


	private async void LogoutHandler()
	{
        bool answer = await DisplayAlert("Uwaga", "Czy chcesz siê wylogowaæ?", "Tak", "Nie");
        if (!answer)
            return;

        SecureStorage.Remove("auth_token");
		await Shell.Current.GoToAsync(nameof(LoginPage));
	}

    private void UpdateUsernameHandler()
    {
        
    }

    private void UpdateEmailHandler()
    {
        
    }

    private void UpdatePasswordHandler()
    {
        
    }

    private void DeleteAccountHandler()
    {

    }
}
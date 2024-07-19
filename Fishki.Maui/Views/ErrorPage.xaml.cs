using System.ComponentModel;
using System.Windows.Input;

namespace Fishki.Maui.Views;

[QueryProperty(nameof(ErrorMessage), "msg")]
public partial class ErrorPage : ContentPage, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;

	private string _errorMessage;
	public string ErrorMessage
	{
		get => _errorMessage;
		set
		{
			_errorMessage = value;
			OnPropertyChanged(nameof(ErrorMessage));
		}
	}

	public ICommand ReturnButtonCommand => new Command(() =>
	{
		FishkiSetsPage.ShouldBeRefreshed = true;
        FishkiDetailsPage.ShouldBeRefreshed = true;

        Shell.Current.GoToAsync($"//{nameof(FishkiSetsPage)}");
    });

    public ErrorPage()
	{
		InitializeComponent();

		BindingContext = this;
	}


	private void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
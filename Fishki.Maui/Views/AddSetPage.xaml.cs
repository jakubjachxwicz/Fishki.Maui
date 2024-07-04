using System.Windows.Input;

namespace Fishki.Maui.Views;

public partial class AddSetPage : ContentPage
{
	public ICommand CancelButtonCommand { get; set; }
	
	public AddSetPage()
	{
		InitializeComponent();

		CancelButtonCommand = new Command(() => Shell.Current.GoToAsync(".."));

		BindingContext = this;
	}
}
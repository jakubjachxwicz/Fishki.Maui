using System.Windows.Input;
using Fishki.Maui.Models;

namespace Fishki.Maui.Views;

public partial class WordsListPage : ContentPage
{
	public ICommand ReturnButtonCommand => new Command(() => Shell.Current.GoToAsync(".."));
    public List<Words> WordsList { get; set; }

    public WordsListPage()
	{
		WordsList = FishkiDetailsPage.WordsList;
		
		InitializeComponent();

		BindingContext = this;
	}
}
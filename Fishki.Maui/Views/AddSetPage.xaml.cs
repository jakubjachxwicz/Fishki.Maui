using Fishki.Maui.Models;
using Fishki.Maui.Utils;
using System.ComponentModel;
using System.Windows.Input;
using System.Text;
using System.Text.Json.Nodes;

namespace Fishki.Maui.Views;

public partial class AddSetPage : ContentPage, INotifyPropertyChanged
{
    private readonly FishkiApiService _apiService = FishkiSetsPage._apiService;

    public AddSetPage()
	{
        InitializeComponent();
	}

    private async void OnSaveHandler(object sender, EventArgs e)
    {
        var json = new JsonObject();
        json.Add("name", FishkiControl.FishkiName);
        json.Add("lang_1", FishkiControl.FirstSelectedLanguage.Code);
        json.Add("lang_2", FishkiControl.SecondSelectedLanguage.Code);

        var request = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
        await _apiService.AddSet(request);

        await DisplayAlert("Komunikat", "Zestaw dodany pomyœlnie", "OK");
        await Shell.Current.GoToAsync($"..?refresh=true");
    }

    private void OnCancelHandler(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..?refresh=false");
    }

    private void OnErrorHandler(object sender, string e)
    {
        DisplayAlert("B³¹d", e, "OK");
    }
}
using Fishki.Maui.Utils;
using System.ComponentModel;
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
        try
        {
            var json = new JsonObject();
            json.Add("name", FishkiControl.FishkiName);
            json.Add("lang_1", FishkiControl.FirstSelectedLanguage.Code);
            json.Add("lang_2", FishkiControl.SecondSelectedLanguage.Code);

            var requestString = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            var response = await _apiService.AddSet(requestString);

            if (response == null)
                throw new Exception("Problem z wys³aniem danych");

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Komunikat", "Zestaw dodany pomyœlnie", "OK");
                FishkiSetsPage.ShouldBeRefreshed = true;
                await Shell.Current.GoToAsync($"..");
            }
            else
            {
                await DisplayAlert("Komunikat", "Wyst¹pi³ problem podczas dodawania zestawu", "OK");
                await Shell.Current.GoToAsync($"..");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.GoToAsync($"{nameof(ErrorPage)}?msg={ex.Message}");
        }
    }

    private void OnCancelHandler(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..");
    }

    private void OnErrorHandler(object sender, string e)
    {
        DisplayAlert("B³¹d", e, "OK");
    }
}
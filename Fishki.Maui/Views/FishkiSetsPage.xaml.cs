using Fishki.Maui.Utils;
using System.Diagnostics;

namespace Fishki.Maui.Views;

public partial class FishkiSetsPage : ContentPage
{
	private readonly FishkiApiService _apiService;
    
    public FishkiSetsPage()
	{
		InitializeComponent();

        _apiService = new FishkiApiService();
	}


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var apiResponse = await _apiService.GetAllSets();
        if (apiResponse != null)
        {
            foreach (var fishkiSet in apiResponse)
            {
                Debug.WriteLine($"{fishkiSet.Name}, {fishkiSet.WordsCount}");
            }
        }
    }
}
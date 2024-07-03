using Fishki.Maui.Models;
using Fishki.Maui.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Fishki.Maui.Views;

public partial class FishkiSetsPage : ContentPage
{
    private readonly FishkiApiService _apiService;
    private ObservableCollection<FishkiSet> _fishkiSets;

    public ObservableCollection<FishkiSet> FishkiSets
    {
        get => _fishkiSets;
        set
        {
            _fishkiSets = value;
            OnPropertyChanged(nameof(FishkiSets));
        }
    }

    public FishkiSetsPage()
    {
        InitializeComponent();

        _fishkiSets = new ObservableCollection<FishkiSet>();
        _apiService = new FishkiApiService();

        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        RefreshFishkiList();
    }

    private async void RefreshFishkiList()
    {
        var apiResponse = await _apiService.GetAllSets();
        if (apiResponse != null)
        {
            foreach (var fishkiSet in apiResponse)
            {
                fishkiSet.FirstFlagIconSource = $"FlagIcons/flag_{fishkiSet.FirstLanguage}.png";
                fishkiSet.SecondFlagIconSource = $"FlagIcons/flag_{fishkiSet.SecondLanguage}.png";
                _fishkiSets.Add(fishkiSet);
            }
        }
    }
}
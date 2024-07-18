using Fishki.Maui.Models;
using Fishki.Maui.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Fishki.Maui.Views;

public partial class FishkiSetsPage : ContentPage, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public static FishkiApiService _apiService;
    private ObservableCollection<FishkiSet> _fishkiSets;
    private bool _isRefresing;
    public ICommand RefreshCommand { get; set; }
    public ICommand AddSetCommand { get; set; }
    public ICommand ItemClickedCommand => new Command(ItemClickedHandler);
    public static bool ShouldBeRefreshed { get; set; } = false;

    public ObservableCollection<FishkiSet> FishkiSets
    {
        get => _fishkiSets;
        set
        {
            _fishkiSets = value;
            OnPropertyChanged(nameof(FishkiSets));
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        FishkiCollectionView.SelectedItem = null;
        FishkiDetailsPage.ShouldBeRefreshed = true;

        if (ShouldBeRefreshed)
        {
            ShouldBeRefreshed = false;
            RefreshFishkiList();
        }
    }

    public bool IsRefreshing
    {
        get => _isRefresing;
        set
        {
            _isRefresing = value;
            OnPropertyChanged(nameof(IsRefreshing));
        }
    }

    public FishkiSetsPage()
    {
        InitializeComponent();

        _fishkiSets = new ObservableCollection<FishkiSet>();
        _apiService = new FishkiApiService();

        RefreshCommand = new Command(() =>
        {
            RefreshFishkiList();
            IsRefreshing = false;
        });

        AddSetCommand = new Command(() => Shell.Current.GoToAsync(nameof(AddSetPage)));

        BindingContext = this;

        RefreshFishkiList();
    }

    private void ItemClickedHandler(object obj)
    {
        if (obj == null)
            return;
        
        var item = (FishkiSet)obj;
        Shell.Current.GoToAsync($"FishkiDetailsPage?id={item.SetId}");
    }

    private async void RefreshFishkiList()
    {
        var temp = new ObservableCollection<FishkiSet>();
        
        var apiResponse = await _apiService.GetAllSets();

        if (apiResponse != null)
        {
            foreach (var fishkiSet in apiResponse)
            {
                fishkiSet.FirstFlagIconSource = $"flag_{fishkiSet.FirstLanguage}.png";
                fishkiSet.SecondFlagIconSource = $"flag_{fishkiSet.SecondLanguage}.png";
                temp.Add(fishkiSet);
            }
        }

        FishkiSets = temp;
        ShouldBeRefreshed = false;
    }

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
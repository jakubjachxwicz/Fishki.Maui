using Fishki.Maui.Models;
using Fishki.Maui.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Input;

namespace Fishki.Maui.Views;

public partial class FishkiSetsPage : ContentPage, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private readonly FishkiApiService _apiService;
    private ObservableCollection<FishkiSet> _fishkiSets;
    private bool _isRefresing;
    public ICommand RefreshCommand { get; set; }

    public ObservableCollection<FishkiSet> FishkiSets
    {
        get => _fishkiSets;
        set
        {
            _fishkiSets = value;
            OnPropertyChanged(nameof(FishkiSets));
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
            Debug.WriteLine("pierdolone gówno");
            RefreshFishkiList();
            IsRefreshing = false;
        });

        kurwa.Command = RefreshCommand;

        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        RefreshFishkiList();
    }

    private async void RefreshFishkiList()
    {
        var temp = new ObservableCollection<FishkiSet>();
        
        var apiResponse = await _apiService.GetAllSets();

        if (apiResponse != null)
        {
            foreach (var fishkiSet in apiResponse)
            {
                fishkiSet.FirstFlagIconSource = $"FlagIcons/flag_{fishkiSet.FirstLanguage}.png";
                fishkiSet.SecondFlagIconSource = $"FlagIcons/flag_{fishkiSet.SecondLanguage}.png";
                temp.Add(fishkiSet);
            }
        }

        FishkiSets = temp;

        Debug.WriteLine("awjkdbawhj");
    }

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
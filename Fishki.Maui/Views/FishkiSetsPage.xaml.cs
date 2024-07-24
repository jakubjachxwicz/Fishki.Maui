using Fishki.Maui.Models;
using Fishki.Maui.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Input;

namespace Fishki.Maui.Views;

public partial class FishkiSetsPage : ContentPage, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private readonly FishkiApiService _apiService = LoginPage.apiService;
    private ObservableCollection<FishkiSet> _fishkiSets;
    private bool _isRefresing;
    public ICommand RefreshCommand { get; set; }
    public ICommand AddSetCommand { get; set; }
    //public ICommand LogoutCommand => new Command(LogoutHandler);
    public ICommand ItemClickedCommand => new Command(ItemClickedHandler);
    public ICommand SettingsButtonCommand => new Command(() => Shell.Current.GoToAsync(nameof(SettingsPage)));
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
        
        try
        {
            var apiResponse = await _apiService.GetAllSets();
            if (apiResponse == null)
                throw new Exception("Problem z pobraniem danych");

            if (apiResponse.IsSuccessStatusCode)
            {
                var stringData = await apiResponse.Content.ReadAsStringAsync();
                var jsonData = JsonSerializer.Deserialize<List<FishkiSet>>(stringData);

                foreach (var fishkiSet in jsonData)
                {
                    fishkiSet.FirstFlagIconSource = $"flag_{fishkiSet.FirstLanguage}.png";
                    fishkiSet.SecondFlagIconSource = $"flag_{fishkiSet.SecondLanguage}.png";
                    temp.Add(fishkiSet);
                }

                FishkiSets = temp;
                ShouldBeRefreshed = false;
            }
            
            else throw new Exception($"Wyst¹pi³ problem. Kod b³êdu: {((int)apiResponse.StatusCode)}");
        }
        catch (Exception ex)
        {
            await Shell.Current.GoToAsync($"{nameof(ErrorPage)}?msg={ex.Message}");
        }
    }

    //private void LogoutHandler()
    //{
    //    SecureStorage.Remove("auth_token");
    //    Shell.Current.GoToAsync(nameof(LoginPage));
    //}

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
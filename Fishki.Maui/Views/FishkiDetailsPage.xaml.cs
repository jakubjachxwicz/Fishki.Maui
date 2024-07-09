using System.ComponentModel;
using Fishki.Maui.Models;
using System.Windows.Input;
using Fishki.Maui.Utils;

namespace Fishki.Maui.Views;

[QueryProperty(nameof(SetId), "id")]
public partial class FishkiDetailsPage : ContentPage, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;
    private readonly FishkiApiService _apiService = FishkiSetsPage._apiService;
    private int _setId;
	public int SetId
	{ 
		get => _setId;
		set
		{
			_setId = value;
			OnPropertyChanged(nameof(SetId));
		}
	}
    public string WordsCount { get => $"Liczba s³ówek: {CurrentSet.WordsCount}"; set { } }
    public string FishkiSetName { get => CurrentSet.Name; set { } }
    public string FirstLanguageName { get => Languages.List.Find((x) => x.Code == CurrentSet.FirstLanguage).Name.ToLower(); set { } }
    public string SecondLanguageName { get => Languages.List.Find((x) => x.Code == CurrentSet.SecondLanguage).Name.ToLower(); set { } }
    public string FirstFlagUri { get => $"flag_{CurrentSet.FirstLanguage}.png"; set { } }
    public string SecondFlagUri { get => $"flag_{CurrentSet.SecondLanguage}.png"; set { } }
	public ICommand ReturnButtonCommand => new Command(() => Shell.Current.GoToAsync("..?refresh=false"));
	public ICommand DeleteFishkiCommand => new Command(DeleteFishkiSet);
    public FishkiSet CurrentSet { get; set; }
    public List<Words> WordsList { get; set; }

    public FishkiDetailsPage()
	{
		InitializeComponent();

		CurrentSet = new FishkiSet
		{
			Name = "",
			WordsCount = 0,
			FirstLanguage = "xx",
			SecondLanguage = "xx"
		};

		BindingContext = this;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

		GetSetInfo();
		GetWordsList();
    }


	private async void GetSetInfo()
	{
		var apiResponse = await _apiService.GetSet(SetId);

        if (apiResponse != null)
        {
            CurrentSet = apiResponse;

			OnPropertyChanged(nameof(WordsCount));
			OnPropertyChanged(nameof(FishkiSetName));
			OnPropertyChanged(nameof(FirstLanguageName));
			OnPropertyChanged(nameof(SecondLanguageName));
			OnPropertyChanged(nameof(FirstFlagUri));
			OnPropertyChanged(nameof(SecondFlagUri));
        }
    }

	private async void GetWordsList()
	{

	}

	private async void DeleteFishkiSet()
	{
        bool answer = await DisplayAlert("Uwaga", "Czy chcesz usun¹æ ten zestaw Fishek?", "Tak", "Nie");
		if (!answer)
			return;

		await _apiService.DeleteSet(SetId);
		await Shell.Current.GoToAsync("..?refresh=true");
    }

    private void OnPropertyChanged(string name)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
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
	public static bool ShouldBeRefreshed { get; set; } = true;
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
	public ICommand ReturnButtonCommand => new Command(() => Shell.Current.GoToAsync(".."));
	public ICommand DeleteFishkiCommand => new Command(DeleteFishkiSet);
	public ICommand EditButtonCommand => new Command(EditButtonHandler);
	public ICommand WordsListCommand => new Command(() => Shell.Current.GoToAsync(nameof(WordsListPage)));
    public FishkiSet CurrentSet { get; set; }
    public static List<Words> WordsList { get; set; }

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

		if (ShouldBeRefreshed)
		{
			ShouldBeRefreshed = false;

            GetSetInfo();
            GetWordsList();
        }
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
		List<Words> temp = new List<Words>();

		var apiResponse = await _apiService.GetWords(SetId);

		if (apiResponse != null)
		{
			foreach (var words in apiResponse)
			{
				temp.Add(words);
			}
		}

		WordsList = temp;
	}

	private async void DeleteFishkiSet()
	{
        bool answer = await DisplayAlert("Uwaga", "Czy chcesz usun¹æ ten zestaw Fishek?", "Tak", "Nie");
		if (!answer)
			return;

		await _apiService.DeleteSet(SetId);
		FishkiSetsPage.ShouldBeRefreshed = true;
		await Shell.Current.GoToAsync("..");
    }

	private void EditButtonHandler()
	{
        Shell.Current.GoToAsync($"{nameof(EditFishkiSetPage)}?set_id={SetId}&name={FishkiSetName}&lang_1={CurrentSet.FirstLanguage}&lang_2={CurrentSet.SecondLanguage}");
        //Shell.Current.GoToAsync($"{nameof(EditFishkiSetPage)}");
    }

    private void OnPropertyChanged(string name)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
using System.ComponentModel;
using Fishki.Maui.Models;
using System.Windows.Input;
using Fishki.Maui.Utils;
using System.Text.Json;

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
	public ICommand ManageWordsButtonCommand => new Command(ManageWordsButtonHandler);
	public ICommand LearnButtonCommand => new Command(LearnButtonHandler);
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
		try
		{
            var apiResponse = await _apiService.GetSet(SetId);

			if (apiResponse == null)
				throw new Exception("Nie uda³o siê pobraæ danych o zestawie Fishek");

			if (apiResponse.IsSuccessStatusCode)
			{
				var stringData = await apiResponse.Content.ReadAsStringAsync();
				var jsonData = JsonSerializer.Deserialize<FishkiSet>(stringData);

				if (jsonData == null)
					throw new Exception("Nie uda³o siê pobraæ danych o zestawie Fishek");

				CurrentSet = jsonData;

                OnPropertyChanged(nameof(WordsCount));
				OnPropertyChanged(nameof(FishkiSetName));
				OnPropertyChanged(nameof(FirstLanguageName));
				OnPropertyChanged(nameof(SecondLanguageName));
				OnPropertyChanged(nameof(FirstFlagUri));
				OnPropertyChanged(nameof(SecondFlagUri));
			}

			else
				throw new Exception("Nie uda³o siê pobraæ danych o zestawie Fishek");
        }
		catch (Exception ex)
		{
			await Shell.Current.GoToAsync($"{nameof(ErrorPage)}?msg={ex.Message}");
		}
    }

	private async void GetWordsList()
	{
		try
		{
            var apiResponse = await _apiService.GetWords(SetId);

			if (apiResponse != null && apiResponse.IsSuccessStatusCode)
			{
				var stringData = await apiResponse.Content.ReadAsStringAsync();
				var jsonData = JsonSerializer.Deserialize<List<Words>>(stringData);

				if (jsonData == null)
                    throw new Exception("Nie uda³o siê pobraæ s³ówek z tego zestawu");

				WordsList = new List<Words>(jsonData);
            }

			else
                throw new Exception("Nie uda³o siê pobraæ s³ówek z tego zestawu");
        }
		catch (Exception ex)
		{
			await Shell.Current.GoToAsync($"{nameof(ErrorPage)}?msg={ex.Message}");
		}
	}

	private async void DeleteFishkiSet()
	{
        bool answer = await DisplayAlert("Uwaga", "Czy chcesz usun¹æ ten zestaw Fishek?", "Tak", "Nie");
		if (!answer)
			return;

		try
		{
            var response = await _apiService.DeleteSet(SetId);

			if (response != null && response.IsSuccessStatusCode)
			{
				FishkiSetsPage.ShouldBeRefreshed = true;
				await Shell.Current.GoToAsync("..");
			}
			else
				throw new Exception("Nie uda³o siê usun¹æ tego zestawu Fishek");
        }
		catch (Exception ex)
		{
			await Shell.Current.GoToAsync($"{nameof(ErrorPage)}?msg={ex.Message}");
		}
    }

	private void EditButtonHandler()
	{
        Shell.Current.GoToAsync($"{nameof(EditFishkiSetPage)}?set_id={SetId}&name={FishkiSetName}&lang_1={CurrentSet.FirstLanguage}&lang_2={CurrentSet.SecondLanguage}");
    }

	private void ManageWordsButtonHandler()
	{
		Shell.Current.GoToAsync($"{nameof(ManageWordsPage)}?set_id={SetId}&lang_1={FirstLanguageName}&lang_2={SecondLanguageName}");
	}

	private void LearnButtonHandler(object obj)
	{
		if (CurrentSet.WordsCount > 0)
			Shell.Current.GoToAsync($"{nameof(LearnPage)}?random={obj}");

		else DisplayAlert("Komunikat", "Brak s³ówek w tym zestawie Fishek", "OK");
    }

    private void OnPropertyChanged(string name)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
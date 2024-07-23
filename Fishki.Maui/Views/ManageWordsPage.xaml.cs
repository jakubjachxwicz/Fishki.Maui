using Fishki.Maui.Models;
using Fishki.Maui.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json.Nodes;
using System.Web;
using System.Text;
using System.Windows.Input;
using System.Text.Json;

namespace Fishki.Maui.Views;

public partial class ManageWordsPage : ContentPage, IQueryAttributable, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private readonly FishkiApiService _fishkiApiService = LoginPage.apiService;
    public int SetId { get; set; }
    private ObservableCollection<Words> _wordsList;
    public ObservableCollection<Words> WordsList
    {
        get => _wordsList;
        set
        {
            _wordsList = value;
            OnPropertyChanged(nameof(WordsList));
        }
    }
    private string _firstDisplayLanguage;
    public string FirstDisplayLanguage
    {
        get => $"{_firstDisplayLanguage}:";
        set
        {
            _firstDisplayLanguage = value;
            OnPropertyChanged(nameof(FirstDisplayLanguage));
        }
    }
    private string _secondDisplayLanguage;
    public string SecondDisplayLanguage
    {
        get => $"{_secondDisplayLanguage}:";
        set
        {
            _secondDisplayLanguage = value;
            OnPropertyChanged(nameof(SecondDisplayLanguage));
        }
    }
    private string _firstNewWordEntry;
    public string FirstNewWordsEntry
    {
        get => _firstNewWordEntry;
        set
        {
            _firstNewWordEntry = value;
            OnPropertyChanged(nameof(FirstNewWordsEntry));
        }
    }
    private string _secondNewWordEntry;
    public string SecondNewWordsEntry
    {
        get => _secondNewWordEntry;
        set
        {
            _secondNewWordEntry = value;
            OnPropertyChanged(nameof(SecondNewWordsEntry));
        }
    }
    public ICommand ReturnButtonCommand => new Command(ReturnButtonHandler);
    public ICommand AddWordsCommand => new Command(AddWordsHandler);

    public ManageWordsPage()
	{
		FirstNewWordsEntry = string.Empty;
        SecondNewWordsEntry = string.Empty;

        WordsList = new ObservableCollection<Words>(FishkiDetailsPage.WordsList);
        
        InitializeComponent();

        BindingContext = this;
	}

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("set_id", out var idObj) && idObj is string idStr && int.TryParse(idStr, out var id))
        {
            SetId = id;
        }

        if (query.TryGetValue("lang_1", out var lang_1_Obj) && lang_1_Obj is string lang_1)
        {
            FirstDisplayLanguage = HttpUtility.UrlDecode(lang_1);
        }

        if (query.TryGetValue("lang_2", out var lang_2_Obj) && lang_2_Obj is string lang_2)
        {
            SecondDisplayLanguage = HttpUtility.UrlDecode(lang_2);
        }
    }

    private void ReturnButtonHandler()
    {
        FishkiSetsPage.ShouldBeRefreshed = true;
        FishkiDetailsPage.ShouldBeRefreshed = true;

        Shell.Current.GoToAsync("..");
    }

    private async void AddWordsHandler()
    {
        if (FirstNewWordsEntry == string.Empty || FirstNewWordsEntry.Length > 36)
        {
            await DisplayAlert("B³¹d", "Niepoprawna d³ugoœæ pierwszego s³ówka", "OK");
            return;
        }

        if (SecondNewWordsEntry == string.Empty || SecondNewWordsEntry.Length > 36)
        {
            await DisplayAlert("B³¹d", "Niepoprawna d³ugoœæ drugiego s³ówka", "OK");
            return;
        }


        try
        {
            var jsonRequest = new JsonObject();
            jsonRequest.Add("word_1", FirstNewWordsEntry);
            jsonRequest.Add("word_2", SecondNewWordsEntry);

            var requestString = new StringContent(jsonRequest.ToString(), Encoding.UTF8, "application/json");
            var apiResponse = await _fishkiApiService.AddWords(SetId, requestString);

            if (apiResponse != null && apiResponse.IsSuccessStatusCode)
            {
                FirstNewWordsEntry = string.Empty;
                SecondNewWordsEntry = string.Empty;

                FishkiSetsPage.ShouldBeRefreshed = true;
                FishkiDetailsPage.ShouldBeRefreshed = true;

                RefreshWordsList();
            }
            else
                throw new Exception("Nie uda³o siê dodaæ nowych s³ówek do zestawu");
        }
        catch (Exception ex)
        {
            await Shell.Current.GoToAsync($"{nameof(ErrorPage)}?msg={ex.Message}");
        }
    }

    private async void RefreshWordsList()
    {
        try
        {
            var apiResponse = await _fishkiApiService.GetWords(SetId);

            if (apiResponse != null && apiResponse.IsSuccessStatusCode)
            {
                var stringData = await apiResponse.Content.ReadAsStringAsync();
                var jsonData = JsonSerializer.Deserialize<List<Words>>(stringData);

                if (jsonData == null)
                    throw new Exception("Nie uda³o siê wczytaæ listy s³ówek");

                WordsList = new ObservableCollection<Words>(jsonData);
            }
            else
                throw new Exception("Nie uda³o siê wczytaæ listy s³ówek");
        }
        catch (Exception ex)
        {
            await Shell.Current.GoToAsync($"{nameof(ErrorPage)}?msg={ex.Message}");
        }
    }

    private async void DeleteWordsHandler(object sender, EventArgs e)
    {
        try
        {
            var button = sender as ImageButton;
            if (button == null) return;

            Words words = button.CommandParameter as Words;
            if (words == null) return;

            var apiResponse = await _fishkiApiService.DeleteWords(SetId, words.WordsId);

            if (apiResponse != null && apiResponse.IsSuccessStatusCode)
            {
                FishkiSetsPage.ShouldBeRefreshed = true;
                FishkiDetailsPage.ShouldBeRefreshed = true;

                RefreshWordsList();
            }
            else await DisplayAlert("Komunikat", "Nie uda³o siê usun¹æ s³ówke", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.GoToAsync($"{nameof(ErrorPage)}?msg={ex.Message}");
        }
    }

    private async void UpdateWordsHandler(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        if (button == null) return;

        Words words = button.CommandParameter as Words;
        if (words == null) return;

        if (words.First == string.Empty || words.First.Length > 36)
        {
            await DisplayAlert("B³¹d", "Niepoprawna d³ugoœæ pierwszego s³ówka", "OK");
            return;
        }

        if (words.Second == string.Empty || words.Second.Length > 36)
        {
            await DisplayAlert("B³¹d", "Niepoprawna d³ugoœæ drugiego s³ówka", "OK");
            return;
        }

        try
        {
            var requestJson = new JsonObject();
            requestJson.Add("word_1", words.First);
            requestJson.Add("word_2", words.Second);

            var requestString = new StringContent(requestJson.ToString(), Encoding.UTF8, "application/json");

            var response = await _fishkiApiService.UpdateWords(SetId, words.WordsId, requestString);

            if (response != null && response.IsSuccessStatusCode)
            {
                FishkiSetsPage.ShouldBeRefreshed = true;
                FishkiDetailsPage.ShouldBeRefreshed = true;

                RefreshWordsList();
            }
            else await DisplayAlert("Komunikat", "Nie uda³o siê zaktualizowaæ s³ówek", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.GoToAsync($"{nameof(ErrorPage)}?msg={ex.Message}");
        }
    }


    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
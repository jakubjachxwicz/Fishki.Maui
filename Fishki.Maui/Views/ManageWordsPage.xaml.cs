using Fishki.Maui.Models;
using Fishki.Maui.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json.Nodes;
using System.Web;
using System.Text;
using System.Windows.Input;

namespace Fishki.Maui.Views;

public partial class ManageWordsPage : ContentPage, IQueryAttributable, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private readonly FishkiApiService _fishkiApiService = FishkiSetsPage._apiService;
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

        var json = new JsonObject();
        json.Add("word_1", FirstNewWordsEntry);
        json.Add("word_2", SecondNewWordsEntry);

        var request = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
        await _fishkiApiService.AddWords(SetId, request);

        FirstNewWordsEntry = string.Empty;
        SecondNewWordsEntry = string.Empty;

        FishkiSetsPage.ShouldBeRefreshed = true;
        FishkiDetailsPage.ShouldBeRefreshed= true;

        RefreshWordsList();
    }

    private async void RefreshWordsList()
    {
        var temp = new ObservableCollection<Words>();

        var apiResponse = await _fishkiApiService.GetWords(SetId);

        foreach (var doc in apiResponse)
            temp.Add(doc);

        WordsList = temp;
    }

    private async void DeleteWordsHandler(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        Words words = button.CommandParameter as Words;

        await _fishkiApiService.DeleteWords(SetId, words.WordsId);

        FishkiSetsPage.ShouldBeRefreshed = true;
        FishkiDetailsPage.ShouldBeRefreshed = true;

        RefreshWordsList();
    }

    private async void UpdateWordsHandler(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        Words words = button.CommandParameter as Words;

        var json = new JsonObject();
        json.Add("word_1", words.First);
        json.Add("word_2", words.Second);

        var request = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

        await _fishkiApiService.UpdateWords(SetId, words.WordsId, request);

        FishkiSetsPage.ShouldBeRefreshed = true;
        FishkiDetailsPage.ShouldBeRefreshed = true;

        RefreshWordsList();
    }


    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
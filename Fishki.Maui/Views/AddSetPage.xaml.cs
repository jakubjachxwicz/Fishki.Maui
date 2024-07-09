using Fishki.Maui.Models;
using Fishki.Maui.Utils;
using System.ComponentModel;
using System.Windows.Input;
using System.Text;
using System.Text.Json.Nodes;

namespace Fishki.Maui.Views;

public partial class AddSetPage : ContentPage, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
	public ICommand CancelButtonCommand { get; set; }
	public ICommand AddButtonCommand { get; set; }
    public List<Language> LanguagesList { get; set; }
    public Language FirstSelectedLanguage { get; set; }
    public Language SecondSelectedLanguage { get; set; }
    public string FishkiName { get; set; }
    public string FirstFlagUri { get => $"flag_{FirstSelectedLanguage.Code}.png"; set { } }
    public string SecondFlagUri { get => $"flag_{SecondSelectedLanguage.Code}.png"; set { } }
    public bool NameEntryIsValid { get; set; }

    private readonly FishkiApiService _apiService = FishkiSetsPage._apiService;

    public AddSetPage()
	{
        LanguagesList = Languages.List;

        FirstSelectedLanguage = new Language();
        SecondSelectedLanguage = new Language();

        InitializeComponent();

		CancelButtonCommand = new Command(() => Shell.Current.GoToAsync("..?refresh=false"));
        AddButtonCommand = new Command(AddSet);

        BindingContext = this;
	}

    private async void AddSet()
    {
        if (!NameEntryIsValid)
        {
            await DisplayAlert("B³¹d", "Niepoprawne dane w polu \'Nazwa\'", "OK");
            return;
        }

        if (FirstSelectedLanguage.Id == 0 || SecondSelectedLanguage.Id == 0)
        {
            await DisplayAlert("B³¹d", "Nie wybrano jêzyka", "OK");
            return;
        }

        if (FirstSelectedLanguage.Id < 27 && FirstSelectedLanguage.Id == SecondSelectedLanguage.Id)
        {
            await DisplayAlert("B³¹d", "Wybrane jêzyki s¹ takie same", "OK");
            return;
        }

        var json = new JsonObject();
        json.Add("name", FishkiName);
        json.Add("lang_1", FirstSelectedLanguage.Code);
        json.Add("lang_2", SecondSelectedLanguage.Code);

        var request = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
        await _apiService.AddSet(request);

        await DisplayAlert("Komunikat", "Zestaw dodany pomyœlnie", "OK");
        await Shell.Current.GoToAsync($"..?refresh=true");
    }

    private void FirstLanguageChanged(object sender, EventArgs e)
    {
        OnPropertyChanged("FirstFlagUri");
    }

    private void SecondLanguageChanged(object sender, EventArgs e)
    {
        OnPropertyChanged("SecondFlagUri");
    }

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
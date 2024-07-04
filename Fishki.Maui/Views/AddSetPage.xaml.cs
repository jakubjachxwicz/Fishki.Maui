using Fishki.Maui.Models;
using System.ComponentModel;
using System.Windows.Input;

namespace Fishki.Maui.Views;

public partial class AddSetPage : ContentPage, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
	public ICommand CancelButtonCommand { get; set; }
	public ICommand AddButtonCommand { get; set; }
    public List<Language> Languages { get; set; }
    public Language FirstSelectedLanguage { get; set; }
    public Language SecondSelectedLanguage { get; set; }
    public string FirstFlagUri { get => $"flag_{FirstSelectedLanguage.Code}.png"; set { } }
    public string SecondFlagUri { get => $"flag_{SecondSelectedLanguage.Code}.png"; set { } }

    public AddSetPage()
	{
        Languages = new List<Language>
        {
            new Language {Id = 1, Name = "Arabski", Code = "ae"},
            new Language {Id = 2, Name = "Bu³garski", Code = "bg"},
            new Language {Id = 3, Name = "Bia³oruski", Code = "by"},
            new Language {Id = 4, Name = "Chiñski", Code = "cn"},
            new Language {Id = 5, Name = "Czeski", Code = "cz"},
            new Language {Id = 6, Name = "Niemiecki", Code = "de"},
            new Language {Id = 7, Name = "Duñski", Code = "dk"},
            new Language {Id = 8, Name = "Angielski", Code = "en"},
            new Language {Id = 9, Name = "Hiszpañski", Code = "es"},
            new Language {Id = 10, Name = "Fiñski", Code = "fi"},
            new Language {Id = 11, Name = "Francuski", Code = "fr"},
            new Language {Id = 12, Name = "Grecki", Code = "gr"},
            new Language {Id = 13, Name = "Chorwacki", Code = "hr"},
            new Language {Id = 14, Name = "Wêgierski", Code = "hu"},
            new Language {Id = 15, Name = "Islandzki", Code = "is"},
            new Language {Id = 16, Name = "W³oski", Code = "it"},
            new Language {Id = 17, Name = "Japoñski", Code = "jp"},
            new Language {Id = 18, Name = "Koreañski", Code = "kr"},
            new Language {Id = 19, Name = "Niderlandzki", Code = "nl"},
            new Language {Id = 20, Name = "Norwedzki", Code = "no"},
            new Language {Id = 21, Name = "Polski", Code = "pl"},
            new Language {Id = 22, Name = "Portugalski", Code = "pt"},
            new Language {Id = 23, Name = "Rosyjski", Code = "ru"},
            new Language {Id = 24, Name = "Szwedzki", Code = "se"},
            new Language {Id = 25, Name = "Turecki", Code = "tr"},
            new Language {Id = 26, Name = "Ukraiñski", Code = "ua"},
            new Language {Id = 27, Name = "Inny", Code = "xx"},
        };

        FirstSelectedLanguage = new Language();
        SecondSelectedLanguage = new Language();

        InitializeComponent();

		CancelButtonCommand = new Command(() => Shell.Current.GoToAsync(".."));
        AddButtonCommand = new Command(() => AddSet());

        BindingContext = this;
	}

    private void AddSet()
    {
        // DisplayAlert("alert", $"{FirstSelectedLanguage.Code}, {SecondSelectedLanguage.Code}", "OK");
        // OnPropertyChanged("FirstFlagUri");
        DisplayAlert("alert", $"{FirstFlagUri}", "OK");
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
using Fishki.Maui.Models;
using System.Windows.Input;
using Fishki.Maui.Utils;
using System.ComponentModel;

namespace Fishki.Maui.Views;

public partial class AddEditControl : ContentView, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public event EventHandler<string> OnError;
    public event EventHandler<EventArgs> OnSave;
    public event EventHandler<EventArgs> OnCancel;
    private string _fishkiName;
    public string FishkiName
    {
        get => _fishkiName;
        set
        {
            _fishkiName = value;
            OnPropertyChanged(nameof(FishkiName));
        }
    }
    public List<Language> LanguagesList { get; set; }
    private Language _firstLanguage;
    public Language FirstSelectedLanguage
    {
        get => _firstLanguage;
        set
        {
            _firstLanguage = value;
            OnPropertyChanged(nameof(FirstSelectedLanguage));
        }
    }
    private Language _secondLanguage;
    public Language SecondSelectedLanguage
    {
        get => _secondLanguage;
        set
        {
            _secondLanguage = value;
            OnPropertyChanged(nameof(SecondSelectedLanguage));
        }
    }
    public string FirstFlagUri { get => $"flag_{FirstSelectedLanguage.Code}.png"; set { } }
    public string SecondFlagUri { get => $"flag_{SecondSelectedLanguage.Code}.png"; set { } }
    public ICommand SaveButtonCommand => new Command(SaveButtonHandler);
    public ICommand CancelButtonCommand => new Command(CancelButtonHandler);
    public bool NameEntryIsValid { get; set; }

    public AddEditControl()
	{
        LanguagesList = Languages.List;

        FirstSelectedLanguage = new Language();
        SecondSelectedLanguage = new Language();

        InitializeComponent();

        BindingContext = this;
	}

    private void FirstLanguageChanged(object sender, EventArgs e)
    {
        OnPropertyChanged(nameof(FirstFlagUri));
    }

    private void SecondLanguageChanged(object sender, EventArgs e)
    {
        OnPropertyChanged(nameof(SecondFlagUri));
    }

    private void SaveButtonHandler()
    {
        if (!NameEntryIsValid)
        {
            //await DisplayAlert("B³¹d", "Niepoprawne dane w polu \'Nazwa\'", "OK");
            OnError?.Invoke(this, "Niepoprawne dane w polu \'Nazwa\'");
            return;
        }

        if (FirstSelectedLanguage.Id == 0 || SecondSelectedLanguage.Id == 0)
        {
            //await DisplayAlert("B³¹d", "Nie wybrano jêzyka", "OK");
            OnError?.Invoke(this, "Nie wybrano jêzyka");
            return;
        }

        if (FirstSelectedLanguage.Id < 27 && FirstSelectedLanguage.Id == SecondSelectedLanguage.Id)
        {
            //await DisplayAlert("B³¹d", "Wybrane jêzyki s¹ takie same", "OK");
            OnError?.Invoke(this, "Wybrane jêzyki s¹ takie same");
            return;
        }

        OnSave?.Invoke(this, new EventArgs());
    }

    private void CancelButtonHandler()
    {
        OnCancel?.Invoke(this, new EventArgs());
    }


    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
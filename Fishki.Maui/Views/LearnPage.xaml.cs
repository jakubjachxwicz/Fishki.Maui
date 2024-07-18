namespace Fishki.Maui.Views;

using Fishki.Maui.Models;
using System.ComponentModel;
using System.Windows.Input;

[QueryProperty(nameof(IsRandomOrder), "random")]
public partial class LearnPage : ContentPage, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;
	private bool _isRandomOrder;
    public bool IsRandomOrder
	{
		get => _isRandomOrder;
		set
		{
			_isRandomOrder = value;
			//OnPropertyChanged(nameof(IsRandomOrder));
			if (_isRandomOrder)
			{
                InitRandomOrder();

				OnPropertyChanged(nameof(FrontLabelWord));
				OnPropertyChanged(nameof(BackLabelWord));
            }
        }
	}
    public List<Words> WordsList { get; set; }
    public ICommand CardTappedCommand => new Command(CardTappedHandler);
	public ICommand ReturnButtonCommand => new Command(() => Shell.Current.GoToAsync(".."));
	public ICommand SwitchCardCommand => new Command(SwitchCardHandler);


    private bool _isFrontLabelVisible;
    public bool IsFrontLabelVisible
	{
		get => _isFrontLabelVisible;
		set
		{
			_isFrontLabelVisible = value;
			OnPropertyChanged(nameof(IsFrontLabelVisible));
		}
	}
    private bool _isBackLabelVisible;
    public bool IsBackLabelVisible
    {
        get => _isBackLabelVisible;
        set
        {
            _isBackLabelVisible = value;
            OnPropertyChanged(nameof(IsBackLabelVisible));
        }
    }

	private int _wordIndex = 0;

    public string HeaderText
	{
		get => $"{_wordIndex + 1}/{WordsList.Count}";
		set { }
	}

    //public string FrontLabelWord { get => WordsList[_wordIndex].First; set { } }
    public string FrontLabelWord
	{
		get => IsRandomOrder ? WordsList[_randomIndexes[_wordIndex]].First : WordsList[_wordIndex].First;
		set { }
    }
    //public string BackLabelWord { get => WordsList[_wordIndex].Second; set { } }
    public string BackLabelWord
    {
        get => IsRandomOrder ? WordsList[_randomIndexes[_wordIndex]].Second : WordsList[_wordIndex].Second;
        set { }
    }

    public bool PrevButtonEnabled { get => _wordIndex > 0; set { } }
    public bool NextButtonEnabled { get => _wordIndex < WordsList.Count - 1; set { } }

	private int[] _randomIndexes;


    public LearnPage()
	{
		IsFrontLabelVisible = true;
        IsBackLabelVisible = false;

		WordsList = FishkiDetailsPage.WordsList;

        InitializeComponent();

		BindingContext = this;

		OnPropertyChanged(nameof(HeaderText));
	}

	private async void CardTappedHandler()
	{
        await fishkiCard.RotateYTo((IsFrontLabelVisible ? 90 : -90), 250, Easing.CubicIn);

        IsFrontLabelVisible = !IsFrontLabelVisible;
        IsBackLabelVisible = !IsBackLabelVisible;

        fishkiCard.RotationY = (IsFrontLabelVisible ? 90 : -90);
		await fishkiCard.RotateYTo(0, 250, Easing.CubicOut);
    }

	private async void SwitchCardHandler(object obj)
	{
		if ((string)obj == "prev" && _wordIndex > 0)
		{
			_wordIndex--;

            await fishkiCard.TranslateTo(400, Y, 150, Easing.CubicIn);

            OnPropertyChanged(nameof(HeaderText));
			OnPropertyChanged(nameof(FrontLabelWord));
			OnPropertyChanged(nameof(BackLabelWord));
			OnPropertyChanged(nameof(PrevButtonEnabled));
			OnPropertyChanged(nameof(NextButtonEnabled));

            fishkiCard.TranslationX = -400;
            await fishkiCard.TranslateTo(0, Y, 150, Easing.CubicOut);

            return;
		}

		if ((string)obj == "next" && _wordIndex < WordsList.Count - 1)
		{
			_wordIndex++;

			await fishkiCard.TranslateTo(-400, Y, 150, Easing.CubicIn);

			OnPropertyChanged(nameof(HeaderText));
            OnPropertyChanged(nameof(FrontLabelWord));
            OnPropertyChanged(nameof(BackLabelWord));
            OnPropertyChanged(nameof(PrevButtonEnabled));
            OnPropertyChanged(nameof(NextButtonEnabled));

            fishkiCard.TranslationX = 400;
			await fishkiCard.TranslateTo(0, Y, 150, Easing.CubicOut);
        }
	}

	private void InitRandomOrder()
	{
		int len = WordsList.Count;
		_randomIndexes = Enumerable.Range(0, len).ToArray();

		Random r = new Random();
		for (int i = len - 1; i > 0; i--)
		{
			int j = r.Next(0, i + 1);
			int temp = _randomIndexes[i];
			_randomIndexes[i] = _randomIndexes[j];
			_randomIndexes[j] = temp;
		}
	}


    private void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
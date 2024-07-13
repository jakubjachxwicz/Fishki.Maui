namespace Fishki.Maui.Views;

using System.ComponentModel;
using System.Windows.Input;

public partial class LearnPage : ContentPage, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;
	public ICommand CardTappedCommand => new Command(CardTappedHandler);
	public ICommand ReturnButtonCommand => new Command(() => Shell.Current.GoToAsync(".."));


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


    public LearnPage()
	{
		IsFrontLabelVisible = true;
        IsBackLabelVisible = false;

        InitializeComponent();

		BindingContext = this;
	}

	private async void CardTappedHandler()
	{
        await fishkiCard.RotateYTo((IsFrontLabelVisible ? 90 : -90), 250, Easing.CubicIn);

        IsFrontLabelVisible = !IsFrontLabelVisible;
        IsBackLabelVisible = !IsBackLabelVisible;

		fishkiCard.RotationY = (IsFrontLabelVisible ? 90 : -90);
		await fishkiCard.RotateYTo(0, 250, Easing.CubicOut);
    }


	private void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
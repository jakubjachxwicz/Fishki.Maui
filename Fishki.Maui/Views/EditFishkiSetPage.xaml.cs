using Fishki.Maui.Utils;
using System.Text.Json.Nodes;
using System.Text;
using System.Web;

namespace Fishki.Maui.Views;

public partial class EditFishkiSetPage : ContentPage, IQueryAttributable
{
    private readonly FishkiApiService _fishkiApiService = LoginPage.apiService;
    public int SetId { get; set; }

    public EditFishkiSetPage()
	{
        InitializeComponent();
	}

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("set_id", out var idObj) && idObj is string idStr && int.TryParse(idStr, out var id))
        {
            SetId = id;
        }

        if (query.TryGetValue("name", out var nameObj) && nameObj is string name)
        {
            FishkiControl.FishkiName = HttpUtility.UrlDecode(name);
        }

        if (query.TryGetValue("lang_1", out var lang_1_Obj) && lang_1_Obj is string lang_1)
        {
            FishkiControl.FirstSelectedLanguage = Utils.Languages.List.Find((x) => x.Code == lang_1);
        }

        if (query.TryGetValue("lang_2", out var lang_2_Obj) && lang_2_Obj is string lang_2)
        {
            FishkiControl.SecondSelectedLanguage = Utils.Languages.List.Find((x) => x.Code == lang_2);
        }
    }

    private async void OnSaveHandler(object sender, EventArgs e)
    {
        try
        {
            var json = new JsonObject();
            json.Add("name", FishkiControl.FishkiName);
            json.Add("lang_1", FishkiControl.FirstSelectedLanguage.Code);
            json.Add("lang_2", FishkiControl.SecondSelectedLanguage.Code);

            var requestString = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            var response = await _fishkiApiService.UpdateSet(requestString, SetId);

            if (response == null)
                throw new Exception("Problem z aktualizacj¹ danych");

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Komunikat", "Zestaw zaktualizowany pomyœlnie", "OK");
                FishkiSetsPage.ShouldBeRefreshed = true;
                FishkiDetailsPage.ShouldBeRefreshed = true;
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Komunikat", "Nie uda³o siê zakualizowaæ danych", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.GoToAsync($"{nameof(ErrorPage)}?msg={ex.Message}");
        }
    }

    private void OnCancelHandler(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..");
    }

    private void OnErrorHandler(object sender, string e)
    {
        DisplayAlert("B³¹d", e, "OK");
    }
}
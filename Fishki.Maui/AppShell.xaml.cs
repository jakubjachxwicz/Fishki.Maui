using Fishki.Maui.Views;

namespace Fishki.Maui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(FishkiSetsPage), typeof(FishkiSetsPage));
            Routing.RegisterRoute(nameof(FishkiDetailsPage), typeof(FishkiDetailsPage));
            Routing.RegisterRoute(nameof(AddSetPage), typeof(AddSetPage));
            Routing.RegisterRoute(nameof(EditFishkiSetPage), typeof(EditFishkiSetPage));
            Routing.RegisterRoute(nameof(WordsListPage), typeof(WordsListPage));
            Routing.RegisterRoute(nameof(ManageWordsPage), typeof(ManageWordsPage));
            Routing.RegisterRoute(nameof(LearnPage), typeof(LearnPage));
            Routing.RegisterRoute(nameof(ErrorPage), typeof(ErrorPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        }
    }
}

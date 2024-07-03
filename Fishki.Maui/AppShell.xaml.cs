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
        }
    }
}

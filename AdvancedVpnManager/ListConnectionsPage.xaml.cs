using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AdvancedVpnManager
{
    public sealed partial class ListConnectionsPage : Page
    {
        private readonly ListConnectionsViewModel viewModel;

        public ListConnectionsPage()
        {
            InitializeComponent();

            viewModel = new ListConnectionsViewModel();
            DataContext = viewModel;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await viewModel.RefreshAsync();
        }
    }
}

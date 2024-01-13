using Windows.Networking.Vpn;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AdvancedVpnManager
{
    public sealed partial class EditConnectionPage : Page
    {
        private EditViewModel viewModel;

        public EditConnectionPage()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            App.TryGoBack();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            viewModel = new EditViewModel((VpnNativeProfile)e.Parameter);
            DataContext = viewModel;

            base.OnNavigatedTo(e);
        }
    }
}

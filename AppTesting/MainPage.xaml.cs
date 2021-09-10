using AppTesting.ViewModels;
using Xamarin.Forms;

namespace AppTesting
{
    public partial class MainPage : ContentPage
    {

        private MainPageViewModel mViewModel { get; }
        public MainPage()
        {
            InitializeComponent();
            BindingContext = mViewModel = new MainPageViewModel();
        }
    }
}

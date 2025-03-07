using TestTask.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestTask.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContentOverview : ContentPage
	{
        public ContentOverview(MainPageViewModel viewModel)
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            BindingContext = viewModel;
        }

    }
}
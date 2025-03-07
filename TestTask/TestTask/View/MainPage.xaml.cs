using TestTask.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestTask.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();
            BindingContext = new MainPageViewModel();
        }
	}
}
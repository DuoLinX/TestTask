using Xamarin.Forms;
using TestTask.View;
using System.Globalization;

namespace TestTask
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            CultureInfo culture = new CultureInfo("ru-RU"); 
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

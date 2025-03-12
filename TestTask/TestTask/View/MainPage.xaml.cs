using System;
using System.Globalization;
using TestTask.Interface;
using TestTask.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestTask.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
		private bool isBackPressed = false;
        public MainPage ()
		{
			InitializeComponent ();
            BindingContext = new MainPageViewModel();
        }

        protected override bool OnBackButtonPressed()
        {
            if (isBackPressed) 
            {
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                return true;
            }
            else
            {
                ShoExitWarning();
                return true;
            }
        }

        private async void ShoExitWarning()
        {
            isBackPressed = true;
            bool anser = await DisplayAlert("Выход:", "Вы хотите выйти из приложения?", "Да", "Нет");
            if (anser) 
            {
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }
            else isBackPressed = false;
        }

        private void ProductNameEntryCompleted(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dateEntry.Text)) 
            {
                DependencyService.Get<IAudioService>().PlayAlertSound();
                dateEntry.Text = string.Empty;
                return;
            }
            if (nameEntry.Text.Length < 5)
            {
                DependencyService.Get<IAudioService>().PlayAlertSound();
            }
            else
            {
                quantityEntry.Focus();
            }
        }

        private void QuantityEntryCompleted(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(quantityEntry.Text))
            {
                DependencyService.Get<IAudioService>().PlayAlertSound();
            }
            else
            {
                dateEntry.Focus();
            }
        }

        private void DateEntryComleted(object sender, EventArgs e)
        {
            if(DateTime.TryParseExact(dateEntry.Text,
                    new[] { "dd.MM.yyyy", "dd.MM.yy", "ddMMyy" },
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                dateEntry.Text = parsedDate.ToString("dd.MM.yyyy");
            }
            else
            {
                DependencyService.Get<IAudioService>().PlayAlertSound();
            }
        }
    }
}
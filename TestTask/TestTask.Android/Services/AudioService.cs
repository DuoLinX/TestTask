using Android.Media;
using TestTask.Interface;
using TestTask.Droid;
using Xamarin.Forms;
using MyApp.Services;

[assembly: Dependency(typeof(AudioService))]
namespace MyApp.Services
{
    public class AudioService : IAudioService
    {
        private MediaPlayer _player;

        public void PlayAlertSound()
        {
            var context = Android.App.Application.Context;
            _player = MediaPlayer.Create(context, Resource.Raw.warning); 
            _player.Start();
        }
    }
}

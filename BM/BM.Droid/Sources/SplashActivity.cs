using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.OS;
using Android.Support.V7.App;
using System.Threading.Tasks;

namespace BM.Droid.Sources
{
    [Activity(Label = "@string/ApplicationName", Theme = "@style/ActivitySplash", MainLauncher = true, Icon = "@mipmap/ic_launcher", NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        // Simulates background work that happens behind the splash screen
        async void SimulateStartup()
        {
            await Task.Delay(1000);
            StartActivity(MainActivity.CreateStartIntent(this));
        }
    }
}
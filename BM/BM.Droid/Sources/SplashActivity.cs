using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.OS;
using Android.Support.V7.App;
using System.Threading.Tasks;
using Android.Content.Res;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

namespace BM.Droid.Sources
{
    [Activity(Label = "@string/ApplicationName", Theme = "@style/ActivitySplash", MainLauncher = true, Icon = "@mipmap/ic_launcher"/*, NoHistory = true*/)]
    public class SplashActivity : AppCompatActivity
    {
        private PreferencesHelper _recordsHelper;
        private bool _needStartApp = true;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _recordsHelper = new PreferencesHelper();
            _recordsHelper.ProcessFirstStarted(this);

            if (savedInstanceState != null)
            {
                _needStartApp = savedInstanceState.GetBoolean(nameof(_needStartApp));
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutBoolean(nameof(_needStartApp), _needStartApp);
        }

        protected override void OnResume()
        {
            base.OnResume();

            /*MobileCenter.Start("0f1c66c1-dc0c-4f49-96e0-2f4c017631d4",
                   typeof(Analytics), typeof(Crashes));*/ // DEBUG

            MobileCenter.Start("40d1e1c0-0450-4ef1-bda4-8d5f1365f069",
                   typeof(Analytics), typeof(Crashes)); // PLAY MARKET

            if (!_needStartApp)
            {
                return;
            }

            _needStartApp = false;

            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        // Simulates background work that happens behind the splash screen
        async void SimulateStartup()
        {
            await Task.Delay(500);
            StartActivity(MainActivity.CreateStartIntent(this));
        }
    }
}
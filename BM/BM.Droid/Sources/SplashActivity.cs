using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;

namespace BM.Droid.Sources
{
    [Activity(Label = "@string/ApplicationName", Theme = "@style/ActivitySplash", /*MainLauncher = true,*/ Icon = "@mipmap/ic_launcher", NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            StartActivity(MainActivity.CreateStartIntent(this));
        }
    }
}
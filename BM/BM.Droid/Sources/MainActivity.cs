using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using System;

namespace BM.Droid.Sources
{
    [Activity(Label = "@string/ApplicationName", Theme = "@style/AppTheme.Main", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        private Button _enterAnonimButton;
        private Button _enterButton;
        private EditText _name;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView (Resource.Layout.main);

            _name = FindViewById<EditText>(Resource.Id.name);
            _enterButton = FindViewById<Button>(Resource.Id.enterButton);
            _enterAnonimButton = FindViewById<Button>(Resource.Id.enterAnonimButton);

            _enterButton.Click += OnEnterButtonClicked;
            _enterAnonimButton.Click += OnEnterAnonimButtonClicked;
        }

        private void OnEnterAnonimButtonClicked(object sender, EventArgs e)
        {
            StartActivity(MainMenuActivity.CreateStartIntent(this, "Незнакомец"));
        }

        private void OnEnterButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_name.Text))
                return;

            StartActivity(MainMenuActivity.CreateStartIntent(this, _name.Text));
        }
    }
}


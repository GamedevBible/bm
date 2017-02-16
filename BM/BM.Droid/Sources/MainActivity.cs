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
        private Button _startButton;
        private Button _recordsButton;
        private Button _guideButton;
        private Button _contactsButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView (Resource.Layout.main);

            _startButton = FindViewById<Button>(Resource.Id.startButton);
            _recordsButton = FindViewById<Button>(Resource.Id.recordsButton);
            _guideButton = FindViewById<Button>(Resource.Id.guideButton);
            _contactsButton = FindViewById<Button>(Resource.Id.contactsButton);

            _startButton.Click += OnButtonClicked;
            _recordsButton.Click += OnButtonClicked;
            _guideButton.Click += OnButtonClicked;
            _contactsButton.Click += OnButtonClicked;
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            var clickedButton = (Button)sender;

            switch (clickedButton.Id)
            {
                case Resource.Id.startButton:
                    Toast.MakeText(this, "Нажата кнопка НАЧАТЬ ИГРУ", ToastLength.Short).Show();
                    StartActivity(GameActivity.CreateStartIntent(this));
                    break;
                case Resource.Id.recordsButton:
                    Toast.MakeText(this, "Нажата кнопка МОИ РЕКОРДЫ", ToastLength.Short).Show();
                    break;
                case Resource.Id.guideButton:
                    Toast.MakeText(this, "Нажата кнопка СПРАВКА", ToastLength.Short).Show();
                    break;
                case Resource.Id.contactsButton:
                    Toast.MakeText(this, "Нажата кнопка КОНТАКТЫ", ToastLength.Short).Show();
                    break;
                default:
                    break;
            }
        }
    }
}


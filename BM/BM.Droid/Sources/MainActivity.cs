using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using System;
using Android.Content;
using Android.Runtime;

namespace BM.Droid.Sources
{
    [Activity(Label = "@string/ApplicationName", Theme = "@style/AppTheme.Main", MainLauncher = true,
        Icon = "@mipmap/ic_launcher")]
    public class MainActivity : AppCompatActivity
    {
        private const int _gameActivityCode = 11;
        private int _lastQuestion = -1;
        private bool _gotMillion = false;

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
                    StartActivityForResult(GameActivity.CreateStartIntent(this), _gameActivityCode);
                    break;
                case Resource.Id.recordsButton:
                    break;
                case Resource.Id.guideButton:
                    break;
                case Resource.Id.contactsButton:
                    Toast.MakeText(this, "Нажата кнопка КОНТАКТЫ", ToastLength.Short).Show();
                    break;
                default:
                    break;
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (_lastQuestion != -1)
            {
                var ft = SupportFragmentManager.BeginTransaction();

                var prev = SupportFragmentManager.FindFragmentByTag(nameof(GameInformationFragment));
                if (prev != null)
                {
                    ft.Remove(prev);
                }
                ft.AddToBackStack(null);

                var dialogCallFriend = GameInformationFragment.NewInstance(_lastQuestion, _gotMillion);
                dialogCallFriend.Cancelable = false;
                dialogCallFriend.Show(ft, nameof(GameInformationFragment));

                _lastQuestion = -1;
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                if (requestCode == _gameActivityCode)
                {
                    _lastQuestion = data.GetIntExtra("lastQuestion", -1);
                    _gotMillion = data.GetBooleanExtra("gotMillion", false);
                }
            }
        }
    }
}


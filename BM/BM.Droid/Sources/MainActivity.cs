﻿using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using System;
using Android.Content;
using Android.Runtime;
using Android.Content.PM;
using Android.Media;
using System.Threading.Tasks;

namespace BM.Droid.Sources
{
    [Activity(Label = "@string/ApplicationName", Theme = "@style/AppTheme.Main", MainLauncher = true,
        Icon = "@mipmap/ic_launcher")]
    public class MainActivity : AppCompatActivity
    {
        private const int _gameActivityCode = 11;
        private const int _contactsActivityCode = 14;
        private int _lastQuestion = -1;
        private bool _gotMillion = false;
        private bool _gameWasLose;
        private MediaPlayer _millionPlayer;

        private Button _startButton;
        private Button _recordsButton;
        private Button _guideButton;
        private Button _contactsButton;
        private ImageButton _soundButton;
        private PreferencesHelper _recordsHelper;
        private bool _soundEnabled = true;
        private string _bibleTextForAnswer;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView (Resource.Layout.main);

            _startButton = FindViewById<Button>(Resource.Id.startButton);
            _recordsButton = FindViewById<Button>(Resource.Id.recordsButton);
            _guideButton = FindViewById<Button>(Resource.Id.guideButton);
            _contactsButton = FindViewById<Button>(Resource.Id.contactsButton);
            _soundButton = FindViewById<ImageButton>(Resource.Id.soundButton);

            _recordsHelper = new PreferencesHelper();
            _recordsHelper.InitHeplerForSound(this);
            _soundEnabled = _recordsHelper.GetSoundEnabled();

            RefreshSoundButton();

            _startButton.Click += OnButtonClicked;
            _recordsButton.Click += OnButtonClicked;
            _guideButton.Click += OnButtonClicked;
            _contactsButton.Click += OnButtonClicked;
            _soundButton.Click += OnSoundButtonClicked;

            _millionPlayer = MediaPlayer.Create(this, Resource.Raw.million);
        }

        private void OnSoundButtonClicked(object sender, EventArgs e)
        {
            if (_recordsHelper == null)
            {
                _recordsHelper = new PreferencesHelper();
                _recordsHelper.InitHeplerForSound(this);
                _soundEnabled = _recordsHelper.GetSoundEnabled();
            }

            _soundEnabled = !_soundEnabled;
            _recordsHelper.SetSoundEnabled(_soundEnabled);
            
            RefreshSoundButton();
        }

        private void RefreshSoundButton()
        {
            if (_soundEnabled)
                _soundButton.SetImageResource(Resource.Drawable.volume_high);
            else
                _soundButton.SetImageResource(Resource.Drawable.volume_off);
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
                    StartActivity(RecordsActivity.CreateStartIntent(this));
                    break;
                case Resource.Id.guideButton:
                    Toast.MakeText(this, "Экран справки об игре еще в разработке", ToastLength.Short).Show();
                    break;
                case Resource.Id.contactsButton:
                    StartActivityForResult(ContactsActivity.CreateStartIntent(this), _contactsActivityCode);
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

                if (_recordsHelper == null)
                {
                    _recordsHelper = new PreferencesHelper();
                    _recordsHelper.InitHeplerForSound(this);
                    _soundEnabled = _recordsHelper.GetSoundEnabled();
                }

                if (_gotMillion && _soundEnabled)
                    PlayMillion(_millionPlayer);

                var dialogCallFriend = GameInformationFragment.NewInstance(_lastQuestion, _gameWasLose, _gotMillion, _bibleTextForAnswer);
                dialogCallFriend.Cancelable = false;
                dialogCallFriend.Show(ft, nameof(GameInformationFragment));

                _lastQuestion = -1;
            }
        }

        private void PlayMillion(MediaPlayer mediaPlayer)
        {
            Task.Factory.StartNew(() =>
            {
                mediaPlayer = mediaPlayer ?? MediaPlayer.Create(this, Resource.Raw.million);
                mediaPlayer.Start();
            });
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
                    _gameWasLose = data.GetBooleanExtra("needFinishActivity", true);
                    _bibleTextForAnswer = data.GetStringExtra("bibleTextForAnswer");
                }
            }
        }
    }
}


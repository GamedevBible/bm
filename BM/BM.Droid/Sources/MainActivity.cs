using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using System;
using Android.Content;
using Android.Runtime;
using Android.Media;
using System.Threading.Tasks;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using Java.Util;

namespace BM.Droid.Sources
{
    [Activity(Label = "@string/ApplicationName", Theme = "@style/AppTheme.Main",
        Icon = "@mipmap/ic_launcher")]
    public class MainActivity : AppCompatActivity
    {
        private const int _gameActivityCode = 11;
        private const int _contactsActivityCode = 14;
        private int _lastQuestion = -1;
        private bool _gotMillion;
        private bool _withoutHelp;
        private bool _gameWasLose;
        private MediaPlayer _millionPlayer;
        private bool _inactive;

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

            MobileCenter.Start("0f1c66c1-dc0c-4f49-96e0-2f4c017631d4",
                   typeof(Analytics), typeof(Crashes));

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

            if (_recordsHelper.ProcessFirstStarted(this) == 0)
            {
                ShowGreetingsAlert();
            }
        }

        private void ShowGreetingsAlert()
        {
            var locale = Locale.Default.DisplayLanguage;

            var title = locale == "en" || locale == "en-US" || locale == "English" ? "Welcome!" : "Добро пожаловать!";

            var message = locale == "en" || locale == "en-US" || locale == "English"
                ? "Dear friend! This time our app supports only russian language. We are sorry for that." 
                : "Дорогой друг! Мы очень рады, что ты присоединился к нашему приложению! Тебя ждут более 3000 вопросов! Желаем тебе успехов и хорошего настроения!";

            var closeButton = locale == "en" || locale == "en-US" || locale == "English" ? "Close" : "Закрыть";

            var dialog = new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.AlertDialogTheme)
                    .SetTitle(title)
                    .SetMessage(message)
                    .SetPositiveButton(closeButton, CloseDialog)
                    .SetCancelable(false)
                    .Create();

            dialog.Show();
        }

        private void CloseDialog(object sender, DialogClickEventArgs e)
        {
            ((Android.Support.V7.App.AlertDialog)sender).Dismiss();
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
            if (_inactive)
                return;
            _inactive = true;

            var clickedButton = (Button)sender;

            switch (clickedButton.Id)
            {
                case Resource.Id.startButton:
                    StartActivityForResult(GameActivity.CreateStartIntent(this), _gameActivityCode);
                    break;
                case Resource.Id.recordsButton:
                    StartActivity(RecordsActivity.CreateStartIntent(this));
                    _inactive = false;
                    break;
                case Resource.Id.guideButton:
                    Toast.MakeText(this, "Экран справки об игре еще в разработке", ToastLength.Short).Show();
                    _inactive = false;
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
                _inactive = false;

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

                var dialogCallFriend = GameInformationFragment.NewInstance(_lastQuestion, _gameWasLose, _withoutHelp, gotMillion : _gotMillion, bibleTextForAnswer : _bibleTextForAnswer);
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

            _inactive = false;

            if (resultCode == Result.Ok)
            {
                if (requestCode == _gameActivityCode)
                {
                    _lastQuestion = data.GetIntExtra("lastQuestion", -1);
                    _gotMillion = data.GetBooleanExtra("gotMillion", false);
                    _gameWasLose = data.GetBooleanExtra("needFinishActivity", true);
                    _withoutHelp = data.GetBooleanExtra("withoutHelp", false);
                    _bibleTextForAnswer = data.GetStringExtra("bibleTextForAnswer");
                }
            }
        }

        public static Intent CreateStartIntent(Context context, string message = null)
        {
            var intent = new Intent(context, typeof(MainActivity));

            return intent;
        }
    }
}


using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using System;
using Android.Content;
using Android.Runtime;
using Android.Media;
using System.Threading.Tasks;
using Java.Util;
using Android.Content.PM;

/*Что сделать перед выкатыванием новой версии:
 - Повысить версию
 - Обновить Alert Whats New
 - Посмотреть есть ли новые истории
 - Посмотреть, стоит ли обновить благодарности
 - Проверить, включена ли аналитика релиза в SplashActivity
 - */

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
        private bool _greetingsWasShowed;
        private bool _needShowWhatsNew;

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

            if (bundle != null)
            {
                _greetingsWasShowed = bundle.GetBoolean(nameof(_greetingsWasShowed));
            }

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

            //_needShowWhatsNew = true;

            if (_recordsHelper.GetEntersCount() == 1 && !_greetingsWasShowed)
            {
                ShowGreetingsAlert();
                _greetingsWasShowed = true;
                _recordsHelper.PutLastVersion(this, PackageManager.GetPackageInfo(PackageName, PackageInfoFlags.Configurations).VersionName);
            }
            else
            if (!_recordsHelper.GetLastVersion().Equals(PackageManager.GetPackageInfo(PackageName, PackageInfoFlags.Configurations).VersionName))
            {
                if (_needShowWhatsNew)
                    ShowWhatsNewAlert();
                _recordsHelper.PutLastVersion(this, PackageManager.GetPackageInfo(PackageName, PackageInfoFlags.Configurations).VersionName);
                CopyDatabase("");
            }
        }

        private void CopyDatabase(string dataBaseName)
        {
            dataBaseName = "millionaire.db";
            var dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/" + dataBaseName;

            if (System.IO.File.Exists(dbPath))
                System.IO.File.Delete(dbPath);

            if (!System.IO.File.Exists(dbPath))
            {
                var dbAssetStream = Assets.Open(dataBaseName);
                var dbFileStream = new System.IO.FileStream(dbPath, System.IO.FileMode.OpenOrCreate);
                var buffer = new byte[1024];

                int b = buffer.Length;
                int length;

                while ((length = dbAssetStream.Read(buffer, 0, b)) > 0)
                {
                    dbFileStream.Write(buffer, 0, length);
                }

                dbFileStream.Flush();
                dbFileStream.Close();
                dbAssetStream.Close();
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutBoolean(nameof(_greetingsWasShowed), true);
        }

        private void ShowWhatsNewAlert()
        {
            var dialog = new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.AlertDialogTheme)
                    .SetTitle($"Версия {PackageManager.GetPackageInfo(PackageName, PackageInfoFlags.Configurations).VersionName}")
                    .SetMessage("Что нового:" + "\n" + "- исправлены найденные ошибки в вопросах." + "\n" + "\n" + "Мы очень рады, что вы участвуете в нашей викторине! А мы будем делать наше приложение все более интересным для вас!")
                    .SetPositiveButton("Закрыть", CloseDialog)
                    .SetCancelable(false)
                    .Create();

            dialog.Show();
        }

        private void ShowGreetingsAlert()
        {
            var locale = Locale.Default.DisplayLanguage;

            var title = locale == "en" || locale == "en-US" || locale == "English" ? "Welcome!" : "Добро пожаловать!";

            var message = locale == "en" || locale == "en-US" || locale == "English"
                ? "Dear friend! This time our app supports only russian language. We are sorry for that." 
                : "Дорогой друг! Мы очень рады, что ты присоединился к нашему приложению! Тебя ждут более 3000 вопросов! Желаем тебе успехов!";

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
            mediaPlayer = mediaPlayer ?? MediaPlayer.Create(this, Resource.Raw.million);
            Task.Factory.StartNew(() =>
            {
                mediaPlayer.Completion += (sender, args) => {
                    if (mediaPlayer != null && !mediaPlayer.IsPlaying)
                    {
                        mediaPlayer.Release();
                        mediaPlayer = null;
                    }
                };
                mediaPlayer?.Start();
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
            intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
            return intent;
        }
    }
}


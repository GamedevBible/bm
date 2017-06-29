using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Content.PM;
using Android.Text;
using System.Linq;

namespace BM.Droid.Sources
{
    [Activity(Label = "ContactsActivity", Theme = "@style/AppTheme.Main",
        Icon = "@mipmap/ic_launcher")]
    public class ContactsActivity : AppCompatActivity
    {
        private TextView _appVersion;
        private TextView _contactUs;
        private TextView _supportUs;
        private const int _emailRequestCode = 11234;
        private ImageButton _historiesButton;
        private bool _historiesButtonEnabled;
        private ImageButton _thanksButton;
        private bool _inactive;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.contacts);
            
            _appVersion = FindViewById<TextView>(Resource.Id.appVersion);
            _contactUs = FindViewById<TextView>(Resource.Id.contactUs);
            _supportUs = FindViewById<TextView>(Resource.Id.supportUs);
            _historiesButton = FindViewById<ImageButton>(Resource.Id.historiesButton);
            _thanksButton = FindViewById<ImageButton>(Resource.Id.thanksButton);

            var recordsHelper = new PreferencesHelper();
            recordsHelper.InitHelperForRecords(this);

            var records = recordsHelper.GetRecords();
            records.OrderByDescending(t => t.QuestionNumber);

            _historiesButtonEnabled = ValuesConverter.QuestionNumberToPoints(records[0].QuestionNumber).Equals("1 000 000")
                && ValuesConverter.QuestionNumberToPoints(records[1].QuestionNumber).Equals("1 000 000")
                && ValuesConverter.QuestionNumberToPoints(records[2].QuestionNumber).Equals("1 000 000");

            if (_historiesButtonEnabled)
                _historiesButton.SetImageResource(Resource.Drawable.book_open_page_variant);
            else
                _historiesButton.SetImageResource(Resource.Drawable.book_open_page_variant_disabled);

            _historiesButton.Click += OnHistoriesButtonClicked;
            _thanksButton.Click += OnThanksButtonClicked;

            //if Landscape
            if (WindowManager.DefaultDisplay.Rotation == SurfaceOrientation.Rotation90 || WindowManager.DefaultDisplay.Rotation == SurfaceOrientation.Rotation270)
            {
                string text = "<font>У вас есть вопросы или предложения? </font>" +
                "<font>Напишите нам: </font><font color=#03a9f4>biblegamedev@gmail.com</font>";

                _contactUs.SetText(Html.FromHtml(text), TextView.BufferType.Spannable);
            }
            else
            {
                string text = "<font>У вас есть вопросы или предложения?</font><br>" +
                "<font>Напишите нам: </font><font color=#03a9f4>biblegamedev@gmail.com</font>";

                _contactUs.SetText(Html.FromHtml(text), TextView.BufferType.Spannable);
            }

            _appVersion.Text = 
                $"Версия приложения {PackageManager.GetPackageInfo(PackageName, PackageInfoFlags.Configurations).VersionName}";

            string supportText = "</font><font color=#03a9f4>- Поддержать нас -</font>";

            _supportUs.SetText(Html.FromHtml(supportText), TextView.BufferType.Spannable);

            _appVersion.Click += OnAppVersionClicked;
            _contactUs.Click += OnContactUsClicked;
            _contactUs.LongClick += OnContactUsLongClicked;
            _supportUs.Click += OnSupportUsClicked;
        }

        private void OnAppVersionClicked(object sender, EventArgs e)
        {
            if (_inactive)
                return;
            _inactive = true;

            var dialog = new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.AlertDialogTheme)
                    .SetTitle($"Версия {PackageManager.GetPackageInfo(PackageName, PackageInfoFlags.Configurations).VersionName}")
                    .SetMessage("Приложение добавлено в Google Play. Мы подготовили и оформили для вас более трех тысяч вопросов. " +
                                "Еще мы создали в приложении раздел с историями.")
                    .SetPositiveButton("OK", AlertConfirmButtonClicked)
                    .SetCancelable(false)
                    .Create();

            dialog.Show();
        }

        private void OnSupportUsClicked(object sender, EventArgs e)
        {
            if (_inactive)
                return;
            _inactive = true;

            var dialog = new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.AlertDialogTheme)
                    .SetTitle("Поддержка BibleGameDev")
                    .SetMessage("Поддержать нас можно переводом на карту: 1234567890")
                    .SetPositiveButton("OK", AlertConfirmButtonClicked)
                    .SetCancelable(false)
                    .Create();

            dialog.Show();
        }

        private void OnThanksButtonClicked(object sender, EventArgs e)
        {
            var ft = SupportFragmentManager.BeginTransaction();
            var prev = SupportFragmentManager.FindFragmentByTag(nameof(ThanksFragment));
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);

            var dialog = ThanksFragment.NewInstance();
            dialog.Show(ft, nameof(ThanksFragment));
        }

        private void OnHistoriesButtonClicked(object sender, EventArgs e)
        {
            if (_historiesButtonEnabled)
            {
                var dialog = new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.AlertDialogTheme)
                    .SetTitle("Закрыто")
                    .SetMessage("Сейчас вы не можете читать истории. Выиграйте игру несколько раз, и экран историй станет доступным.")
                    .SetPositiveButton("OK", AlertConfirmButtonClicked)
                    .SetCancelable(false)
                    .Create();

                dialog.Show();
            }
            else
            {
                StartActivity(HistoriesActivity.CreateStartIntent(this));
            }
        }

        private void AlertConfirmButtonClicked(object sender, DialogClickEventArgs e)
        {
            _inactive = false;
            ((Android.Support.V7.App.AlertDialog)sender).Dismiss();
        }

        private void OnContactUsLongClicked(object sender, View.LongClickEventArgs e)
        {
            Android.Content.ClipboardManager clipboard = (Android.Content.ClipboardManager)GetSystemService(ClipboardService);
            ClipData clip = ClipData.NewPlainText("label", "biblegamedev@gmail.com");
            clipboard.PrimaryClip = clip;

            Toast.MakeText(this, "Email скопирован", ToastLength.Short).Show();
        }

        private void OnContactUsClicked(object sender, EventArgs e)
        {
            var emailIntent = new Intent(Intent.ActionSend);
            // The intent does not have a URI, so declare the "text/plain" MIME type
            emailIntent.SetType("vnd.android.cursor.dir/email");
            emailIntent.PutExtra(Intent.ExtraEmail, new[] { "biblegamedev@gmail.com" }); // recipients
            emailIntent.PutExtra(Intent.ExtraSubject, "Message from BM game");
            emailIntent.PutExtra(Intent.ExtraText, string.Empty);

            try
            {
                StartActivityForResult(emailIntent, _emailRequestCode);
            }
            catch (Exception ex)
            {
                Android.Content.ClipboardManager clipboard = (Android.Content.ClipboardManager)GetSystemService(ClipboardService);
                ClipData clip = ClipData.NewPlainText("label", "biblegamedev@gmail.com");
                clipboard.PrimaryClip = clip;

                Toast.MakeText(this, "Email скопирован", ToastLength.Short).Show();
            }
            
        }

        public static Intent CreateStartIntent(Context context, string message = null)
        {
            var intent = new Intent(context, typeof(ContactsActivity));

            return intent;
        }
    }
}
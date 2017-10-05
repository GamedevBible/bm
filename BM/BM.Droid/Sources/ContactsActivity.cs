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
using Microsoft.Azure.Mobile.Analytics;

namespace BM.Droid.Sources
{
    [Activity(Label = "ContactsActivity", Theme = "@style/AppTheme.Main",
        Icon = "@mipmap/ic_launcher")]
    public class ContactsActivity : AppCompatActivity
    {
        private TextView _version;
        private TextView _lessons;
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

            _version = FindViewById<TextView>(Resource.Id.appVersion);
            _lessons = FindViewById<TextView>(Resource.Id.lessons);
            _contactUs = FindViewById<TextView>(Resource.Id.contactUs);
            _supportUs = FindViewById<TextView>(Resource.Id.supportUs);
            _historiesButton = FindViewById<ImageButton>(Resource.Id.historiesButton);
            _thanksButton = FindViewById<ImageButton>(Resource.Id.thanksButton);

            var recordsHelper = new PreferencesHelper();
            recordsHelper.InitHelperForRecords(this);

            var records = recordsHelper.GetRecords();
            records.OrderByDescending(t => t.QuestionNumber);

            _version.Text = $"Версия приложения {PackageManager.GetPackageInfo(PackageName, PackageInfoFlags.Configurations).VersionName}";

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

            _lessons.Text = 
                $"Изучение Библии";

            string supportText = "</font><font color=#03a9f4>- Поддержать нас -</font>";

            _supportUs.SetText(Html.FromHtml(supportText), TextView.BufferType.Spannable);

            _lessons.Click += OnAppVersionClicked;
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
                    .SetTitle($"Изучение Библии")
                    .SetMessage("У вас есть вопросы по Библии? Вы всегда можете задать их нам на Email! "+
                    "Также вы можете изучать Библию самостоятельно при помощи уроков по изучению! Хороший сайт с уроками имеется по адресу http://apologetica.ru.")
                    .SetNegativeButton("Перейти на сайт", GoToLessonsSite)
                    .SetPositiveButton("Закрыть", AlertConfirmButtonClicked)
                    .SetCancelable(false)
                    .Create();

            dialog.Show();
        }

        private void GoToLessonsSite(object sender, DialogClickEventArgs e)
        {
            _inactive = false;

            Analytics.TrackEvent("User go to Bible lessons site");

            var uri = Android.Net.Uri.Parse("http://apologetica.ru/uroki/uroki-po-izucheniyu-biblii.html");
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }

        private void OnSupportUsClicked(object sender, EventArgs e)
        {
            if (_inactive)
                return;
            _inactive = true;

            var dialog = new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.AlertDialogTheme)
                    .SetTitle("Поддержать нас")
                    .SetMessage("Дорогие друзья! Вы всегда можете поддержать нас: поставив нашему приложению 5 звезд в магазине Play Market, "+
                    "молясь за пробуждение у людей интереса к изучению Библии, рассказывая друзьям и знакомым об этом приложении, "+
                    "помогая нам добровольными пожертвованиями. Спасибо вам за вашу поддержку!")
                    .SetNegativeButton("О пожертвованиях", OnDonateUsClicked)
                    .SetPositiveButton("Закрыть", AlertConfirmButtonClicked)
                    .SetCancelable(false)
                    .Create();

            dialog.Show();
        }

        private void OnDonateUsClicked(object sender, EventArgs e)
        {
            _inactive = false;
            ((Android.Support.V7.App.AlertDialog)sender).Dismiss();

            if (_inactive)
                return;
            _inactive = true;

            var dialog = new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.AlertDialogTheme)
                    .SetTitle("Поддержка BibleGameDev")
                    .SetMessage("Вся оказываемая поддержка проектов BibleGameDev является добровольной и безвозмездной."+
                    " Поддержать нас своими пожертвованиями можно переводом на счет Яндекс.Деньги: 410015425804928."+
                    " По желанию, в сообщении к пожертвованию вы можете указать свое имя или инициалы."+
                    " Благодарим вас за оказанную помощь! С вашей помощью наши новые проекты будут более качественными!"+
                    " Вы помогаете пробудить у людей интерес к изучению Библии!")
                    .SetNegativeButton("Скопировать номер счета", YandexMoneyCopied)
                    .SetPositiveButton("Закрыть", AlertConfirmButtonClicked)
                    .SetCancelable(false)
                    .Create();

            Analytics.TrackEvent("User open donate alert");

            dialog.Show();
        }

        private void YandexMoneyCopied(object sender, DialogClickEventArgs e)
        {
            Android.Content.ClipboardManager clipboard = (Android.Content.ClipboardManager)GetSystemService(ClipboardService);
            ClipData clip = ClipData.NewPlainText("label", "410015425804928");
            clipboard.PrimaryClip = clip;

            Toast.MakeText(this, "Номер счета скопирован", ToastLength.Short).Show();
            _inactive = false;
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
            dialog.Cancelable = false;
            dialog.Show(ft, nameof(ThanksFragment));
        }

        private void OnHistoriesButtonClicked(object sender, EventArgs e)
        {
            if (_inactive)
                return;
            _inactive = true;

            if (!_historiesButtonEnabled)
            {
                var dialog = new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.AlertDialogTheme)
                    .SetTitle("Закрыто")
                    .SetMessage("Сейчас вы не можете читать истории. Выиграйте игру несколько раз, и экран историй станет доступным.")
                    .SetPositiveButton("Закрыть", AlertConfirmButtonClicked)
                    .SetCancelable(false)
                    .Create();

                dialog.Show();
            }
            else
            {
                StartActivity(HistoriesActivity.CreateStartIntent(this));
                _inactive = false;
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
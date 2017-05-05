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
using Android.Text.Method;
using Android.Content.PM;
using Android.Text;

namespace BM.Droid.Sources
{
    [Activity(Label = "ContactsActivity", Theme = "@style/AppTheme.Main",
        Icon = "@mipmap/ic_launcher")]
    public class ContactsActivity : AppCompatActivity
    {
        private TextView _info;
        private TextView _appVersion;
        private TextView _contactUs;
        private const int _emailRequestCode = 11234;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.contacts);

            _info = FindViewById<TextView>(Resource.Id.info);
            _info.MovementMethod = new ScrollingMovementMethod();
            _appVersion = FindViewById<TextView>(Resource.Id.appVersion);
            _contactUs = FindViewById<TextView>(Resource.Id.contactUs);

            //if Landscape
            if (WindowManager.DefaultDisplay.Rotation == SurfaceOrientation.Rotation90 || WindowManager.DefaultDisplay.Rotation == SurfaceOrientation.Rotation270)
            {
                _info.Text = "Дизайн и разработка: С. Ларионов (мл.)" + "\n" +
                    "Вопросы для игры: С. Ларионов (ст.)";

                string text = "<font>У вас есть вопросы или предложения? </font>" +
                "<font>Желаете поддержать этот проект и наши дальнейшие проекты?</font><br>" +
                "<font>Напишите нам: </font><font color=#03a9f4>biblegamedev@gmail.com</font>";

                _contactUs.SetText(Html.FromHtml(text), TextView.BufferType.Spannable);
            }
            else
            {
                _info.Text = "Дизайн и разработка:" + "\n" +
                    "С. Ларионов (мл.)" + "\n" +
                    "Вопросы для игры:" + "\n" +
                    "С. Ларионов (ст.)";

                string text = "<font>У вас есть вопросы или предложения?</font><br>" +
                "<font>Желаете поддержать этот проект и наши дальнейшие проекты?</font><br>" +
                "<font>Напишите нам: </font><font color=#03a9f4>biblegamedev@gmail.com</font>";

                _contactUs.SetText(Html.FromHtml(text), TextView.BufferType.Spannable);
            }

            _appVersion.Text = 
                $"Версия приложения {PackageManager.GetPackageInfo(PackageName, PackageInfoFlags.Configurations).VersionName}";

            _contactUs.Click += OnContactUsClicked;
            _contactUs.LongClick += OnContactUsLongClicked;
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
            emailIntent.PutExtra(Intent.ExtraSubject, "Message from game");
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
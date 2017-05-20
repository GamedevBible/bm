using System;

using Android.App;
using Android.Content;
using Android.OS;
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
        private ImageButton _historiesButton;
        private bool _historiesButtonEnabled;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.contacts);

            _info = FindViewById<TextView>(Resource.Id.info);
            _info.MovementMethod = new ScrollingMovementMethod();
            _appVersion = FindViewById<TextView>(Resource.Id.appVersion);
            _contactUs = FindViewById<TextView>(Resource.Id.contactUs);
            _historiesButton = FindViewById<ImageButton>(Resource.Id.historiesButton);

            if (_historiesButtonEnabled)
                _historiesButton.SetImageResource(Resource.Drawable.book_open_page_variant);
            else
                _historiesButton.SetImageResource(Resource.Drawable.book_open_page_variant_disabled);

            //if Landscape
            if (WindowManager.DefaultDisplay.Rotation == SurfaceOrientation.Rotation90 || WindowManager.DefaultDisplay.Rotation == SurfaceOrientation.Rotation270)
            {
                _info.Text = "������ � ����������: �. �������� (��.)" + "\n" +
                    "������� ��� ����: �. �������� (��.)";

                string text = "<font>� ��� ���� ������� ��� �����������? </font>" +
                "<font>������� ���������� ���� ������ � ���� ���������� �������?</font><br>" +
                "<font>�������� ���: </font><font color=#03a9f4>biblegamedev@gmail.com</font>";

                _contactUs.SetText(Html.FromHtml(text), TextView.BufferType.Spannable);
            }
            else
            {
                _info.Text = "������ � ����������:" + "\n" +
                    "�. �������� (��.)" + "\n" +
                    "������� ��� ����:" + "\n" +
                    "�. �������� (��.)";

                string text = "<font>� ��� ���� ������� ��� �����������?</font><br>" +
                "<font>������� ���������� ��� ������?</font><br>" +
                "<font>�������� ���: </font><font color=#03a9f4>biblegamedev@gmail.com</font>";

                _contactUs.SetText(Html.FromHtml(text), TextView.BufferType.Spannable);
            }

            _appVersion.Text = 
                $"������ ���������� {PackageManager.GetPackageInfo(PackageName, PackageInfoFlags.Configurations).VersionName}";

            _contactUs.Click += OnContactUsClicked;
            _contactUs.LongClick += OnContactUsLongClicked;
        }

        private void OnContactUsLongClicked(object sender, View.LongClickEventArgs e)
        {
            Android.Content.ClipboardManager clipboard = (Android.Content.ClipboardManager)GetSystemService(ClipboardService);
            ClipData clip = ClipData.NewPlainText("label", "biblegamedev@gmail.com");
            clipboard.PrimaryClip = clip;

            Toast.MakeText(this, "Email ����������", ToastLength.Short).Show();
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

                Toast.MakeText(this, "Email ����������", ToastLength.Short).Show();
            }
            
        }

        public static Intent CreateStartIntent(Context context, string message = null)
        {
            var intent = new Intent(context, typeof(ContactsActivity));

            return intent;
        }
    }
}
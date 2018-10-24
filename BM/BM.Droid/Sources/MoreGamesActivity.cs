using Android.App;
using Android.Support.V7.App;
using Android.OS;
using Android.Content;
using Android.Widget;
using System;
using Microsoft.Azure.Mobile.Analytics;

namespace BM.Droid.Sources
{
    [Activity(Label = "MoreGamesActivity", Theme = "@style/AppTheme.Main",
        Icon = "@mipmap/ic_launcher")]
    public class MoreGamesActivity : AppCompatActivity
    {
        private TextView _fpowTextView;
        private TextView _fpowTitleTextView;
        private Button _fpowButton;

        private TextView _bwTextView;
        private TextView _bwTitleTextView;
        private Button _bwButton;

        private const string _fpowPackageName = "com.biblegamedev.fpow";
        private const string _bwPackageName = "com.biblegamedev.bw";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.more_games);

            _fpowTextView = FindViewById<TextView>(Resource.Id.fpowtext);
            _fpowTitleTextView = FindViewById<TextView>(Resource.Id.fpowtitletext);
            _fpowButton = FindViewById<Button>(Resource.Id.fpowButton);
            _bwTextView = FindViewById<TextView>(Resource.Id.bwtext);
            _bwTitleTextView = FindViewById<TextView>(Resource.Id.bwtitletext);
            _bwButton = FindViewById<Button>(Resource.Id.bwButton);
            _fpowButton.Click += OnFpowClicked;
            _bwButton.Click += OnBwClicked;

            _fpowTitleTextView.Text = "4 фото 1 слово - Библия";
            _fpowTextView.Text = "Игра, в которой вам предстоит отгадать, какое слово скрыто в представленных четырех фотографиях." + "\n" +
                "Попробуйте отгадать все загаданные слова!" + "\n" +
                "В игре также постепенно будут добавляться новые уровни!";

            _bwTitleTextView.Text = "Библейские слова";
            _bwTextView.Text = "Игра, в которой вам предстоит отгадать, какое слово из выбранной вами категории загадано." + "\n" +
                "У вас есть несколько попыток угадать слово, пока мост не разрушился!" + "\n" + "В игре можно загадывать слово другу и играть с ним на одном устройстве!" + "\n" +
                "В игре также постепенно будут добавляться новые уровни!";
        }

        private void OnBwClicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("User go to BW from BM");

            try
            {
                StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=" + _bwPackageName)));
            }
            catch (ActivityNotFoundException anfe)
            {
                StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("http://play.google.com/store/apps/details?id=" + _bwPackageName)));
            }
        }

        private void OnFpowClicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("User go to FPOW from BM");

            try
            {
                StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=" + _fpowPackageName)));
            }
            catch (ActivityNotFoundException anfe)
            {
                StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("http://play.google.com/store/apps/details?id=" + _fpowPackageName)));
            }
        }

        public static Intent CreateStartIntent(Context context, string message = null)
        {
            var intent = new Intent(context, typeof(MoreGamesActivity));

            return intent;
        }
    }
}
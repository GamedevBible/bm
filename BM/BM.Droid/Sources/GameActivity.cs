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
using Android.Support.V4.Content;

namespace BM.Droid.Sources
{
    [Activity(Label = "GameActivity", Theme = "@style/AppTheme.Main")]
    public class GameActivity : AppCompatActivity
    {
        bool _doubleBackToExitPressedOnce = false;
        private TextView _question;
        private ImageButton _backButton;
        private Button _pointsButton;
        private ImageButton _callButton;
        private ImageButton _peopleButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.game);
            
            //_question = FindViewById<TextView>(Resource.Id.question);
            //_question.MovementMethod = new ScrollingMovementMethod();

            _backButton = FindViewById<ImageButton>(Resource.Id.backButton);
            _callButton = FindViewById<ImageButton>(Resource.Id.callButton);
            _peopleButton = FindViewById<ImageButton>(Resource.Id.peopleButton);
            _pointsButton = FindViewById<Button>(Resource.Id.pointsButton);

            _backButton.Click += OnImageButtonClicked;
            _callButton.Click += OnImageButtonClicked;
            _peopleButton.Click += OnImageButtonClicked;
            _pointsButton.Click += OnButtonClicked;

            if (savedInstanceState != null)
            {
                _callButton.Enabled = savedInstanceState.GetBoolean(nameof(_callButton));
                _callButton.SetColorFilter(new Android.Graphics.Color(ContextCompat.GetColor(this, _callButton.Enabled ? Resource.Color.bm_white : Resource.Color.lighter_gray)));
                _peopleButton.Enabled = savedInstanceState.GetBoolean(nameof(_peopleButton));
                _peopleButton.SetColorFilter(new Android.Graphics.Color(ContextCompat.GetColor(this, _peopleButton.Enabled ? Resource.Color.bm_white : Resource.Color.lighter_gray)));
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutBoolean(nameof(_callButton), _callButton.Enabled);
            outState.PutBoolean(nameof(_peopleButton), _peopleButton.Enabled);
        }

        private void OnImageButtonClicked(object sender, EventArgs e)
        {
            var buttonClicked = (ImageButton)sender;
            var ft = SupportFragmentManager.BeginTransaction();
            switch (buttonClicked.Id)
            {
                case Resource.Id.backButton:
                    Finish();
                    break;
                case Resource.Id.callButton:
                    _callButton.Enabled = false;
                    _callButton.SetColorFilter(new Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.lighter_gray)));
                    RemoveFragmentIfOpened(ft, nameof(CallFriendFragment));

                    var dialogCallFriend = CallFriendFragment.NewInstance(new Question
                    {
                        Level = 10,
                        Variant1 = "1939",
                        Variant2 = "1918",
                        Variant3 = "1941",
                        Variant4 = "1906",
                        Answer = 3
                    });
                    dialogCallFriend.Show(ft, nameof(CallFriendFragment));
                    break;
                case Resource.Id.peopleButton:
                    _peopleButton.Enabled = false;
                    _peopleButton.SetColorFilter(new Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.lighter_gray)));
                    RemoveFragmentIfOpened(ft, nameof(AuditoryHelpFragment));

                    var dialogPeopleHelp = AuditoryHelpFragment.NewInstance(new Question
                    {
                        Level = 10,
                        Variant1 = "1939",
                        Variant2 = "1918",
                        Variant3 = "1941",
                        Variant4 = "1906",
                        Answer = 3
                    });
                    dialogPeopleHelp.Show(ft, nameof(AuditoryHelpFragment));
                    break;
                default:
                    break;
            }
            }

        private void RemoveFragmentIfOpened(Android.Support.V4.App.FragmentTransaction ft, string fragmentName)
        {
            var prev = SupportFragmentManager.FindFragmentByTag(fragmentName);
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            var buttonClicked = (Button)sender;
            switch (buttonClicked.Id)
            {
                case Resource.Id.pointsButton:
                    var ft = SupportFragmentManager.BeginTransaction();
                    var prev = SupportFragmentManager.FindFragmentByTag(nameof(PointsFragment));
                    if (prev != null)
                    {
                        ft.Remove(prev);
                    }
                    ft.AddToBackStack(null);

                    /*********************** Потом нужно передавать реальный номер вопроса **********************************/
                    Random rand = new Random();
                    int temp;
                    temp = rand.Next(0, 15);
                    
                    var dialog = PointsFragment.NewInstance(temp);
                    dialog.Show(ft, nameof(PointsFragment));
                    break;
                default:
                    break;
            }
        }

        public override void OnBackPressed()
        {
            if (_doubleBackToExitPressedOnce)
            {
                base.OnBackPressed();
                Finish();
                return;
            }


            this._doubleBackToExitPressedOnce = true;
            Toast.MakeText(this, "Нажмите еще раз чтобы вернуться в меню", ToastLength.Short).Show();

            new Handler().PostDelayed(() =>
            {
                _doubleBackToExitPressedOnce = false;
            }, 2000);
        }

        public static Intent CreateStartIntent(Context context, string message = null)
        {
            var intent = new Intent(context, typeof(GameActivity));

            //intent.PutExtra(_actionArgumentTag, (int)accountAction);

            /*if (!string.IsNullOrWhiteSpace(message))
                intent.PutExtra(_messageArgTag, message);

            if (accountAction == AccountAction.Logout)
                intent = intent.AddFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);*/
            return intent;
        }
    }
}
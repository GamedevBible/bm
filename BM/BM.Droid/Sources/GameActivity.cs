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
using Android.Content.Res;

namespace BM.Droid.Sources
{
    [Activity(Label = "GameActivity", Theme = "@style/AppTheme.Main")]
    public class GameActivity : AppCompatActivity
    {
        bool _doubleBackToExitPressedOnce = false;
        private TextView _question;
        private Button _variant1Button;
        private Button _variant2Button;
        private Button _variant3Button;
        private Button _variant4Button;
        private FrameLayout _variant1Layout;
        private FrameLayout _variant2Layout;
        private FrameLayout _variant3Layout;
        private FrameLayout _variant4Layout;
        private ImageButton _backButton;
        private Button _pointsButton;
        private ImageButton _callButton;
        private ImageButton _peopleButton;
        private ImageButton _twoVariantsButton;
        private Question _currentQuestion;
        private bool _needEnableButtons;
        private QuestionsDatabase _questionsDatabase = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.game);

            _questionsDatabase = new QuestionsDatabase(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));

            var questions = _questionsDatabase.GetAllItems();

            _currentQuestion = new Question
            {
                Id = 1,
                QuestionText = "В каком году началась Великая Отечественная Война?",
                Level = 10,
                Variant1 = "1939",
                Variant2 = "1918",
                Variant3 = "1941",
                Variant4 = "1906",
                Answer = 3
            };

            _needEnableButtons = false;
            //_question = FindViewById<TextView>(Resource.Id.question);
            //_question.MovementMethod = new ScrollingMovementMethod();
            _question = FindViewById<TextView>(Resource.Id.question);
            _variant1Button = FindViewById<Button>(Resource.Id.variant1Button);
            _variant2Button = FindViewById<Button>(Resource.Id.variant2Button);
            _variant3Button = FindViewById<Button>(Resource.Id.variant3Button);
            _variant4Button = FindViewById<Button>(Resource.Id.variant4Button);
            _backButton = FindViewById<ImageButton>(Resource.Id.backButton);
            _callButton = FindViewById<ImageButton>(Resource.Id.callButton);
            _peopleButton = FindViewById<ImageButton>(Resource.Id.peopleButton);
            _twoVariantsButton = FindViewById<ImageButton>(Resource.Id.fiftyButton);
            _pointsButton = FindViewById<Button>(Resource.Id.pointsButton);
            _variant1Layout = FindViewById<FrameLayout>(Resource.Id.variant1Layout);
            _variant2Layout = FindViewById<FrameLayout>(Resource.Id.variant2Layout);
            _variant3Layout = FindViewById<FrameLayout>(Resource.Id.variant3Layout);
            _variant4Layout = FindViewById<FrameLayout>(Resource.Id.variant4Layout);

            _variant1Layout.Click += OnAnswerButtonClick;
            _variant2Layout.Click += OnAnswerButtonClick;
            _variant3Layout.Click += OnAnswerButtonClick;
            _variant4Layout.Click += OnAnswerButtonClick;
            _backButton.Click += OnImageButtonClicked;
            _callButton.Click += OnImageButtonClicked;
            _peopleButton.Click += OnImageButtonClicked;
            _twoVariantsButton.Click += OnImageButtonClicked;
            _pointsButton.Click += OnButtonClicked;

            InstallCurrentQuestion(_currentQuestion);

            if (savedInstanceState != null)
            {
                _callButton.Enabled = savedInstanceState.GetBoolean(nameof(_callButton));
                _callButton.SetColorFilter(new Android.Graphics.Color(ContextCompat.GetColor(this, _callButton.Enabled ? Resource.Color.bm_white : Resource.Color.lighter_gray)));
                _peopleButton.Enabled = savedInstanceState.GetBoolean(nameof(_peopleButton));
                _peopleButton.SetColorFilter(new Android.Graphics.Color(ContextCompat.GetColor(this, _peopleButton.Enabled ? Resource.Color.bm_white : Resource.Color.lighter_gray)));
                _twoVariantsButton.Enabled = savedInstanceState.GetBoolean(nameof(_twoVariantsButton));
                _twoVariantsButton.SetColorFilter(new Android.Graphics.Color(ContextCompat.GetColor(this, _twoVariantsButton.Enabled ? Resource.Color.bm_white : Resource.Color.lighter_gray)));
                _needEnableButtons = savedInstanceState.GetBoolean(nameof(_needEnableButtons));

                if (_needEnableButtons)
                {
                    _variant1Layout.Enabled = savedInstanceState.GetBoolean(nameof(_variant1Layout));
                    _variant2Layout.Enabled = savedInstanceState.GetBoolean(nameof(_variant2Layout));
                    _variant3Layout.Enabled = savedInstanceState.GetBoolean(nameof(_variant3Layout));
                    _variant4Layout.Enabled = savedInstanceState.GetBoolean(nameof(_variant4Layout));

                    _variant1Button.SetBackgroundResource(_variant1Layout.Enabled ? Resource.Drawable.button_background : Resource.Drawable.button_disabled);
                    _variant2Button.SetBackgroundResource(_variant2Layout.Enabled ? Resource.Drawable.button_background : Resource.Drawable.button_disabled);
                    _variant3Button.SetBackgroundResource(_variant3Layout.Enabled ? Resource.Drawable.button_background : Resource.Drawable.button_disabled);
                    _variant4Button.SetBackgroundResource(_variant4Layout.Enabled ? Resource.Drawable.button_background : Resource.Drawable.button_disabled);
                }
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutBoolean(nameof(_callButton), _callButton.Enabled);
            outState.PutBoolean(nameof(_peopleButton), _peopleButton.Enabled);
            outState.PutBoolean(nameof(_twoVariantsButton), _twoVariantsButton.Enabled);
            outState.PutBoolean(nameof(_needEnableButtons), _needEnableButtons);

            if (_needEnableButtons)
            {
                outState.PutBoolean(nameof(_variant1Layout), _variant1Layout.Enabled);
                outState.PutBoolean(nameof(_variant2Layout), _variant2Layout.Enabled);
                outState.PutBoolean(nameof(_variant3Layout), _variant3Layout.Enabled);
                outState.PutBoolean(nameof(_variant4Layout), _variant4Layout.Enabled);
            }
        }

        private void InstallCurrentQuestion(Question currentQuestion)
        {
            _question.Text = currentQuestion.QuestionText;
            _variant1Button.Text = currentQuestion.Variant1;
            _variant2Button.Text = currentQuestion.Variant2;
            _variant3Button.Text = currentQuestion.Variant3;
            _variant4Button.Text = currentQuestion.Variant4;
        }

        private void OnAnswerButtonClick(object sender, EventArgs e)
        {
            _currentQuestion = new Question
            {
                Id = 2,
                QuestionText = "Сколько дней в високосном году?",
                Level = 10,
                Variant1 = "366",
                Variant2 = "29",
                Variant3 = "365",
                Variant4 = "28",
                Answer = 1
            };

            if (_needEnableButtons)
            {
                _variant1Layout.Enabled = true;
                _variant2Layout.Enabled = true;
                _variant3Layout.Enabled = true;
                _variant4Layout.Enabled = true;
                _variant1Button.SetBackgroundResource(Resource.Drawable.button_background);
                _variant2Button.SetBackgroundResource(Resource.Drawable.button_background);
                _variant3Button.SetBackgroundResource(Resource.Drawable.button_background);
                _variant4Button.SetBackgroundResource(Resource.Drawable.button_background);
                _needEnableButtons = false;
            }

            InstallCurrentQuestion(_currentQuestion);
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

                    var dialogCallFriend = CallFriendFragment.NewInstance(_currentQuestion);
                    dialogCallFriend.Cancelable = false;
                    dialogCallFriend.Show(ft, nameof(CallFriendFragment));
                    break;
                case Resource.Id.peopleButton:
                    _peopleButton.Enabled = false;
                    _peopleButton.SetColorFilter(new Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.lighter_gray)));
                    RemoveFragmentIfOpened(ft, nameof(AuditoryHelpFragment));

                    var dialogPeopleHelp = AuditoryHelpFragment.NewInstance(_currentQuestion);
                    dialogPeopleHelp.Cancelable = false;
                    dialogPeopleHelp.Show(ft, nameof(AuditoryHelpFragment));
                    break;
                case Resource.Id.fiftyButton:
                    _twoVariantsButton.Enabled = false;
                    _twoVariantsButton.SetColorFilter(new Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.lighter_gray)));
                    LeaveTwoVariants(_currentQuestion);
                    break;
                default:
                    break;
            }
            }

        private void LeaveTwoVariants(Question currentQuestion)
        {
            Random rand = new Random();
            int temp;
            temp = rand.Next(1, 4);

            switch (currentQuestion.Answer)
            {
                case 1:
                    if (temp == 1)
                    {
                        SetButtonDisabled(_variant3Layout, _variant3Button);
                        SetButtonDisabled(_variant4Layout, _variant4Button);
                    }
                    if (temp == 2)
                    {
                        SetButtonDisabled(_variant2Layout, _variant2Button);
                        SetButtonDisabled(_variant4Layout, _variant4Button);
                    }
                    if (temp == 3)
                    {
                        SetButtonDisabled(_variant2Layout, _variant2Button);
                        SetButtonDisabled(_variant3Layout, _variant3Button);
                    }
                    break;
                case 2:
                    if (temp == 1)
                    {
                        SetButtonDisabled(_variant3Layout, _variant3Button);
                        SetButtonDisabled(_variant4Layout, _variant4Button);
                    }
                    if (temp == 2)
                    {
                        SetButtonDisabled(_variant1Layout, _variant1Button);
                        SetButtonDisabled(_variant4Layout, _variant4Button);
                    }
                    if (temp == 3)
                    {
                        SetButtonDisabled(_variant1Layout, _variant1Button);
                        SetButtonDisabled(_variant3Layout, _variant3Button);
                    }
                    break;
                case 3:
                    if (temp == 1)
                    {
                        SetButtonDisabled(_variant2Layout, _variant2Button);
                        SetButtonDisabled(_variant4Layout, _variant4Button);
                    }
                    if (temp == 2)
                    {
                        SetButtonDisabled(_variant1Layout, _variant1Button);
                        SetButtonDisabled(_variant4Layout, _variant4Button);
                    }
                    if (temp == 3)
                    {
                        SetButtonDisabled(_variant1Layout, _variant1Button);
                        SetButtonDisabled(_variant2Layout, _variant2Button);
                    }
                    break;
                case 4:
                    if (temp == 1)
                    {
                        SetButtonDisabled(_variant2Layout, _variant2Button);
                        SetButtonDisabled(_variant3Layout, _variant3Button);
                    }
                    if (temp == 2)
                    {
                        SetButtonDisabled(_variant1Layout, _variant1Button);
                        SetButtonDisabled(_variant3Layout, _variant3Button);
                    }
                    if (temp == 3)
                    {
                        SetButtonDisabled(_variant1Layout, _variant1Button);
                        SetButtonDisabled(_variant2Layout, _variant2Button);
                    }
                    break;
                default:
                    break;
            }
        }

        private void SetButtonDisabled(FrameLayout buttonLayout, Button button)
        {
            buttonLayout.Enabled = false;
            button.SetBackgroundResource(Resource.Drawable.button_disabled);
            _needEnableButtons = true;
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
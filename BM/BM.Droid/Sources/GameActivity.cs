using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Support.V7.App;
using Android.Text.Method;
using System.Threading.Tasks;
using Android.Animation;
using Android.Views;
using Android.Support.V4.Content;
using Android.Media;

namespace BM.Droid.Sources
{
    [Activity(Label = "GameActivity", Theme = "@style/AppTheme.Main", 
        Icon = "@mipmap/ic_launcher")]
    public class GameActivity : AppCompatActivity
    {
        private string _lastQuestionIdTag = nameof(_lastQuestionIdTag);
        private int _lastQuestionId = -1;
        private bool _needFinishActivity;
        bool _doubleBackToExitPressedOnce = false;
        private TextView _question;
        private Button _variant1Button;
        private Button _variant2Button;
        private Button _variant3Button;
        private Button _variant4Button;
        private int _answer1SwapWith;
        private bool _anotherAnswersSwap;
        private View _inactiveView;
        private FrameLayout _variant1Layout;
        private FrameLayout _variant2Layout;
        private FrameLayout _variant3Layout;
        private FrameLayout _variant4Layout;
        private ImageButton _backButton;
        private Button _pointsButton;
        private ImageButton _callButton;
        private ImageButton _peopleButton;
        private ImageButton _twoVariantsButton;
        private int _currentQuestion;
        private bool _needEnableButtons;
        private QuestionsDatabase _questionsDatabase = null;
        private List<questions> _gameQuestions = null;
        private ProgressDialog _progressDialog;
        private int _goodColor;
        private int _defaultColor;
        private int _badColor;
        private bool _inactive;
        private bool _gotMillion;
        private bool _withoutHelp = true;
        private MediaPlayer _hintPlayer;
        private MediaPlayer _wrongPlayer;
        private MediaPlayer _successPlayer;
        private MediaPlayer _startPlayer;
        private PreferencesHelper _recordsHelper;
        private bool _soundEnabled = true;

        private bool Inactive
        {
            get { return _inactive; }
            set
            {
                _inactiveView.Visibility = value ? ViewStates.Visible : ViewStates.Gone;
                _inactive = value;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.game);

            this.Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);

            _needEnableButtons = false;
            //_question = FindViewById<TextView>(Resource.Id.question);
            //_question.MovementMethod = new ScrollingMovementMethod();
            _inactiveView = FindViewById(Resource.Id.inactiveView);
            _question = FindViewById<TextView>(Resource.Id.question);
            _question.MovementMethod = new ScrollingMovementMethod();
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

            _progressDialog = new ProgressDialog(this, Resource.Style.ProgressDialogTheme) { Indeterminate = true };
            _progressDialog.SetCancelable(false);
            _progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            _progressDialog.SetMessage("��������...");

            _variant1Layout.Click += OnAnswerButtonClick;
            _variant2Layout.Click += OnAnswerButtonClick;
            _variant3Layout.Click += OnAnswerButtonClick;
            _variant4Layout.Click += OnAnswerButtonClick;
            _backButton.Click += OnImageButtonClicked;
            _backButton.LongClick += OnImageButtonLongClick;
            _callButton.Click += OnImageButtonClicked;
            _callButton.LongClick += OnImageButtonLongClick;
            _peopleButton.Click += OnImageButtonClicked;
            _peopleButton.LongClick += OnImageButtonLongClick;
            _twoVariantsButton.Click += OnImageButtonClicked;
            _twoVariantsButton.LongClick += OnImageButtonLongClick;
            _pointsButton.Click += OnButtonClicked;

            _startPlayer = MediaPlayer.Create(this, Resource.Raw.start);
            _successPlayer = MediaPlayer.Create(this, Resource.Raw.success);
            _hintPlayer = MediaPlayer.Create(this, Resource.Raw.hint);
            _wrongPlayer = MediaPlayer.Create(this, Resource.Raw.wrong);

            _defaultColor =  ContextCompat.GetColor(this, Resource.Color.bm_blue);
            _goodColor = ContextCompat.GetColor(this, Resource.Color.good_answer);
            _badColor = ContextCompat.GetColor(this, Resource.Color.bad_answer);

            _recordsHelper = new PreferencesHelper();
            _recordsHelper.InitHeplerForSound(this);
            _soundEnabled = _recordsHelper.GetSoundEnabled();
                        
            if (savedInstanceState != null)
            {
                _callButton.Enabled = savedInstanceState.GetBoolean(nameof(_callButton));
                _callButton.SetColorFilter(new Android.Graphics.Color(ContextCompat.GetColor(this, _callButton.Enabled ? Resource.Color.bm_white : Resource.Color.lighter_gray)));
                _peopleButton.Enabled = savedInstanceState.GetBoolean(nameof(_peopleButton));
                _peopleButton.SetColorFilter(new Android.Graphics.Color(ContextCompat.GetColor(this, _peopleButton.Enabled ? Resource.Color.bm_white : Resource.Color.lighter_gray)));
                _twoVariantsButton.Enabled = savedInstanceState.GetBoolean(nameof(_twoVariantsButton));
                _twoVariantsButton.SetColorFilter(new Android.Graphics.Color(ContextCompat.GetColor(this, _twoVariantsButton.Enabled ? Resource.Color.bm_white : Resource.Color.lighter_gray)));
                _needEnableButtons = savedInstanceState.GetBoolean(nameof(_needEnableButtons));
                _currentQuestion = savedInstanceState.GetInt(nameof(_currentQuestion));
                _lastQuestionId = savedInstanceState.GetInt(nameof(_lastQuestionIdTag));
                _needFinishActivity = savedInstanceState.GetBoolean(nameof(_needFinishActivity));
                _answer1SwapWith = savedInstanceState.GetInt(nameof(_answer1SwapWith));
                _anotherAnswersSwap = savedInstanceState.GetBoolean(nameof(_anotherAnswersSwap));
                _gotMillion = savedInstanceState.GetBoolean(nameof(_gotMillion));
                _withoutHelp = savedInstanceState.GetBoolean(nameof(_withoutHelp));

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

            if (!_needFinishActivity)
                InitQuestionsAndStart();
            else
            {
                Intent myIntent = new Intent(this, typeof(MainActivity));
                myIntent.PutExtra("lastQuestion", _currentQuestion);
                myIntent.PutExtra("gotMillion", _gotMillion);
                myIntent.PutExtra("withoutHelp", _withoutHelp);
                myIntent.PutExtra("needFinishActivity", _needFinishActivity);
                SetResult(Result.Ok, myIntent);
                Finish();
            }
        }

        private void PlayMedia(int mediaPlayer)
        {
            if (_recordsHelper == null)
            {
                _recordsHelper = new PreferencesHelper();
                _recordsHelper.InitHeplerForSound(this);
                _soundEnabled = _recordsHelper.GetSoundEnabled();
            }

            if (!_soundEnabled)
                return;

            switch (mediaPlayer)
            {
                case 0:
                    if (_startPlayer == null)
                        _startPlayer = MediaPlayer.Create(this, Resource.Raw.start);
                    Task.Factory.StartNew(() =>
                    {
                        _startPlayer.Completion += (sender, args) => {
                            if (_startPlayer != null && !_startPlayer.IsPlaying)
                            {
                                _startPlayer.Release();
                                _startPlayer = null;
                            }
                        };
                        _startPlayer?.Start();
                    });
                    break;
                case 1:
                    if (_successPlayer == null)
                        _successPlayer = MediaPlayer.Create(this, Resource.Raw.success);
                    Task.Factory.StartNew(() =>
                    {
                        _successPlayer.Completion += (sender, args) => {
                            if (_successPlayer != null && !_successPlayer.IsPlaying)
                            {
                                _successPlayer.Release();
                                _successPlayer = null;
                            }
                        };
                        _successPlayer?.Start();
                    });
                    break;
                case 2:
                    if (_hintPlayer == null)
                        _hintPlayer = MediaPlayer.Create(this, Resource.Raw.hint);
                    Task.Factory.StartNew(() =>
                    {
                        _hintPlayer.Completion += (sender, args) => {
                            if (_hintPlayer != null && !_hintPlayer.IsPlaying)
                            {
                                _hintPlayer.Release();
                                _hintPlayer = null;
                            }
                        };
                        _hintPlayer?.Start();
                    });
                    break;
                case 3:
                    if (_wrongPlayer == null)
                        _wrongPlayer = MediaPlayer.Create(this, Resource.Raw.wrong);
                    Task.Factory.StartNew(() =>
                    {
                        _wrongPlayer.Completion += (sender, args) => {
                            if (_wrongPlayer != null && !_wrongPlayer.IsPlaying)
                            {
                                _wrongPlayer.Release();
                                _wrongPlayer = null;
                            }
                        };
                        _wrongPlayer?.Start();
                    });
                    break;
            }
        }

        private void OnImageButtonLongClick(object sender, View.LongClickEventArgs e)
        {
            var buttonClicked = (ImageButton)sender;
            switch (buttonClicked.Id)
            {
                case Resource.Id.backButton:
                    Toast.MakeText(this, "��������� � ����", ToastLength.Short).Show();
                    break;
                case Resource.Id.callButton:
                    Toast.MakeText(this, "��������� �����", ToastLength.Short).Show();
                    break;
                case Resource.Id.peopleButton:
                    Toast.MakeText(this, "������ ����", ToastLength.Short).Show();
                    break;
                case Resource.Id.fiftyButton:
                    Toast.MakeText(this, "�������� ��� ��������", ToastLength.Short).Show();
                    break;
                default:
                    break;
            }
        }

        private async void InitQuestionsAndStart()
        {
            if (!_progressDialog.IsShowing)
                _progressDialog.Show();

            _questionsDatabase = new QuestionsDatabase(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));

            _gameQuestions = await Task.Run(() => _questionsDatabase.GetAllItems());

            if (_lastQuestionId == -1)
            {
                _currentQuestion = 0;
                PlayMedia(0);
            }
            else
            {
                _questionsDatabase = new QuestionsDatabase(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));
                _gameQuestions[_currentQuestion] = await Task.Run(() => _questionsDatabase.GetLastItem(_lastQuestionId));
                _lastQuestionId = -1;
            }

            if (_progressDialog.IsShowing)
                _progressDialog.Dismiss();

            InstallCurrentQuestion(_currentQuestion);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            if (_progressDialog.IsShowing)
                _progressDialog.Dismiss();

            outState.PutBoolean(nameof(_callButton), _callButton.Enabled);
            outState.PutBoolean(nameof(_peopleButton), _peopleButton.Enabled);
            outState.PutBoolean(nameof(_twoVariantsButton), _twoVariantsButton.Enabled);
            outState.PutBoolean(nameof(_needEnableButtons), _needEnableButtons);
            outState.PutInt(nameof(_currentQuestion), _currentQuestion);
            if (_currentQuestion >= 0 && _currentQuestion <= 14)
                outState.PutInt(nameof(_lastQuestionIdTag), _lastQuestionId);
            else
                outState.PutInt(nameof(_lastQuestionIdTag), 0);
            outState.PutBoolean(nameof(_needFinishActivity), _needFinishActivity);
            outState.PutInt(nameof(_answer1SwapWith), _answer1SwapWith);
            outState.PutBoolean(nameof(_anotherAnswersSwap), _anotherAnswersSwap);
            outState.PutBoolean(nameof(_gotMillion), _gotMillion);
            outState.PutBoolean(nameof(_withoutHelp), _withoutHelp);

            if (_needEnableButtons)
            {
                outState.PutBoolean(nameof(_variant1Layout), _variant1Layout.Enabled);
                outState.PutBoolean(nameof(_variant2Layout), _variant2Layout.Enabled);
                outState.PutBoolean(nameof(_variant3Layout), _variant3Layout.Enabled);
                outState.PutBoolean(nameof(_variant4Layout), _variant4Layout.Enabled);
            }
        }

        private void InstallCurrentQuestion(int currentQuestion)
        {
            if (currentQuestion > 14 || _needFinishActivity)
            {
                Intent myIntent = new Intent(this, typeof(MainActivity));
                myIntent.PutExtra("lastQuestion", _currentQuestion);
                myIntent.PutExtra("gotMillion", _gotMillion);
                myIntent.PutExtra("withoutHelp", _withoutHelp);
                myIntent.PutExtra("needFinishActivity", _needFinishActivity);
                myIntent.PutExtra("bibleTextForAnswer", _gameQuestions[_currentQuestion - 1]?.bibleText ?? string.Empty);
                SetResult(Result.Ok, myIntent);
                Finish();
                return;
            }

            var question = _gameQuestions[currentQuestion];

            // ����� �� ����������
            if (_lastQuestionId != -1)
            {
                Random rand = new Random();
                _answer1SwapWith = rand.Next(2, 5);
                _anotherAnswersSwap = rand.Next(1, 3) == 2 ? true : false;
            }

            _lastQuestionId = question._id;

            switch(_answer1SwapWith)
            {
                case 2:
                    var c2 = question.variant2;
                    question.variant2 = question.variant1;
                    question.variant1 = c2;
                    if (question.answer == 2)
                        _gameQuestions[currentQuestion].answer = 1;
                    else if (question.answer == 1)
                        _gameQuestions[currentQuestion].answer = 2;

                    //Swap another
                    if (_anotherAnswersSwap)
                    {
                        c2 = question.variant3;
                        question.variant3 = question.variant4;
                        question.variant4 = c2;
                        if (question.answer == 3)
                            _gameQuestions[currentQuestion].answer = 4;
                        else if (question.answer == 4)
                            _gameQuestions[currentQuestion].answer = 3;
                    }

                    break;
                case 3:
                    var c3 = question.variant3;
                    question.variant3 = question.variant1;
                    question.variant1 = c3;
                    _answer1SwapWith = 3;
                    if (question.answer == 3)
                        _gameQuestions[currentQuestion].answer = 1;
                    else if (question.answer == 1)
                        _gameQuestions[currentQuestion].answer = 3;

                    //Swap another
                    if (_anotherAnswersSwap)
                    {
                        c3 = question.variant2;
                        question.variant2 = question.variant4;
                        question.variant4 = c3;
                        if (question.answer == 2)
                            _gameQuestions[currentQuestion].answer = 4;
                        else if (question.answer == 4)
                            _gameQuestions[currentQuestion].answer = 2;
                    }

                    break;
                case 4:
                    var c4 = question.variant4;
                    question.variant4 = question.variant1;
                    question.variant1 = c4;
                    _answer1SwapWith = 4;
                    if (question.answer == 4)
                        _gameQuestions[currentQuestion].answer = 1;
                    else if (question.answer == 1)
                        _gameQuestions[currentQuestion].answer = 4;

                    //Swap another
                    if (_anotherAnswersSwap)
                    {
                        c3 = question.variant2;
                        question.variant2 = question.variant3;
                        question.variant3 = c3;
                        if (question.answer == 2)
                            _gameQuestions[currentQuestion].answer = 3;
                        else if (question.answer == 3)
                            _gameQuestions[currentQuestion].answer = 2;
                    }

                    break;
                default:
                    break;
            }
            
            _question.Text = _gameQuestions[currentQuestion].questionText;
            _question.ScrollTo(0, 0);
            _variant1Button.Text = _gameQuestions[currentQuestion].variant1;
            _variant2Button.Text = _gameQuestions[currentQuestion].variant2;
            _variant3Button.Text = _gameQuestions[currentQuestion].variant3;
            _variant4Button.Text = _gameQuestions[currentQuestion].variant4;

            _pointsButton.Text = ValuesConverter.LevelToPoints(_gameQuestions[_currentQuestion].level) + " �����";
        }

        private void OnAnswerButtonClick(object sender, EventArgs e)
        {
            if (Inactive)
                return;
            Inactive = true;

            var question = _gameQuestions[_currentQuestion];
            var buttonClicked = (FrameLayout)sender;

            _currentQuestion++;

            switch (buttonClicked.Id)
            {
                case Resource.Id.variant1Layout:
                    if (question.answer == 1)
                    {
                        if (_currentQuestion > 14)
                        {
                            _gotMillion = true;
                            _needFinishActivity = true;
                        }
                        else
                            _lastQuestionId = _gameQuestions[_currentQuestion]._id;
                        PlayMedia(1);
                        StartAnimationButtonClick(_variant1Button, true);
                    }
                    else
                    {
                        _needFinishActivity = true;
                        PlayMedia(3);
                        StartAnimationButtonClick(_variant1Button, false, question.answer);
                    }
                    break;
                case Resource.Id.variant2Layout:
                    if (question.answer == 2)
                    {
                        if (_currentQuestion > 14)
                        {
                            _gotMillion = true;
                            _needFinishActivity = true;
                        }
                        else
                            _lastQuestionId = _gameQuestions[_currentQuestion]._id;
                        PlayMedia(1);
                        StartAnimationButtonClick(_variant2Button, true);
                    }
                    else
                    {
                        _needFinishActivity = true;
                        PlayMedia(3);
                        StartAnimationButtonClick(_variant2Button, false, question.answer);
                    }
                    break;
                case Resource.Id.variant3Layout:
                    if (question.answer == 3)
                    {
                        if (_currentQuestion > 14)
                        {
                            _gotMillion = true;
                            _needFinishActivity = true;
                        }
                        else
                            _lastQuestionId = _gameQuestions[_currentQuestion]._id;
                        PlayMedia(1);
                        StartAnimationButtonClick(_variant3Button, true);
                    }
                    else
                    {
                        _needFinishActivity = true;
                        PlayMedia(3);
                        StartAnimationButtonClick(_variant3Button, false, question.answer);
                    }
                    break;
                case Resource.Id.variant4Layout:
                    if (question.answer == 4)
                    {
                        if (_currentQuestion > 14)
                        {
                            _gotMillion = true;
                            _needFinishActivity = true;
                        }
                        else
                            _lastQuestionId = _gameQuestions[_currentQuestion]._id;
                        PlayMedia(1);
                        StartAnimationButtonClick(_variant4Button, true);
                    }  
                    else
                    {
                        _needFinishActivity = true;
                        PlayMedia(3);
                        StartAnimationButtonClick(_variant4Button, false, question.answer);
                    }
                    break;
                default:
                    break;
            }

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
        }

        private void StartAnimationButtonClick(Button button, bool isGoodAnswer, int goodAnswer = 1)
        {
            ValueAnimator anim = ObjectAnimator.OfInt(button, "backgroundColor",
                _defaultColor, isGoodAnswer ? _goodColor : _badColor);
            anim.SetEvaluator(new ArgbEvaluator());
            anim.RepeatMode = ValueAnimatorRepeatMode.Reverse;
            anim.RepeatCount = 1;
            anim.SetDuration(1000);
            anim.SetupStartValues();
            anim.AnimationEnd += ((s, args) =>
            {
                if (!isGoodAnswer)
                {
                    button.SetBackgroundResource(Resource.Drawable.button_background);
                    ShowGoodAnswer(goodAnswer);
                }
                else
                {
                    button.SetBackgroundResource(Resource.Drawable.button_background);
                    InstallCurrentQuestion(_currentQuestion);
                    Inactive = false;
                }
            });
            anim.Start();
        }

        private void ShowGoodAnswer(int answer)
        {
            Button goodButton = _variant1Button;

            switch (answer)
            {
                case 1:
                    goodButton = _variant1Button;
                    break;
                case 2:
                    goodButton = _variant2Button;
                    break;
                case 3:
                    goodButton = _variant3Button;
                    break;
                case 4:
                    goodButton = _variant4Button;
                    break;
                default:
                    break;
            }

            ValueAnimator anim = ObjectAnimator.OfInt(goodButton, "backgroundColor",
                _defaultColor, _goodColor);
            anim.SetEvaluator(new ArgbEvaluator());
            anim.RepeatMode = ValueAnimatorRepeatMode.Reverse;
            anim.RepeatCount = 1;
            anim.SetDuration(1000);
            anim.SetupStartValues();
            anim.AnimationEnd += ((s, args) =>
            {
                goodButton.SetBackgroundResource(Resource.Drawable.button_background);
                InstallCurrentQuestion(_currentQuestion);
                Inactive = false;
            });
            anim.Start();
        }

        private void OnImageButtonClicked(object sender, EventArgs e)
        {
            var buttonClicked = (ImageButton)sender;
            var ft = SupportFragmentManager.BeginTransaction();
            switch (buttonClicked.Id)
            {
                case Resource.Id.backButton:
                    Intent myIntent = new Intent(this, typeof(MainActivity));
                    myIntent.PutExtra("lastQuestion", _currentQuestion);
                    myIntent.PutExtra("gotMillion", _gotMillion);
                    myIntent.PutExtra("withoutHelp", _withoutHelp);
                    myIntent.PutExtra("needFinishActivity", false);
                    SetResult(Result.Ok, myIntent);
                    Finish();
                    break;
                case Resource.Id.callButton:
                    _withoutHelp = false;
                    PlayMedia(2);
                    _callButton.Enabled = false;
                    _callButton.SetColorFilter(new Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.lighter_gray)));
                    RemoveFragmentIfOpened(ft, nameof(CallFriendFragment));

                    var dialogCallFriend = CallFriendFragment.NewInstance(ToQuestion(_gameQuestions[_currentQuestion]));
                    dialogCallFriend.Cancelable = false;
                    dialogCallFriend.Show(ft, nameof(CallFriendFragment));
                    break;
                case Resource.Id.peopleButton:
                    _withoutHelp = false;
                    PlayMedia(2);
                    _peopleButton.Enabled = false;
                    _peopleButton.SetColorFilter(new Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.lighter_gray)));
                    RemoveFragmentIfOpened(ft, nameof(AuditoryHelpFragment));

                    var dialogPeopleHelp = AuditoryHelpFragment.NewInstance(ToQuestion(_gameQuestions[_currentQuestion]));
                    dialogPeopleHelp.Cancelable = false;
                    dialogPeopleHelp.Show(ft, nameof(AuditoryHelpFragment));
                    break;
                case Resource.Id.fiftyButton:
                    _withoutHelp = false;
                    PlayMedia(2);
                    _twoVariantsButton.Enabled = false;
                    _twoVariantsButton.SetColorFilter(new Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.lighter_gray)));
                    LeaveTwoVariants(_gameQuestions[_currentQuestion]);
                    break;
                default:
                    break;
            }
        }

        private Question ToQuestion(questions questionFromDb)
        {
            return new Question
            {
                Id = questionFromDb._id,
                QuestionText = questionFromDb.questionText,
                Level = questionFromDb.level,
                Variant1 = questionFromDb.variant1,
                Variant2 = questionFromDb.variant2,
                Variant3 = questionFromDb.variant3,
                Variant4 = questionFromDb.variant4,
                Answer = questionFromDb.answer
            };

        }

        private void LeaveTwoVariants(questions currentQuestion)
        {
            Random rand = new Random();
            int temp;
            temp = rand.Next(1, 4);

            switch (currentQuestion.answer)
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
                    
                    var dialog = PointsFragment.NewInstance(_currentQuestion);
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
                Intent myIntent = new Intent(this, typeof(MainActivity));
                myIntent.PutExtra("lastQuestion", _currentQuestion);
                myIntent.PutExtra("gotMillion", _gotMillion);
                myIntent.PutExtra("withoutHelp", _withoutHelp);
                myIntent.PutExtra("needFinishActivity", false);
                SetResult(Result.Ok, myIntent);
                Finish();
                return;
            }

            this._doubleBackToExitPressedOnce = true;
            Toast.MakeText(this, "������� ��� ��� ����� ��������� � ����", ToastLength.Short).Show();

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
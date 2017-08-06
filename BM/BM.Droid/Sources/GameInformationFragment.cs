using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace BM.Droid.Sources
{
    public class GameInformationFragment : AppCompatDialogFragment
    {
        private const string _questionTag = nameof(_questionTag);
        private const string _gotMillionTag = nameof(_gotMillionTag);
        private const string _gameWasLoseTag = nameof(_gameWasLoseTag);
        private const string _bibleTextForAnswerTag = nameof(_bibleTextForAnswerTag);
        private int _lastQuestion;
        private bool _gotMillion = false;
        private TextView _questionInfo;
        private ImageView _millionImage;
        private int _imageForMillion = -1;
        private bool _gameWasLose;
        private bool _recordWasSaved;
        private string _bibleTextForAnswer;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var view = LayoutInflater.From(Activity).Inflate(Resource.Layout.fragment_game_info, null);

            InitTextViews(view);

            _lastQuestion = Arguments.GetInt(_questionTag);
            _gotMillion = Arguments.GetBoolean(_gotMillionTag);
            _gameWasLose = Arguments.GetBoolean(_gameWasLoseTag);
            _bibleTextForAnswer = Arguments.GetString(_bibleTextForAnswerTag);

            _millionImage.Visibility = _gotMillion ? ViewStates.Visible : ViewStates.Gone;
            var bibleText = string.IsNullOrEmpty(_bibleTextForAnswer) ? string.Empty : "(вопрос взят из " + _bibleTextForAnswer + ")";

            _questionInfo.Text = _gotMillion
                ? "Вы дошли до самого конца и выиграли! Поздравляем!"
                : $"Вы смогли дойти до {_lastQuestion}-го вопроса" + (_gameWasLose 
                    ? $", но ответили неверно. Ваши итоговые очки: {ValuesConverter.LevelToCheckPoints(_lastQuestion - 1)}. {bibleText}" 
                    : $"! Ваши очки: {ValuesConverter.LevelToPoints(_lastQuestion)}");

            var dialog = new Android.Support.V7.App.AlertDialog.Builder(Activity, Resource.Style.AlertDialogTheme)
                .SetTitle(_gotMillion ? "МИЛЛИОН ОЧКОВ!" : "Неплохо!")
                .SetView(view)
                .SetPositiveButton("ЗАКРЫТЬ", ConfirmButtonClicked)
                .Create();

            return dialog;
        }

        private void ProcessRecord()
        {
            var records = new PreferencesHelper();
            records.InitHelperForRecords(Activity);
            records.ProcessRecord(lastQuestion: _lastQuestion, gotMillion: _gotMillion, gameWasLose: _gameWasLose);
        }

        public override void OnResume()
        {
            base.OnResume();

            if (_millionImage.Visibility == ViewStates.Visible)
            {
                if (_imageForMillion == -1)
                {
                    Random rand = new Random();
                    int temp;
                    temp = rand.Next(1, 8);

                    switch (temp)
                    {
                        case 1:
                            _imageForMillion = Resource.Drawable.million1;
                            break;
                        case 2:
                            _imageForMillion = Resource.Drawable.million2;
                            break;
                        case 3:
                            _imageForMillion = Resource.Drawable.million3;
                            break;
                        case 4:
                            _imageForMillion = Resource.Drawable.million4;
                            break;
                        case 5:
                            _imageForMillion = Resource.Drawable.million5;
                            break;
                        case 6:
                            _imageForMillion = Resource.Drawable.million6;
                            break;
                        case 7:
                            _imageForMillion = Resource.Drawable.million7;
                            break;
                    }
                }

                _millionImage.SetImageResource(_imageForMillion);
            }
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutInt(nameof(_questionTag), _lastQuestion);
            outState.PutBoolean(nameof(_gotMillionTag), _gotMillion);
            outState.PutBoolean(nameof(_gameWasLoseTag), _gameWasLose);
            outState.PutInt(nameof(_imageForMillion), _imageForMillion);
            outState.PutBoolean(nameof(_recordWasSaved), _recordWasSaved);
            outState.PutString(nameof(_bibleTextForAnswerTag), _bibleTextForAnswer);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            if (savedInstanceState != null)
            {
                _lastQuestion = savedInstanceState.GetInt(nameof(_questionTag));
                _gotMillion = savedInstanceState.GetBoolean(nameof(_gotMillionTag));
                _gameWasLose = savedInstanceState.GetBoolean(nameof(_gameWasLoseTag));
                _imageForMillion = savedInstanceState.GetInt(nameof(_imageForMillion));
                _recordWasSaved = savedInstanceState.GetBoolean(nameof(_recordWasSaved));
                _bibleTextForAnswer = savedInstanceState.GetString(nameof(_bibleTextForAnswerTag));
            }

            if (!_recordWasSaved)
                ProcessRecord();
            _recordWasSaved = true;

            base.OnActivityCreated(savedInstanceState);            
        }

        public static GameInformationFragment NewInstance(int question, bool gameWasLose, bool gotMillion = false, string bibleTextForAnswer = "")
        {
            var args = new Bundle();
            args.PutInt(_questionTag, question);
            args.PutBoolean(_gotMillionTag, gotMillion);
            args.PutBoolean(_gameWasLoseTag, gameWasLose);
            args.PutString(_bibleTextForAnswerTag, bibleTextForAnswer);

            return new GameInformationFragment { Arguments = args };
        }

        private void ConfirmButtonClicked(object sender, DialogClickEventArgs e)
        {
            Dismiss();
        }

        private void InitTextViews(View view)
        {
            _questionInfo = view.FindViewById<TextView>(Resource.Id.questionInfo);
            _millionImage = view.FindViewById<ImageView>(Resource.Id.millionImage);
        }
    }
}
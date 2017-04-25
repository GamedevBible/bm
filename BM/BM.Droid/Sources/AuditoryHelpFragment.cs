using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Support.V4.Content;

namespace BM.Droid.Sources
{
    internal class AuditoryHelpFragment : AppCompatDialogFragment
    {
        private const string _questionTag = nameof(_questionTag);
        private Question _question;
        private TextView _answer1;
        private TextView _answer2;
        private TextView _answer3;
        private TextView _answer4;
        private int[] _percentArray;
        private bool _auditoryGaveAnswer;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var view = LayoutInflater.From(Activity).Inflate(Resource.Layout.fragment_auditory_help, null);

            InitTextViews(view);

            _question = (Question)Arguments.GetSerializable(_questionTag);

            var dialog = new Android.Support.V7.App.AlertDialog.Builder(Activity, Resource.Style.AlertDialogTheme)
                .SetTitle("Помощь зала")
                .SetView(view)
                .SetPositiveButton("OK", ConfirmButtonClicked)
                .Create();

            return dialog;
        }

        public override void OnResume()
        {
            base.OnResume();

            if (!_auditoryGaveAnswer)
                InitAuditoryHelp(_question);
            else
                InstallAnswer(_question, _percentArray);
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutBoolean(nameof(_auditoryGaveAnswer), _auditoryGaveAnswer);
            outState.PutIntArray(nameof(_percentArray), _percentArray);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            if (savedInstanceState != null)
            {
                _auditoryGaveAnswer = savedInstanceState.GetBoolean(nameof(_auditoryGaveAnswer));
                _percentArray = savedInstanceState.GetIntArray(nameof(_percentArray));
            }
        }

        private void InitAuditoryHelp(Question _question)
        {
            Random rand = new Random();
            var numberOfRandomizer = rand.Next(1, 6);
            _percentArray = SelectRandomizer(numberOfRandomizer);

            InstallAnswer(_question, _percentArray);
            _auditoryGaveAnswer = true;
        }

        private void InstallAnswer(Question _question, int[] percentArray)
        {
            switch (_question.Answer)
            {
                case 1:
                    _answer1.Text = $"1. {percentArray[0]}%";
                    _answer1.SetTextColor(new Android.Graphics.Color(Activity.GetColor(Resource.Color.bm_blue)));
                    _answer2.Text = $"2. {percentArray[1]}%";
                    _answer3.Text = $"3. {percentArray[2]}%";
                    _answer4.Text = $"4. {percentArray[3]}%";
                    break;
                case 2:
                    _answer2.Text = $"2. {percentArray[0]}%";
                    _answer2.SetTextColor(new Android.Graphics.Color(Activity.GetColor(Resource.Color.bm_blue)));
                    _answer1.Text = $"1. {percentArray[1]}%";
                    _answer3.Text = $"3. {percentArray[2]}%";
                    _answer4.Text = $"4. {percentArray[3]}%";
                    break;
                case 3:
                    _answer3.Text = $"3. {percentArray[0]}%";
                    _answer3.SetTextColor(new Android.Graphics.Color(Activity.GetColor(Resource.Color.bm_blue)));
                    _answer2.Text = $"2. {percentArray[1]}%";
                    _answer1.Text = $"1. {percentArray[2]}%";
                    _answer4.Text = $"4. {percentArray[3]}%";
                    break;
                case 4:
                    _answer4.Text = $"4. {percentArray[0]}%";
                    _answer4.SetTextColor(new Android.Graphics.Color(Activity.GetColor(Resource.Color.bm_blue)));
                    _answer2.Text = $"2. {percentArray[1]}%";
                    _answer3.Text = $"3. {percentArray[2]}%";
                    _answer1.Text = $"1. {percentArray[3]}%";
                    break;
                default:
                    break;
            }
        }

        private int[] SelectRandomizer (int number)
        {
            // Выбор одного из 5ти случайного набора процентов зала
            switch (number)
            {
                case 1:
                    return Randomizer1();
                case 2:
                    return Randomizer2();
                case 3:
                    return Randomizer3();
                case 4:
                    return Randomizer4();
                case 5:
                    return Randomizer5();
                default:
                    return new int[] { };
            }
        }

        private int[] Randomizer1 ()
        {
            return new int[] { 45, 20, 10, 25 };
        }

        private int[] Randomizer2()
        {
            return new int[] { 42, 23, 14, 21 };
        }

        private int[] Randomizer3()
        {
            return new int[] { 51, 18, 16, 15 };
        }

        private int[] Randomizer4()
        {
            return new int[] { 40, 20, 15, 25 };
        }

        private int[] Randomizer5()
        {
            return new int[] { 44, 22, 11, 23 };
        }

        private void InitTextViews(View view)
        {
            _answer1 = view.FindViewById<TextView>(Resource.Id.answer1);
            _answer2 = view.FindViewById<TextView>(Resource.Id.answer2);
            _answer3 = view.FindViewById<TextView>(Resource.Id.answer3);
            _answer4 = view.FindViewById<TextView>(Resource.Id.answer4);
        }

        private void ConfirmButtonClicked(object sender, DialogClickEventArgs e)
        {
            Dismiss();
        }

        public static AuditoryHelpFragment NewInstance(Question question)
        {
            var args = new Bundle();
            args.PutSerializable(_questionTag, question);

            return new AuditoryHelpFragment { Arguments = args };
        }
    }
}
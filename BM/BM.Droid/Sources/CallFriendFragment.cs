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
using System.Threading.Tasks;
using BM;

namespace BM.Droid.Sources
{
    internal class CallFriendFragment : AppCompatDialogFragment
    {
        private const string _questionTag = nameof(_questionTag);
        private Question _question;
        private TextView _answer;
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var view = LayoutInflater.From(Activity).Inflate(Resource.Layout.fragment_call_friend, null);

            InitTextViews(view);

            _question = (Question)Arguments.GetSerializable(_questionTag);

            var dialog = new Android.Support.V7.App.AlertDialog.Builder(Activity, Resource.Style.AlertDialogTheme)
                .SetTitle("������ �����")
                .SetView(view)
                .SetPositiveButton("OK", ConfirmButtonClicked)
                .Create();
            
            return dialog;
        }

        public override void OnResume()
        {
            base.OnResume();

            InitCall(_question);
        }

        private async void InitCall(Question question)
        {
            _answer.Text = "������ �����...";
            await Task.Delay(1500);
            _answer.Text = "������ ������...";

            if (question.Level < 5)
            {
                await Task.Delay(1500);
                _answer.Text = $"� ����! �����: {GetCorrectAnswer(question)}";
            }
            else 
            if (question.Level >= 5 && question.Level < 10)
            {
                await Task.Delay(1500);
                _answer.Text = $"��... ��... {GetCorrectMiddleAnswer(question)}";
            }
            else
            {
                await Task.Delay(2500);
                _answer.Text = $"������� ������... {GetCorrectHardAnswer(question)}";
            }
        }

        private string GetCorrectAnswer(Question question)
        {
            switch (question.Answer)
            {
                case 1:
                    return question.Variant1;
                case 2:
                    return question.Variant2;
                case 3:
                    return question.Variant3;
                case 4:
                    return question.Variant4;
                default:
                    return "���� �� ��������...";
            }
        }

        private string GetCorrectMiddleAnswer(Question question)
        {
            Random rand = new Random();
            int countAnswers;
            countAnswers = rand.Next(1, 11);

            var correctVariant = GetCorrectAnswer(question);

            if (countAnswers == 10)
                return "� �� ���� �����...";

            if (countAnswers <= 5)
                return $"�����, �����: {correctVariant}";
            else
            {
                var secondVariant = question.Answer;
                while (secondVariant == question.Answer)
                {
                    secondVariant = rand.Next(1, 5);
                }

                switch (secondVariant)
                {
                    case 1:
                        return countAnswers <= 7 ? $"{question.Variant1} ��� {correctVariant}" : $"{correctVariant} ��� {question.Variant1}";
                    case 2:
                        return countAnswers <= 7 ? $"{question.Variant2} ��� {correctVariant}" : $"{correctVariant} ��� {question.Variant2}";
                    case 3:
                        return countAnswers <= 7 ? $"{question.Variant3} ��� {correctVariant}" : $"{correctVariant} ��� {question.Variant3}";
                    case 4:
                        return countAnswers <= 7 ? $"{question.Variant4} ��� {correctVariant}" : $"{correctVariant} ��� {question.Variant4}";
                    default:
                        return "���� �� ��������...";
                }
            }
        }

        private string GetCorrectHardAnswer(Question question)
        {
            Random rand = new Random();
            int countAnswers;
            countAnswers = rand.Next(1, 11);

            var correctVariant = GetCorrectAnswer(question);

            if (countAnswers >= 8)
                return "� �� ���� �����...";

            if (countAnswers <= 2)
                return $"�����, �����: {correctVariant}";
            else
            {
                var secondVariant = question.Answer;
                while (secondVariant == question.Answer)
                {
                    secondVariant = rand.Next(1, 5);
                }

                switch (secondVariant)
                {
                    case 1:
                        return countAnswers <= 5 ? $"{question.Variant1} ��� {correctVariant}" : $"{correctVariant} ��� {question.Variant1}";
                    case 2:
                        return countAnswers <= 5 ? $"{question.Variant2} ��� {correctVariant}" : $"{correctVariant} ��� {question.Variant2}";
                    case 3:
                        return countAnswers <= 5 ? $"{question.Variant3} ��� {correctVariant}" : $"{correctVariant} ��� {question.Variant3}";
                    case 4:
                        return countAnswers <= 5 ? $"{question.Variant4} ��� {correctVariant}" : $"{correctVariant} ��� {question.Variant4}";
                    default:
                        return "���� �� ��������...";
                }
            }
        }

        public static CallFriendFragment NewInstance(Question question)
        {
            var args = new Bundle();
            args.PutSerializable(_questionTag, question);

            return new CallFriendFragment { Arguments = args };
        }

        private void ConfirmButtonClicked(object sender, DialogClickEventArgs e)
        {
            Dismiss();
        }

        private void InitTextViews(View view)
        {
            _answer = view.FindViewById<TextView>(Resource.Id.answer);
        }
    }
}
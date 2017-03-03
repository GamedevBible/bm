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
                .SetTitle("Звонок другу")
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
            _answer.Text = "Звоним другу...";
            await Task.Delay(1500);
            _answer.Text = "Задаем вопрос...";

            if (question.Level < 5)
            {
                await Task.Delay(1500);
                _answer.Text = $"Я знаю! Ответ: {GetCorrectAnswer(question)}";
            }
            else 
            if (question.Level >= 5 && question.Level < 10)
            {
                await Task.Delay(1500);
                _answer.Text = $"Хм... Ну... {GetCorrectMiddleAnswer(question)}";
            }
            else
            {
                await Task.Delay(2500);
                _answer.Text = $"Тяжелый вопрос... {GetCorrectHardAnswer(question)}";
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
                    return "Друг не отвечает...";
            }
        }

        private string GetCorrectMiddleAnswer(Question question)
        {
            Random rand = new Random();
            int countAnswers;
            countAnswers = rand.Next(1, 11);

            var correctVariant = GetCorrectAnswer(question);

            if (countAnswers == 10)
                return "Я не знаю ответ...";

            if (countAnswers <= 5)
                return $"Думаю, ответ: {correctVariant}";
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
                        return countAnswers <= 7 ? $"{question.Variant1} или {correctVariant}" : $"{correctVariant} или {question.Variant1}";
                    case 2:
                        return countAnswers <= 7 ? $"{question.Variant2} или {correctVariant}" : $"{correctVariant} или {question.Variant2}";
                    case 3:
                        return countAnswers <= 7 ? $"{question.Variant3} или {correctVariant}" : $"{correctVariant} или {question.Variant3}";
                    case 4:
                        return countAnswers <= 7 ? $"{question.Variant4} или {correctVariant}" : $"{correctVariant} или {question.Variant4}";
                    default:
                        return "Друг не отвечает...";
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
                return "Я не знаю ответ...";

            if (countAnswers <= 2)
                return $"Думаю, ответ: {correctVariant}";
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
                        return countAnswers <= 5 ? $"{question.Variant1} или {correctVariant}" : $"{correctVariant} или {question.Variant1}";
                    case 2:
                        return countAnswers <= 5 ? $"{question.Variant2} или {correctVariant}" : $"{correctVariant} или {question.Variant2}";
                    case 3:
                        return countAnswers <= 5 ? $"{question.Variant3} или {correctVariant}" : $"{correctVariant} или {question.Variant3}";
                    case 4:
                        return countAnswers <= 5 ? $"{question.Variant4} или {correctVariant}" : $"{correctVariant} или {question.Variant4}";
                    default:
                        return "Друг не отвечает...";
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
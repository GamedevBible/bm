using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        private int _lastQuestion;
        private bool _gotMillion = false;
        private TextView _questionInfo;
        private ImageView _millionImage;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var view = LayoutInflater.From(Activity).Inflate(Resource.Layout.fragment_game_info, null);

            InitTextViews(view);

            _lastQuestion = Arguments.GetInt(_questionTag);
            _gotMillion = Arguments.GetBoolean(_gotMillionTag);

            _millionImage.Visibility = _gotMillion ? ViewStates.Visible : ViewStates.Gone;

            _questionInfo.Text = _gotMillion 
                ? "Вы дошли до самого конца и выиграли! Поздравляем!" 
                : $"Вы дошли до {_lastQuestion}-го вопроса!";

            if (_millionImage.Visibility == ViewStates.Visible)
            {
                Random rand = new Random();
                int temp;
                temp = rand.Next(1, 8);

                var img = 1;

                switch (temp)
                {
                    case 1:
                        img = Resource.Drawable.million1;
                        break;
                    case 2:
                        img = Resource.Drawable.million2;
                        break;
                    case 3:
                        img = Resource.Drawable.million3;
                        break;
                    case 4:
                        img = Resource.Drawable.million4;
                        break;
                    case 5:
                        img = Resource.Drawable.million5;
                        break;
                    case 6:
                        img = Resource.Drawable.million6;
                        break;
                    case 7:
                        img = Resource.Drawable.million7;
                        break;
                }

                _millionImage.SetImageResource(img);
            }

            var dialog = new Android.Support.V7.App.AlertDialog.Builder(Activity, Resource.Style.AlertDialogTheme)
                .SetTitle(_gotMillion ? "МИЛЛИОН ОЧКОВ!" : "Неплохо!")
                .SetView(view)
                .SetPositiveButton("ЗАКРЫТЬ", ConfirmButtonClicked)
                .Create();

            return dialog;
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutInt(nameof(_questionTag), _lastQuestion);
            outState.PutBoolean(nameof(_gotMillionTag), _gotMillion);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            if (savedInstanceState != null)
            {
                _lastQuestion = savedInstanceState.GetInt(nameof(_questionTag));
                _gotMillion = savedInstanceState.GetBoolean(nameof(_gotMillionTag));
            }
        }

        public static GameInformationFragment NewInstance(int question, bool gotMillion = false)
        {
            var args = new Bundle();
            args.PutInt(_questionTag, question);
            args.PutBoolean(_gotMillionTag, gotMillion);

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
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
using Android.Support.V4.Content;

namespace BM.Droid.Sources
{
    class PointsFragment : AppCompatDialogFragment
    {
        private const string _questionNumberTag = nameof(_questionNumberTag);

        private List<TextView> _textViews = new List<TextView>();

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var view = LayoutInflater.From(Activity).Inflate(Resource.Layout.fragment_points, null);

            InitTextViews(view);

            var dialog = new Android.Support.V7.App.AlertDialog.Builder(Activity, Resource.Style.PointsDialog)
                .SetTitle("Таблица очков")
                .SetView(view)
                .Create();

            var currentPoints = Arguments.GetInt(_questionNumberTag);

            for (int i = 0; i < 15; i++)
            {
                _textViews[i].SetTextColor(new Android.Graphics.Color(ContextCompat.GetColor(Activity, Resource.Color.lighter_gray)));
                
                if (i == currentPoints)
                {
                    _textViews[i].SetTextColor(new Android.Graphics.Color(ContextCompat.GetColor(Activity, Resource.Color.bm_blue)));
                    break;
                }
            }

            return dialog;
        }

        private void InitTextViews(View view)
        {
            _textViews.Add(view.FindViewById<TextView>(Resource.Id.points_0));
            _textViews.Add(view.FindViewById<TextView>(Resource.Id.points_1));
            _textViews.Add(view.FindViewById<TextView>(Resource.Id.points_2));
            _textViews.Add(view.FindViewById<TextView>(Resource.Id.points_3));
            _textViews.Add(view.FindViewById<TextView>(Resource.Id.points_4));
            _textViews.Add(view.FindViewById<TextView>(Resource.Id.points_5));
            _textViews.Add(view.FindViewById<TextView>(Resource.Id.points_6));
            _textViews.Add(view.FindViewById<TextView>(Resource.Id.points_7));
            _textViews.Add(view.FindViewById<TextView>(Resource.Id.points_8));
            _textViews.Add(view.FindViewById<TextView>(Resource.Id.points_9));
            _textViews.Add(view.FindViewById<TextView>(Resource.Id.points_10));
            _textViews.Add(view.FindViewById<TextView>(Resource.Id.points_11));
            _textViews.Add(view.FindViewById<TextView>(Resource.Id.points_12));
            _textViews.Add(view.FindViewById<TextView>(Resource.Id.points_13));
            _textViews.Add(view.FindViewById<TextView>(Resource.Id.points_14));
        }

        public static PointsFragment NewInstance(int questionNumber)
        {
            var args = new Bundle();
            args.PutInt(_questionNumberTag, questionNumber);

            return new PointsFragment { Arguments = args };
        }
    }
}
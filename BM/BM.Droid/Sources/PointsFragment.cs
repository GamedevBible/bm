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

namespace BM.Droid.Sources
{
    class PointsFragment : AppCompatDialogFragment
    {
        private const string _questionNumberTag = nameof(_questionNumberTag);

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var view = LayoutInflater.From(Activity).Inflate(Resource.Layout.fragment_points, null);

            var dialog = new Android.Support.V7.App.AlertDialog.Builder(Activity)
                .SetTitle("Таблица очков")
                .SetView(view)
                .Create();

            //Arguments.GetInt(_questionNumberTag);
            return dialog;
        }

        public static PointsFragment NewInstance(int questionNumber)
        {
            var args = new Bundle();
            args.PutLong(_questionNumberTag, questionNumber);

            return new PointsFragment { Arguments = args };
        }
    }
}
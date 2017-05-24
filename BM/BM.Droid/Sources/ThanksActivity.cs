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

namespace BM.Droid.Sources
{
    [Activity(Label = "ThanksActivity", Theme = "@style/AppTheme.Main",
        Icon = "@mipmap/ic_launcher")]
    public class ThanksActivity : AppCompatActivity
    {
        private TextView _thanksTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.thanks);

            _thanksTextView = FindViewById<TextView>(Resource.Id.thanksTextView);
            _thanksTextView.MovementMethod = new ScrollingMovementMethod();

            _thanksTextView.Text = "Разработка: " + "С. Ларионов (мл.)" + "\n" +
                "Вопросы для игры: " + "С. Ларионов (ст.)" + "\n" + "\n" +
                "Благодарности:" + "\n" + "(пока пусто)";
        }

        public static Intent CreateStartIntent(Context context)
        {
            var intent = new Intent(context, typeof(ThanksActivity));

            return intent;
        }
    }
}
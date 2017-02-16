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
    [Activity(Label = "GameActivity", Theme = "@style/AppTheme.Main")]
    public class GameActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.game);
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
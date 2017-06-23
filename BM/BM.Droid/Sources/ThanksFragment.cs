using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V7.App;
using Android.Support.V4.Content;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BM.Droid.Sources
{
    internal class ThanksFragment : AppCompatDialogFragment
    {
        private TextView _thanksTextView;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var view = LayoutInflater.From(Activity).Inflate(Resource.Layout.fragment_thanks, null);

            _thanksTextView = view.FindViewById<TextView>(Resource.Id.thanksTextView);
            _thanksTextView.Text = "Разработка: " + "С. Ларионов (мл.)" + "\n" +
                "Вопросы для игры: " + "С. Ларионов (ст.)" + "\n" + "\n" +
                "Благодарности:" + "\n" + "(здесь будут отмечены все, оказавшие нам поддержку и помощь)";

            var dialog = new Android.Support.V7.App.AlertDialog.Builder(Activity, Resource.Style.AlertDialogTheme)
                .SetTitle("Благодарности")
                .SetView(view)
                .SetPositiveButton("OK", ConfirmButtonClicked)
                .SetCancelable(false)
                .Create();
            
            return dialog;
        }

        public static ThanksFragment NewInstance()
        {
            var args = new Bundle();

            return new ThanksFragment { Arguments = args };
        }

        private void ConfirmButtonClicked(object sender, DialogClickEventArgs e)
        {
            ((Android.Support.V7.App.AlertDialog)sender).Dismiss();
        }
    }
}
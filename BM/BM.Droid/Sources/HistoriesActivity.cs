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
    [Activity(Label = "HistoriesActivity", Theme = "@style/AppTheme.Main",
        Icon = "@mipmap/ic_launcher")]
    internal class HistoriesActivity : AppCompatActivity
    {
        private TextView _history1;
        private TextView _history2;
        private TextView _history3;
        private TextView _history4;
        private TextView _history5;
        private TextView _history6;
        private TextView _history7;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.histories);

            _history1 = FindViewById<TextView>(Resource.Id.history1);
            _history2 = FindViewById<TextView>(Resource.Id.history2);
            _history3 = FindViewById<TextView>(Resource.Id.history3);
            _history4 = FindViewById<TextView>(Resource.Id.history4);
            _history5 = FindViewById<TextView>(Resource.Id.history5);
            _history6 = FindViewById<TextView>(Resource.Id.history6);
            _history7 = FindViewById<TextView>(Resource.Id.history7);

            FillHistories();
        }

        private void FillHistories()
        {
            _history1.Text = "";
            _history2.Text = "";
            _history3.Text = "";
            _history4.Text = "";
            _history5.Text = "";
            _history6.Text = "";
            _history7.Text = " По окончании вечерней молитвы на судах Российского императорского флота вахтенный начальник командовал «Накройся!», что означало надеть головные уборы, и одновременно подавался сигнал отбоя молитвы. Длилась такая молитва обычно 15 минут.";
        }

        public static Intent CreateStartIntent(Context context, string message = null)
        {
            var intent = new Intent(context, typeof(HistoriesActivity));

            return intent;
        }
    }
}
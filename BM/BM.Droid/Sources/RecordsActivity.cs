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
    [Activity(Label = "RecordsActivity", Theme = "@style/AppTheme.Main",
        Icon = "@mipmap/ic_launcher")]
    public class RecordsActivity : AppCompatActivity
    {
        private TextView _points1;
        private TextView _points2;
        private TextView _points3;
        private TextView _points4;
        private TextView _points5;
        private TextView _points6;
        private TextView _points7;

        private ImageButton _millionsButton;
        private ImageButton _cleverButton;
        private ImageButton _entersButton;

        private TextView _date1;
        private TextView _date2;
        private TextView _date3;
        private TextView _date4;
        private TextView _date5;
        private TextView _date6;
        private TextView _date7;

        private bool _inactive;
        private int _millionsCount;
        private int _entersCount;
        private int _cleverCount;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.records);

            _points1 = FindViewById<TextView>(Resource.Id.points1);
            _points2 = FindViewById<TextView>(Resource.Id.points2);
            _points3 = FindViewById<TextView>(Resource.Id.points3);
            _points4 = FindViewById<TextView>(Resource.Id.points4);
            _points5 = FindViewById<TextView>(Resource.Id.points5);
            _points6 = FindViewById<TextView>(Resource.Id.points6);
            _points7 = FindViewById<TextView>(Resource.Id.points7);

            _entersButton = FindViewById<ImageButton>(Resource.Id.enterGameButton);
            _millionsButton = FindViewById<ImageButton>(Resource.Id.millionsButton);
            _cleverButton = FindViewById<ImageButton>(Resource.Id.cleverButton);

            _date1 = FindViewById<TextView>(Resource.Id.date1);
            _date2 = FindViewById<TextView>(Resource.Id.date2);
            _date3 = FindViewById<TextView>(Resource.Id.date3);
            _date4 = FindViewById<TextView>(Resource.Id.date4);
            _date5 = FindViewById<TextView>(Resource.Id.date5);
            _date6 = FindViewById<TextView>(Resource.Id.date6);
            _date7 = FindViewById<TextView>(Resource.Id.date7);

            var recordsHelper = new PreferencesHelper();
            recordsHelper.InitHelperForRecords(this);

            var records = recordsHelper.GetRecords();
            records.OrderByDescending(t => t.QuestionNumber);

            _points1.Text = ValuesConverter.QuestionNumberToPoints(records[0].QuestionNumber);
            _points2.Text = ValuesConverter.QuestionNumberToPoints(records[1].QuestionNumber);
            _points3.Text = ValuesConverter.QuestionNumberToPoints(records[2].QuestionNumber);
            _points4.Text = ValuesConverter.QuestionNumberToPoints(records[3].QuestionNumber);
            _points5.Text = ValuesConverter.QuestionNumberToPoints(records[4].QuestionNumber);
            _points6.Text = ValuesConverter.QuestionNumberToPoints(records[5].QuestionNumber);
            _points7.Text = ValuesConverter.QuestionNumberToPoints(records[6].QuestionNumber);

            _date1.Text = records[0].RecordDate;
            _date2.Text = records[1].RecordDate;
            _date3.Text = records[2].RecordDate;
            _date4.Text = records[3].RecordDate;
            _date5.Text = records[4].RecordDate;
            _date6.Text = records[5].RecordDate;
            _date7.Text = records[6].RecordDate;

            _millionsCount = recordsHelper.GetMillionsCount();
            _entersCount = recordsHelper.GetEntersCount();
            _cleverCount = recordsHelper.GetCleverCount();

            _entersButton.SetImageResource(_entersCount >= 100 ? Resource.Drawable.calendar_check : Resource.Drawable.calendar_check_disabled);
            _millionsButton.SetImageResource(_millionsCount >= 50 ? Resource.Drawable.briefcase_check : Resource.Drawable.briefcase_check_disabled);
            _cleverButton.SetImageResource(_cleverCount >= 20 ? Resource.Drawable.account_check : Resource.Drawable.account_check_disabled);

            _entersButton.Click += OnEntersButtonClicked;
            _millionsButton.Click += OnMillionsButtonClicked;
            _cleverButton.Click += OnCleverButtonClicked;
        }

        private void OnEntersButtonClicked(object sender, EventArgs e)
        {
            if (_inactive)
                return;
            _inactive = true;

            var dialog = new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.AlertDialogTheme)
                    .SetTitle(_entersCount >= 100 ? "Популярное приложение!" : "Популярное приложение")
                    .SetMessage(_entersCount >= 100 ? "Вы открыли приложение более 100 раз! Поздравляем с достижением!" : "Это достижение еще не получено." + "\n" + "(при удалении приложения достижения также удаляются)")
                    .SetPositiveButton("Закрыть", AlertConfirmButtonClicked)
                    .SetCancelable(false)
                    .Create();

            dialog.Show();
        }

        private void OnMillionsButtonClicked(object sender, EventArgs e)
        {
            if (_inactive)
                return;
            _inactive = true;

            var dialog = new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.AlertDialogTheme)
                    .SetTitle(_millionsCount >= 50 ? "Настоящий победитель!" : "Настоящий победитель")
                    .SetMessage(_millionsCount >= 50 ? "Вы выиграли миллион очков более 50 раз! Поздравляем с достижением!" : "Это достижение еще не получено." + "\n" + "(при удалении приложения достижения также удаляются)")
                    .SetPositiveButton("Закрыть", AlertConfirmButtonClicked)
                    .SetCancelable(false)
                    .Create();

            dialog.Show();
        }

        private void OnCleverButtonClicked(object sender, EventArgs e)
        {
            if (_inactive)
                return;
            _inactive = true;

            var dialog = new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.AlertDialogTheme)
                    .SetTitle(_cleverCount >= 20 ? "Настоящий знаток!" : "Настоящий знаток")
                    .SetMessage(_cleverCount >= 20 ? "Вы выиграли миллион очков более 20 раз! И все это без подсказок! Поздравляем с достижением!" : "Это достижение еще не получено." + "\n" + "(при удалении приложения достижения также удаляются)")
                    .SetPositiveButton("Закрыть", AlertConfirmButtonClicked)
                    .SetCancelable(false)
                    .Create();

            dialog.Show();
        }

        private void AlertConfirmButtonClicked(object sender, DialogClickEventArgs e)
        {
            _inactive = false;
            ((Android.Support.V7.App.AlertDialog)sender).Dismiss();
        }

        public static Intent CreateStartIntent(Context context, string message = null)
        {
            var intent = new Intent(context, typeof(RecordsActivity));

            return intent;
        }
    }
}
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
    [Activity(Label = "BM", Theme = "@style/AppTheme.Main", WindowSoftInputMode = SoftInput.AdjustResize, NoHistory = true)]
    internal class MainMenuActivity : AppCompatActivity
    {
        private LinearLayout _newGameLayout;
        private ImageView _newGameImage;
        private TextView _newGameText;
        private LinearLayout _infoLayout;
        private ImageView _infoImage;
        private TextView _infoText;
        private LinearLayout _contactsLayout;
        private ImageView _contactsImage;
        private TextView _contactsText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.main_menu);

            _newGameLayout = FindViewById<LinearLayout>(Resource.Id.newGameLayout);
            _newGameImage = FindViewById<ImageView>(Resource.Id.newGameImage);
            _newGameText = FindViewById<TextView>(Resource.Id.newGameText);
            _infoLayout = FindViewById<LinearLayout>(Resource.Id.infoLayout);
            _infoImage = FindViewById<ImageView>(Resource.Id.infoImage);
            _infoText = FindViewById<TextView>(Resource.Id.infoText);
            _contactsLayout = FindViewById<LinearLayout>(Resource.Id.contactsLayout);
            _contactsImage = FindViewById<ImageView>(Resource.Id.contactsImage);
            _contactsText = FindViewById<TextView>(Resource.Id.contactsText);

            _newGameImage.SetImageResource(Resource.Drawable.book_open_variant);
            _infoImage.SetImageResource(Resource.Drawable.information_outline);
            _contactsImage.SetImageResource(Resource.Drawable.contact_mail);
        }

        public static Intent CreateStartIntent(Context context, string name)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (name == null) throw new ArgumentNullException(nameof(name));

            var intent = new Intent(context, typeof(MainMenuActivity));
            intent.PutExtra(nameof(name), name);

            return intent;
        }
    }
}
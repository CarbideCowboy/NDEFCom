using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Nfc;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace NDEFCom.Pages
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.DayNight.NoActionBar", MainLauncher = true)]
    [IntentFilter(new[] { NfcAdapter.ActionNdefDiscovered}, Categories = new[] { Intent.CategoryDefault }, DataMimeType = "text/plain")]
    public class ScanPrompt : AppCompatActivity
    {
        private NfcAdapter nfcDevice;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_scanprompt);

            nfcDevice = NfcAdapter.GetDefaultAdapter(this);

            HandleNdefIntent(this.Intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnResume()
        {
            base.OnResume();

            var tagDetected = new IntentFilter(NfcAdapter.ActionNdefDiscovered);
            var filters = new[] { tagDetected };
            var intent = new Intent(this, this.GetType()).AddFlags(ActivityFlags.SingleTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, 0);
            nfcDevice.EnableForegroundDispatch(this, pendingIntent, filters, null);
        }

        protected override void OnPause()
        {
            base.OnPause();
            nfcDevice.DisableForegroundDispatch(this);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            HandleNdefIntent(intent);
        }
        
        private void HandleNdefIntent(Intent intent)
        {
            if(intent.Action == NfcAdapter.ActionNdefDiscovered)
            {
                var tag = intent.GetParcelableExtra(NfcAdapter.ExtraTag) as Tag;
                if(tag != null)
                {
                    var rawMessages = intent.GetParcelableArrayExtra(NfcAdapter.ExtraNdefMessages);
                    if(rawMessages != null)
                    {
                        var message = (NdefMessage)rawMessages[0];
                        var record = message.GetRecords()[0];
                        if(record != null)
                        {
                            if(record.Tnf == NdefRecord.TnfWellKnown)
                            {
                                var payload = Encoding.ASCII.GetString(record.GetPayload());
                                StartActivity(typeof(NdefEdit));
                            }
                        }
                    }
                }
            }
        }
    }
}
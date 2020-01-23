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

namespace NDEFCom.Pages
{
    [Activity(Label = "NdefEdit")]
    public class NdefEdit : Activity
    {
        private string NdefPayload;
        private EditText uxNdefEditor;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.NdefEdit);

            NdefPayload = Intent.GetStringExtra("NdefPayload");

            //set up id's
            uxNdefEditor = FindViewById<EditText>(Resource.Id.uxNdefEditor);

            uxNdefEditor.Text = NdefPayload;

            CheckEncrypted();
        }

        private void CheckEncrypted()
        {
            if(NdefPayload.Length < 27)
            {
                return;
            }
            else if(!NdefPayload.Substring(0, 27).Equals("-----BEGIN PGP MESSAGE-----"))
            {
                return;
            }
            else
            {
                Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("Encrypted payload detected");
                alert.SetMessage("Would you like to attempt decryption with OpenKeychain?");
                alert.SetButton("Yes", (c, ev) =>
                {
                    ClipboardManager clipboard = (ClipboardManager)GetSystemService(Context.ClipboardService);
                    ClipData clip = ClipData.NewPlainText("payload", NdefPayload);
                    clipboard.PrimaryClip = clip;

                    Intent intent = Android.App.Application.Context.PackageManager.GetLaunchIntentForPackage("org.sufficientlysecure.keychain.debug");

                    if(intent != null)
                    {
                        intent.AddFlags(ActivityFlags.NewTask);
                        StartActivity(intent);
                    }
                    else
                    {
                        intent = new Intent(Intent.ActionView);
                        intent.AddFlags(ActivityFlags.NewTask);
                        intent.SetData(Android.Net.Uri.Parse("https://f-droid.org/en/packages/org.sufficientlysecure.keychain/"));
                        StartActivity(intent);
                    }
                });

                alert.SetButton2("No", (c, ev) =>
                {
                    return;
                });

                alert.Show();
            }
        }
    }
}
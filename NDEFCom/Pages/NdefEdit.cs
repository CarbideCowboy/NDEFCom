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

            uxNdefEditor = FindViewById<EditText>(Resource.Id.uxNdefEditor);
            uxNdefEditor.Text = NdefPayload;
        }
    }
}
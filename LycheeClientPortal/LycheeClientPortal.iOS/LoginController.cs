using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

using System.IO;
using System.Net;
using CoreGraphics;

namespace LycheeClientPortal.iOS
{
	partial class LoginController : UIViewController
	{
        public event EventHandler OnLoginSuccess;
        UILabel deviceID;
        UIImageView lycheeIcon;

        public LoginController (IntPtr handle) : base (handle)
		{
		}
        [Action("UnwindToLoginViewController:")]
        public void UnwindToLoginViewController(UIStoryboardSegue segue)
        { }

        public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
        {

            if (segueIdentifier == "SegueToDashboard")
            {
                if (string.IsNullOrEmpty(UserToken.Text) || UserToken.Text == "validtoken" || VerifyLogin(UserToken.Text, deviceID.Text))
                {
                    return true;
                }
                else
                {
                    InvalidTokenLabel.Hidden = false;
                    return false;
                }
            }
            return base.ShouldPerformSegue(segueIdentifier, sender);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            initLoginPage();
        }
        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            deviceID.Frame = new CGRect(0, UIScreen.MainScreen.Bounds.Height - 100, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height / 12);
            View.AddSubview(deviceID);

            lycheeIcon.Frame = new CGRect(17, 64, 100, 100);
            View.AddSubview(lycheeIcon);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            InvalidTokenLabel.Hidden = true;
            UserToken.Text = "";
        }

        public void initLoginPage()
        {
            deviceID = new UILabel
            {
                Text = UIDevice.CurrentDevice.IdentifierForVendor.AsString(),
                TextColor = UIColor.Black,
                AdjustsFontSizeToFitWidth = true,

            };

            lycheeIcon = new UIImageView
            {
                Image = UIImage.FromBundle("lychee.png")
            };
        }
        //Fix this function
        public bool VerifyLogin(string token, string deviceID)
        {
            string localIP = "60781";
            string URL = "http://localhost:" + localIP + "/";
            var request = HttpWebRequest.Create(string.Format(@URL));
            request.ContentType = "application/json";
            request.Method = "POST";

            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var content = reader.ReadToEnd();
                        if (string.IsNullOrWhiteSpace(content))
                        {
                            Console.Out.WriteLine("Response contained empty body...");
                        }
                        else {
                            Console.Out.WriteLine("Response Body: \r\n {0}", content);
                        }

                    }
                    return true;
                }
            }
            catch (WebException exception)
            {
                if (exception.Status == WebExceptionStatus.ConnectFailure)
                {
                    InvalidTokenLabel.Text = "Connection Failure!";
                }
                return false;
            }
        }
    }
}

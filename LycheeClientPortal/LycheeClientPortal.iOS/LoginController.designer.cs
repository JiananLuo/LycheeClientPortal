// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace LycheeClientPortal.iOS
{
	[Register ("LoginController")]
	partial class LoginController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel InvalidTokenLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton UserLoginButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField UserToken { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (InvalidTokenLabel != null) {
				InvalidTokenLabel.Dispose ();
				InvalidTokenLabel = null;
			}
			if (UserLoginButton != null) {
				UserLoginButton.Dispose ();
				UserLoginButton = null;
			}
			if (UserToken != null) {
				UserToken.Dispose ();
				UserToken = null;
			}
		}
	}
}

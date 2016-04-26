using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

using System.Linq;
using CoreGraphics;

namespace LycheeClientPortal.iOS
{
	partial class ProjectDetailController : UITabBarController
	{
        public Project thisProject { get; set; }

        public UIView titleView;
        UILabel projectCode;
        UILabel projectName;
        UIImageView managerPhoto;
        UIImageView healthPhoto;

        public nfloat titleViewYOffSet { get; set; }

        public ProjectDetailController(IntPtr handle) : base(handle)
        {
            //Hook up a "fade" animation between tabs
            ShouldSelectViewController = (tabController, controller) =>
            {
                if (SelectedViewController == null || controller == SelectedViewController)
                    return true;
                UIView fromView = SelectedViewController.View;
                UIView toView = controller.View;
                UIView.Transition(fromView, toView, .3f, UIViewAnimationOptions.TransitionCrossDissolve, null);
                return true;
            };
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            titleViewYOffSet = UIApplication.SharedApplication.StatusBarFrame.Height + NavigationController.NavigationBar.Bounds.Height + UIScreen.MainScreen.Bounds.Height / 6;

            //update view after loading
            initProjectDetailHeader();
            initTabBar();
            initViewSwipe();
        }
        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            titleView.Frame = new CGRect(
                0,
                this.NavigationController.NavigationBar.Bounds.Height + UIApplication.SharedApplication.StatusBarFrame.Height,
                UIScreen.MainScreen.Bounds.Width,
                UIScreen.MainScreen.Bounds.Height / 6);
            View.AddSubview(titleView);

            projectCode.Frame = new CGRect(
                titleView.Bounds.Width / 20,
                titleView.Bounds.Height / 4,
                titleView.Bounds.Width / 2,
                titleView.Bounds.Height / 5);
            titleView.AddSubview(projectCode);

            projectName.Frame = new CGRect(
                titleView.Bounds.Width / 20,
                titleView.Bounds.Height / 2,
                titleView.Bounds.Width * 2 / 3,
                titleView.Bounds.Height / 3);
            titleView.AddSubview(projectName);

            managerPhoto.Frame = new CGRect(
                titleView.Bounds.Width / 1.5 - 15,
                (titleView.Bounds.Height - titleView.Bounds.Width / 6) / 2,
                titleView.Bounds.Width / 6,
                titleView.Bounds.Width / 6);
            titleView.AddSubview(managerPhoto);

            healthPhoto.Frame = new CGRect(
                titleView.Bounds.Width / 1.2 - 7,
                (titleView.Bounds.Height - titleView.Bounds.Width / 6) / 2,
                titleView.Bounds.Width / 6,
                titleView.Bounds.Width / 6);
            titleView.AddSubview(healthPhoto);
        }
        private void initProjectDetailHeader()
        {
            this.Title = "Project Detail";

            titleView = new UIView
            {
                BackgroundColor = UIColor.Gray
            };

            projectCode = new UILabel
            {
                Text = "JONA-1234",
                TextColor = UIColor.LightGray,
                Font = UIFont.FromName("Helvetica", 12f)
            };

            projectName = new UILabel
            {
                Text = "IBM Infosphere",
                TextColor = UIColor.White,
                Font = UIFont.FromName("Helvetica", 24f)
            };

            managerPhoto = new UIImageView
            {
                Image = UIImage.FromBundle("defaultuser.png")
            };

            healthPhoto = new UIImageView
            {
                Image = UIImage.FromBundle("health.png")
            };
            //titleViewYOffSet = 200;
        }
        private void initTabBar()
        {
            TabBar.Items[0].BadgeValue = "9+";
            TabBar.Items[2].BadgeValue = thisProject.invoices.Where(x => x.InvoiceStateValue == InvoiceStateValue.late).Count().ToString();
        }

        protected void HandleLeftSwipe()
        {
            if (this.SelectedIndex != 2)
                this.SelectedIndex = this.SelectedIndex + 1;
            else
                this.SelectedIndex = 0;
        }
        protected void HandleRightSwipe()
        {
            if (this.SelectedIndex != 0)
                this.SelectedIndex = this.SelectedIndex - 1;
            else
                this.SelectedIndex = 2;
        }
        private void initViewSwipe()
        {
            var swipeLeft = new UISwipeGestureRecognizer(HandleLeftSwipe)
            { Direction = UISwipeGestureRecognizerDirection.Left };
            View.AddGestureRecognizer(swipeLeft);

            var swipeRight = new UISwipeGestureRecognizer(HandleRightSwipe)
            { Direction = UISwipeGestureRecognizerDirection.Right };
            View.AddGestureRecognizer(swipeRight);
        }
    }
}

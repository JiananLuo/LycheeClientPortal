using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

using System.Collections.Generic;
using CoreGraphics;
using BarChart;

namespace LycheeClientPortal.iOS
{
	partial class ProjectDetailTimelineController : UIViewController
	{
        BarChartView barChart;
        public ProjectDetailTimelineController (IntPtr handle) : base (handle)
		{
		}
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //update view after loading
            initBarChat();
        }
        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            barChart.Frame = new CGRect(
                0,
                ((ProjectDetailController)ParentViewController).titleViewYOffSet,
                UIScreen.MainScreen.Bounds.Width,
                UIScreen.MainScreen.Bounds.Height - ((ProjectDetailController)ParentViewController).titleViewYOffSet - ((UITabBarController)ParentViewController).TabBar.Bounds.Height);
            View.AddSubview(barChart);
        }

        public List<BarModel> GenerateTestData()
        {
            var models = new List<BarModel>();
            for (DateTime it = ((ProjectDetailController)ParentViewController).thisProject.startDate; it < ((ProjectDetailController)ParentViewController).thisProject.endDate; it = it.AddMonths(1))
            {
                models.Add(new BarModel() { Legend = it.ToString("MMM") + ", " + it.Year.ToString(), Value = it.Month, Color = UIColor.Gray });
            }
            return models;
        }

        public void initBarChat()
        {
            barChart = new BarChartView();
            barChart.BarColor = UIColor.Green;
            barChart.ItemsSource = GenerateTestData();
        }
    }
}

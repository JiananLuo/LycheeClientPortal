using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

using CoreGraphics;

namespace LycheeClientPortal.iOS
{
    public class ProjectOverViewSource : UITableViewSource
    {
        UIViewController parentController;
        string CellIdentifier = "projectOverviewTable";

        Project project;

        public ProjectOverViewSource(UIViewController controller, Project item)
        {
            parentController = controller;
            project = item;
        }
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);

            //---- if there are no cells to reuse, create a new one
            if (cell == null)
            { cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier); }

            var index = ((NSIndexPath)indexPath).Row;
            if (index == 0)
                cell.TextLabel.Text = "Name: " + project.projectName;
            else if (index == 1)
                cell.TextLabel.Text = "Stage: " + project.projectStage.ToString();
            else if (index == 2)
                cell.TextLabel.Text = "Status: " + project.projectStatus.ToString();
            else if (index == 3)
                cell.TextLabel.Text = "Start Date: " + project.startDate.ToString();
            else if (index == 4)
                cell.TextLabel.Text = "End Date: " + project.endDate.ToString();

            return cell;
        }


        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return 5;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 0.0f;
        }


        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            UIView view = new UIView();
            return view;
        }
    }

    partial class ProjectDetailOverviewController : UIViewController
	{
        UITableView projectOverviewTable;
        public ProjectDetailOverviewController(IntPtr handle) : base(handle)
        {
            projectOverviewTable = new UITableView(new CGRect(0, 0, 0, 0), UITableViewStyle.Grouped);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //update view after loading
            initProjectOverviewTable();
        }
        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            projectOverviewTable.Frame = new CGRect(
                0,
                ((ProjectDetailController)ParentViewController).titleViewYOffSet,
                UIScreen.MainScreen.Bounds.Width,
                UIScreen.MainScreen.Bounds.Height / 1.2);
            View.AddSubview(projectOverviewTable);
        }

        private void initProjectOverviewTable()
        {
            projectOverviewTable.Source = new ProjectOverViewSource(this, ((ProjectDetailController)ParentViewController).thisProject);
        }
    }
}

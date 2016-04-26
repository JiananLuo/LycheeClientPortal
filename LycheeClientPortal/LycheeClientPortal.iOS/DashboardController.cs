using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

using System.Collections.Generic;
using CoreGraphics;

namespace LycheeClientPortal.iOS
{
    public class ProjectSource : UITableViewSource
    {
        UIViewController parentController;
        string CellIdentifier = "TableCell";

        List<Project> TableItems = new List<Project>();


        public ProjectSource(UIViewController controller, List<Project> items)
        {
            parentController = controller;
            TableItems = items;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return TableItems.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
            Project item = TableItems[indexPath.Row];

            //---- if there are no cells to reuse, create a new one
            if (cell == null)
            { cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier); }

            cell.TextLabel.Text = item.projectName;
            cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //var index = indexPath.Row;
            //parentController.NavigationController.PushViewController(projectControllers[index], false);

            this.parentController.PerformSegue("SegueToProjectDetail", indexPath);
            tableView.DeselectRow(indexPath, true);
        }
    }
    partial class DashboardController : UIViewController
	{
        UITableView projectTable;
        public List<Project> ProjectList { get; set; }

        public DashboardController(IntPtr handle) : base(handle)
        {
            Project p1 = new Project("IBM Infosphere", new DateTime(2014, 2, 4), DateTime.Now.ToLocalTime().AddDays(90), ProjectStage.STAGE_DANGER, ProjectStatus.STATUS_ACTIVE);
            Project p2 = new Project("IBM Watson Analytics", new DateTime(2014, 2, 4), DateTime.Now.ToLocalTime().AddDays(90), ProjectStage.STAGE_DANGER, ProjectStatus.STATUS_ACTIVE);
            Project p3 = new Project("IBM Cognos Express", new DateTime(2014, 2, 4), DateTime.Now.ToLocalTime().AddDays(90), ProjectStage.STAGE_DANGER, ProjectStatus.STATUS_ACTIVE);
            Project p4 = new Project("IBM Predictive Analytics", new DateTime(2014, 2, 4), DateTime.Now.ToLocalTime().AddDays(90), ProjectStage.STAGE_DANGER, ProjectStatus.STATUS_ACTIVE);

            Invoice i1 = new Invoice("00001", Convert.ToDecimal(1514.01), InvoiceStateValue.closed, new DateTime(2014, 2, 4), new DateTime(2014, 2, 4));
            Invoice i2 = new Invoice("00002", Convert.ToDecimal(2213.21), InvoiceStateValue.open, new DateTime(2016, 4, 17));
            Invoice i3 = new Invoice("00003", Convert.ToDecimal(3216.10), InvoiceStateValue.paid, DateTime.Now.AddDays(1), DateTime.Now.AddDays(-1));
            Invoice i4 = new Invoice("00004", Convert.ToDecimal(4088.56), InvoiceStateValue.open, DateTime.Now.AddMonths(1));


            i1.refreshInvoiceState();
            i2.refreshInvoiceState();
            i3.refreshInvoiceState();
            i4.refreshInvoiceState();

            List<Invoice> InvoiceList = new List<Invoice>();
            InvoiceList.Add(i1);
            InvoiceList.Add(i2);
            InvoiceList.Add(i3);
            InvoiceList.Add(i4);

            ProjectList = new List<Project>();

            p1.invoices = InvoiceList;
            p2.invoices = InvoiceList;
            p3.invoices = InvoiceList;
            p4.invoices = InvoiceList;

            ProjectList.Add(p1);
            ProjectList.Add(p2);
            ProjectList.Add(p3);
            ProjectList.Add(p4);

            projectTable = new UITableView(new CGRect(0, 0, 0, 0), UITableViewStyle.Grouped);
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject indexPath)
        {
            base.PrepareForSegue(segue, indexPath);

            // do first a control on the Identifier for your segue
            if (segue.Identifier.Equals("SegueToProjectDetail"))
            {
                var index = ((NSIndexPath)indexPath).Row;

                var myData = (ProjectDetailController)segue.DestinationViewController;
                myData.thisProject = ProjectList[index];
            }
        }



        /*async*/
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.NavigationController.NavigationBar.TintColor = UIColor.White;
            this.NavigationController.NavigationBar.BarTintColor = UIColor.DarkGray;

            this.Title = "LYCHEE by Johan Group";
            this.NavigationItem.SetHidesBackButton(true, false);
            this.NavigationItem.SetRightBarButtonItem(
                new UIBarButtonItem(UIBarButtonSystemItem.Add, (sender, args) => {
                    // button was clicked

                    UIAlertController actionSheetAlert = UIAlertController.Create("Action Sheet", "Select an item from below", UIAlertControllerStyle.ActionSheet);


                    //About Lychee Selection
                    actionSheetAlert.AddAction(UIAlertAction.Create("About Lychee", UIAlertActionStyle.Default, (action) => {
                        UIAlertView alert = new UIAlertView()
                        {
                            Title = "About Lychee",
                            Message = "Blah blah"
                        };
                        alert.AddButton("OK");
                        alert.Show();
                    }));

                    //Log Out Selection
                    actionSheetAlert.AddAction(UIAlertAction.Create("Log Out", UIAlertActionStyle.Default, (action) => {
                        PerformSegue("UnwindToLoginViewController", this);
                        this.NavigationController.NavigationBar.TintColor = null;
                        this.NavigationController.NavigationBar.BarTintColor = null;
                    }));

                    //Cancel Selection
                    actionSheetAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (actionCancel) => { }));

                    // Required for iPad - You must specify a source for the Action Sheet since it is
                    // displayed as a popover
                    UIPopoverPresentationController presentationPopover = actionSheetAlert.PopoverPresentationController;
                    if (presentationPopover != null)
                    {
                        //presentationPopover.BarButtonItem = this.NavigationItem.RightBarButtonItem;
                        presentationPopover.SourceView = this.View;
                        presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
                    }

                    // Display the alert
                    this.PresentViewController(actionSheetAlert, true, null);
                })
            , true);



            initClientInfo();
            initProjectTable();
        }
        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            projectTable.Frame = new CGRect(
                0,
                UIApplication.SharedApplication.StatusBarFrame.Height + NavigationController.NavigationBar.Bounds.Height + UIScreen.MainScreen.Bounds.Height / 6,
                UIScreen.MainScreen.Bounds.Width,
                UIScreen.MainScreen.Bounds.Height / 1.2);
            View.AddSubview(projectTable);
        }

        void initClientInfo()
        {
            string clientName = "IBM, Canada";
            WelcomeLabel.Text += clientName + ".";
        }
        void initProjectTable()
        {
            projectTable.ScrollEnabled = false;
            var tableViewSource = new ProjectSource(this, ProjectList);
            projectTable.Source = tableViewSource;


            //projectTable.Source = tableViewSource;
        }
    }
}
